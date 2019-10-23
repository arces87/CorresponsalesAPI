using Dapper;
using FBS.DAL.Nomenclador;
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

        public RepositorioTransaccion(ContextoFBSConsolaCB context, IConfiguration configuracion) : base(context)
        {
            _configuracion = configuracion;
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
                if (IdAgente != null && IdAgente != "")
                {
                    var transacciones = await conexion.QueryAsync<Transaccion, Agente, Catalogo, Catalogo, Transaccion>(@"SELECT * FROM Corresponsales.Transaccion transaccion " +
                    "left join Corresponsales.Agente agente on transaccion.AgenteId = agente.Id " +
                    "left join Nomenclador.Catalogo estado on transaccion.EstadoId = estado.Id " +
                    "left join Nomenclador.Catalogo tipo on transaccion.Tipo = tipo.Id " +
                    "where transaccion.EstaActivo='true' and agente.Id = @IdAgente",
                   (transaccion, agente, estado, tipo) =>
                   {
                       transaccion.Estado = estado;
                       transaccion.Agente = agente;
                       transaccion.Tipo = tipo.Nombre;
                       return transaccion;
                   }, param: new { IdAgente });
                    return transacciones.ToList();
                }
                else
                {
                    var transacciones = await conexion.QueryAsync<Transaccion, Agente, Catalogo, Catalogo, Transaccion>(@"SELECT * FROM Corresponsales.Transaccion transaccion " +
                      "left join Corresponsales.Agente agente on transaccion.AgenteId = agente.Id " +
                      "left join Nomenclador.Catalogo estado on transaccion.EstadoId = estado.Id " +
                      "left join Nomenclador.Catalogo tipo on transaccion.Tipo = tipo.Id " +
                      "where transaccion.EstaActivo='true'",
                     (transaccion, agente, estado, tipo) =>
                     {
                         transaccion.Estado = estado;
                         transaccion.Agente = agente;
                         transaccion.Tipo = tipo.Nombre;
                         return transaccion;
                     }
                    );
                    return transacciones.ToList();
                }
            }
        }
        public async Task<Transaccion> GetWithAssociations(string Id)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var transacciones = await conexion.QueryAsync<Transaccion, Agente, Catalogo, Transaccion>(@"SELECT * FROM Corresponsales.Transaccion transaccion " +
                  "left join Corresponsales.Agente agente on transaccion.AgenteId = agente.Id " +
                  "left join Nomenclador.Catalogo estado on transaccion.EstadoId = estado.Id " +
                  "where transaccion.EstaActivo='true' and transaccion.Id = @Id",
                    (transaccion, agente, estado) =>
                    {
                        transaccion.Estado = estado;
                        transaccion.Agente = agente;
                        return transaccion;
                    }, param: new { Id });

                return transacciones.FirstOrDefault();
            }
        }
        public async Task<double> GetSaldoActual(string IdAgente)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var transacciones = await conexion.QueryAsync<Transaccion>(@"SELECT * FROM Corresponsales.Transaccion transaccion " +
                  "where transaccion.EstaActivo='true' and transaccion.ReposicionRealizada = 'false' and transaccion.AgenteId = @IdAgente " +
                  "order by transaccion.FechaSistema",
                  param: new { IdAgente });
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
                var transacciones = await conexion.QueryAsync<Transaccion>(@"SELECT * FROM Corresponsales.Transaccion transaccion " +
                  "where transaccion.EstaActivo='true' and transaccion.ReposicionRealizada = 'false' and transaccion.AgenteId = @IdAgente " +
                  "order by transaccion.FechaSistema",
                  param: new { IdAgente });
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

        public async Task<IEnumerable<Transaccion>> GetForAgente(string Id)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var transacciones = await conexion.QueryAsync<Transaccion, Agente, Catalogo, Transaccion>(@"SELECT * FROM Corresponsales.Transaccion transaccion " +
                  "left join Corresponsales.Agente agente on transaccion.AgenteId = agente.Id " +
                  "left join Nomenclador.Catalogo estado on transaccion.EstadoId = estado.Id " +
                  "where transaccion.EstaActivo='true' and agente.Id = @Id",
                    (transaccion, agente, estado) =>
                    {
                        transaccion.Estado = estado;
                        transaccion.Agente = agente;
                        return transaccion;
                    }, param: new { Id });

                return transacciones.ToList();
            }
        }

        public async Task<IEnumerable<Transaccion>> GetForTipo(string idTipo, string IdAgente)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var transacciones = await conexion.QueryAsync<Transaccion, Agente, Catalogo, Transaccion>(@"SELECT * FROM Corresponsales.Transaccion transaccion " +
                  "left join Corresponsales.Agente agente on transaccion.AgenteId = agente.Id " +
                  "left join Nomenclador.Catalogo estado on transaccion.EstadoId = estado.Id " +
                  "where transaccion.EstaActivo='true' and transaccion.Tipo = @idTipo and agente.Id = @IdAgente " +
                  "and transaccion.ReposicionRealizada = 'false'",
                    (transaccion, agente, estado) =>
                    {
                        transaccion.Estado = estado;
                        transaccion.Agente = agente;
                        return transaccion;
                    }, param: new { idTipo, IdAgente });

                return transacciones.ToList();
            }
        }

        public ContextoFBSConsolaCB Context => _contexto as ContextoFBSConsolaCB;

        public IDbConnection Conexion => new SqlConnection(_configuracion.GetConnectionString("DapperConnection"));
    }
}
