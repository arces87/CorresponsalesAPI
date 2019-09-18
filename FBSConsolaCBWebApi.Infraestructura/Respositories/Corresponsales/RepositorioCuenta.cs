using Dapper;
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
    public class RepositorioCuenta : Repositorio<Cuenta>, IRepositorioCuenta
    {

        private readonly IConfiguration _configuracion;

        public RepositorioCuenta(ContextoFBSConsolaCB context, IConfiguration configuracion) : base(context)
        {
            _configuracion = configuracion;
        }
        public async Task<IEnumerable<Cuenta>> GetAllActive()
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var cuentas = await conexion.QueryAsync<Cuenta>("SELECT * FROM Corresponsales.Cuenta where EstaActivo='true'");
                return cuentas.ToList();
            }
        }
        public async Task<IEnumerable<Cuenta>> GetAllWithAssociations()
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var cuentas = await conexion.QueryAsync<Cuenta, Agente, Cuenta>(@"SELECT * FROM Corresponsales.Cuenta " +
                    "join Corresponsales.Agente on Corresponsales.Cuenta.AgenteId = Corresponsales.Agente.Id " +
                    "where Corresponsales.Cuenta.EstaActivo='true'",
                    (cuenta, agente) =>
                    {
                        cuenta.Agente = agente;
                        return cuenta;
                    });
                return cuentas.ToList();
            }
        }
        public async Task<Cuenta> GetWithAssociations(string Id)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var cuentas = await conexion.QueryAsync<Cuenta, Agente, Cuenta>(@"SELECT * FROM Corresponsales.Cuenta " +
                    "join Corresponsales.Agente on Corresponsales.Cuenta.AgenteId = Corresponsales.Agente.Id " +
                    "where Corresponsales.Cuenta.EstaActivo='true' and Corresponsales.Cuenta.Id = @Id",
                    (cuenta, agente) =>
                    {
                        cuenta.Agente = agente;
                        return cuenta;
                    }, param: new { Id });

                return cuentas.FirstOrDefault();
            }
        }

        public async Task<Cuenta> GetForAgente(string IdAgente)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var cuentas = await conexion.QueryAsync<Cuenta, Agente, Cuenta>(@"SELECT * FROM Corresponsales.Cuenta " +
                    "join Corresponsales.Agente on Corresponsales.Cuenta.AgenteId = Corresponsales.Agente.Id " +
                    "where Corresponsales.Cuenta.EstaActivo='true' and Corresponsales.Cuenta.AgenteId = @IdAgente",
                    (cuenta, agente) =>
                    {
                        cuenta.Agente = agente;
                        return cuenta;
                    }, param: new { IdAgente });
                return cuentas.FirstOrDefault();
            }
        }
        public override async Task<Cuenta> Get(string Id)
        {
            return await Context.Cuentas.FirstOrDefaultAsync(d => d.Id.ToString() == Id);
        }

        public override async Task Remove(Cuenta entidad)
        {
            var cuenta = _contexto.Set<Cuenta>().FirstOrDefault(o => o.Id == entidad.Id);
            cuenta.EstaActivo = false;
            await _contexto.SaveChangesAsync();
        }

        public override async Task<string> Add(Cuenta entidad)
        {
            entidad.Agente = Context.Agentes.FirstOrDefault(c => c.Id == entidad.Agente.Id);
            entidad.EstaActivo = true;
            Context.Cuentas.Add(entidad);
            await Context.SaveChangesAsync();
            return entidad.Id.ToString();
        }
        public override async Task Update(Cuenta entidad)
        {
            entidad.Agente = Context.Agentes.FirstOrDefault(c => c.Id == entidad.Agente.Id);
            _contexto.Entry(entidad).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }

        

        public ContextoFBSConsolaCB Context => _contexto as ContextoFBSConsolaCB;

        public IDbConnection Conexion => new SqlConnection(_configuracion.GetConnectionString("DapperConnection"));
    }
}
