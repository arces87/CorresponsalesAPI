using Dapper;
using FBS.DAL.Nomenclador;
using FBS.Identidad.DAL.Modelado;
using FBS.Infraestructura.Repositorio;
using FBSConsolaCBWebApi.DAL;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Repositories.Corresponsales
{
    public class RepositorioTransaccion : Repositorio<Transaccion>, IRepositorioTransaccion
    {
        private readonly IConfiguration _configuracion;
        private readonly IJsonConfiguracion _jsonConfiguracion;

        public RepositorioTransaccion(ContextoFBSConsolaCB context, IConfiguration configuracion, IJsonConfiguracion jsonConfiguracion) : base(context)
        {
            _configuracion = configuracion;
            _jsonConfiguracion = jsonConfiguracion;
        }
        public async Task<IEnumerable<Transaccion>> GetAllActive()
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var Transaccions = await conexion.QueryAsync<Transaccion>("SELECT * FROM Corresponsales.Transaccion where EstaActivo='true'");
                return Transaccions.ToList();
            }
        }
        public async Task<IEnumerable<Transaccion>> GetAllWithAssociations(string IdAgente)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var estadoTransaccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTransferenciaProcesada").Valor;
                if (IdAgente != null && IdAgente != "")
                {
                    var transacciones = await conexion.QueryAsync<Transaccion, Agente, Catalogo, Transaccion>(@"SELECT * FROM Corresponsales.Transaccion transaccion " +
                    "left join Corresponsales.Agente agente on transaccion.AgenteId = agente.Id " +
                    "left join Nomenclador.Catalogo estado on transaccion.EstadoId = estado.Id " +
                    "where transaccion.EstaActivo='true' and agente.Id = @IdAgente and transaccion.EstadoId = @estadoTransaccion",
                   (transaccion, agente, estado) =>
                   {
                       transaccion.Estado = estado;
                       transaccion.Agente = agente;
                       return transaccion;
                   }, param: new { IdAgente, estadoTransaccion });
                    return transacciones.ToList();
                }
                else
                {
                    var transacciones = await conexion.QueryAsync<Transaccion, Agente, Catalogo, Transaccion>(@"SELECT * FROM Corresponsales.Transaccion transaccion " +
                    "left join Corresponsales.Agente agente on transaccion.AgenteId = agente.Id " +
                    "left join Nomenclador.Catalogo estado on transaccion.EstadoId = estado.Id " +
                    "where transaccion.EstaActivo='true' and transaccion.EstadoId = @estadoTransaccion",
                   (transaccion, agente, estado) =>
                   {
                       transaccion.Estado = estado;
                       transaccion.Agente = agente;
                       return transaccion;
                   }, param: new { estadoTransaccion });
                    return transacciones.ToList();
                }
            }
        }
        public async Task<Transaccion> GetWithAssociations(string Id)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var estadoTransaccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTransferenciaProcesada").Valor;
                var transacciones = await conexion.QueryAsync<Transaccion, Agente, Catalogo, Transaccion>(@"SELECT * FROM Corresponsales.Transaccion transaccion " +
                  "left join Corresponsales.Agente agente on transaccion.AgenteId = agente.Id " +
                  "left join Nomenclador.Catalogo estado on transaccion.EstadoId = estado.Id " +
                  "where transaccion.EstaActivo='true' and transaccion.Id = @Id and transaccion.EstadoId = @estadoTransaccion",
                    (transaccion, agente, estado) =>
                    {
                        transaccion.Estado = estado;
                        transaccion.Agente = agente;
                        return transaccion;
                    }, param: new { Id, estadoTransaccion });

                return transacciones.FirstOrDefault();
            }
        }
        public async Task<double> GetSaldoActual(string IdAgente)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var estadoTransaccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTransferenciaProcesada").Valor;
                var transacciones = await conexion.QueryAsync<Transaccion>(@"SELECT * FROM Corresponsales.Transaccion transaccion " +
                  "where transaccion.EstaActivo='true' and transaccion.ReposicionRealizada = 'false' and transaccion.AgenteId = @IdAgente and transaccion.EstadoId = @estadoTransaccion " +
                  "order by transaccion.FechaSistema",
                  param: new { IdAgente, estadoTransaccion });
                var ultimaTransaccion = transacciones.LastOrDefault();
                if (ultimaTransaccion != null)
                {
                    return ultimaTransaccion.SaldoDisponible;
                }
                return 0;
            }
        }
        public async Task<double> GetSaldoCuenta(string IdAgente)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var estadoTransaccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTransferenciaProcesada").Valor;
                var transacciones = await conexion.QueryAsync<Transaccion>(@"SELECT * FROM Corresponsales.Transaccion transaccion " +
                  "where transaccion.EstaActivo='true' " +
                  "and transaccion.ReposicionRealizada = 'false' " +
                  "and transaccion.AgenteId = @IdAgente " +
                  "and transaccion.EstadoId = @estadoTransaccion " +
                  "order by transaccion.FechaSistema",
                  param: new { IdAgente, estadoTransaccion });
                var ultimaTransaccion = transacciones.LastOrDefault();
                if (ultimaTransaccion != null)
                {
                    return ultimaTransaccion.SaldoCuenta;
                }
                return 0;
            }
        }

        public override async Task<Transaccion> Get(string Id)
        {
            return await Context.Transacciones.FirstOrDefaultAsync(d => d.Id.ToString() == Id);
        }

        public override async Task Remove(Transaccion entidad)
        {
            var catalogo = _contexto.Set<Transaccion>().FirstOrDefault(o => o.Id == entidad.Id);
            catalogo.EstaActivo = false;
            await _contexto.SaveChangesAsync();
        }
        public override async Task<string> Add(Transaccion entidad)
        {
            entidad.Estado = Context.Catalogos.FirstOrDefault(c => c.Id == entidad.Estado.Id);
            entidad.Agente = Context.Agentes.FirstOrDefault(c => c.Id == entidad.Agente.Id);
            entidad.EstaActivo = true;
            Context.Transacciones.Add(entidad);
            await Context.SaveChangesAsync();
            return entidad.Id.ToString();
        }

        public override async Task Update(Transaccion entidad)
        {
            entidad.Estado = Context.Catalogos.FirstOrDefault(c => c.Id == entidad.Estado.Id);
            entidad.Agente = Context.Agentes.FirstOrDefault(c => c.Id == entidad.Agente.Id);
            _contexto.Entry(entidad).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }
        public async Task ReponerTransaccion(string IdAgente)
        {
            var transacciones = Context.Transacciones.Where(t => t.ReposicionRealizada == false && t.Agente.Id.ToString() == IdAgente);
            if (transacciones.Count() > 0)
                foreach (var transaccion in transacciones)
                {
                    transaccion.ReposicionRealizada = true;
                }
            Context.Transacciones.UpdateRange(transacciones);
            await _contexto.SaveChangesAsync();
        }

        public async Task<IEnumerable<Transaccion>> GetForAgente(string Id)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var estadoTransaccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTransferenciaProcesada").Valor;
                var transacciones = await conexion.QueryAsync<Transaccion, Agente, Catalogo, Transaccion>(@"SELECT * FROM Corresponsales.Transaccion transaccion " +
                  "left join Corresponsales.Agente agente on transaccion.AgenteId = agente.Id " +
                  "left join Nomenclador.Catalogo estado on transaccion.EstadoId = estado.Id " +
                  "where transaccion.EstaActivo='true' and agente.Id = @Id and transaccion.EstadoId = @estadoTransaccion",
                    (transaccion, agente, estado) =>
                    {
                        transaccion.Estado = estado;
                        transaccion.Agente = agente;
                        return transaccion;
                    }, param: new { Id, estadoTransaccion });

                return transacciones.ToList();
            }
        }

        public async Task<IEnumerable<Transaccion>> GetForTipo(string idTipo, string IdAgente)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var estadoTransaccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTransferenciaProcesada").Valor;
                var transacciones = await conexion.QueryAsync<Transaccion, Agente, Catalogo, Transaccion>(@"SELECT * FROM Corresponsales.Transaccion transaccion " +
                  "left join Corresponsales.Agente agente on transaccion.AgenteId = agente.Id " +
                  "left join Nomenclador.Catalogo estado on transaccion.EstadoId = estado.Id " +
                  "where transaccion.EstaActivo='true' and transaccion.Tipo = @idTipo and agente.Id = @IdAgente " +
                  "and transaccion.ReposicionRealizada = 'false' and transaccion.EstadoId = @estadoTransaccion",
                    (transaccion, agente, estado) =>
                    {
                        transaccion.Estado = estado;
                        transaccion.Agente = agente;
                        return transaccion;
                    }, param: new { idTipo, IdAgente, estadoTransaccion });

                return transacciones.ToList();
            }
        }

        public async Task<int> TransaccionesRepuestas(string IdAgente)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var estadoTransaccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTransferenciaProcesada").Valor;
                var resultado = await conexion.QueryAsync<int>(@"SELECT Count(transaccion.Id) FROM Corresponsales.Transaccion transaccion " +
                  "left join Corresponsales.Agente agente on transaccion.AgenteId = agente.Id " +
                  "left join Nomenclador.Catalogo estado on transaccion.EstadoId = estado.Id " +
                  "where transaccion.EstaActivo='true' and agente.Id = @IdAgente " +
                  "and transaccion.ReposicionRealizada = 'true' and transaccion.EstadoId = @estadoTransaccion", param: new { IdAgente, estadoTransaccion });

                var cantidadTransacciones = resultado.Single();

                return cantidadTransacciones;
            }
        }


        public async Task<int> TransaccionesProcesadas(string IdAgente)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var estadoTransaccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTransferenciaProcesada").Valor;
                var resultado = await conexion.QueryAsync<int>(@"SELECT Count(transaccion.Id) FROM Corresponsales.Transaccion transaccion " +
                  "left join Corresponsales.Agente agente on transaccion.AgenteId = agente.Id " +
                  "left join Nomenclador.Catalogo estado on transaccion.EstadoId = estado.Id " +
                  "where transaccion.EstaActivo='true' and agente.Id = @IdAgente " +
                  "and transaccion.EstadoId = @estadoTransaccion", param: new { IdAgente, estadoTransaccion });

                var cantidadTransacciones = resultado.Single();

                return cantidadTransacciones;
            }
        }

        public ContextoFBSConsolaCB Context => _contexto as ContextoFBSConsolaCB;

        public IDbConnection Conexion => new SqlConnection(_configuracion.GetConnectionString("DapperConnection"));
    }
}
