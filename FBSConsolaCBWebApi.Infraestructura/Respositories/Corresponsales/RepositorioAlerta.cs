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
    public class RepositorioAlerta : Repositorio<Alerta>, IRepositorioAlerta
    {
        private readonly IConfiguration _configuracion;

        public RepositorioAlerta(ContextoFBSConsolaCB context, IConfiguration configuracion) : base(context)
        {
            _configuracion = configuracion;
        }
        public async Task<IEnumerable<Alerta>> GetAllActive()
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var Alertas = await conexion.QueryAsync<Alerta>("SELECT * FROM Corresponsales.Alerta where EstaActivo='true'");
                return Alertas.ToList();
            }
        }
        public async Task<IEnumerable<Alerta>> GetAllWithAssociations()
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var alertas = await conexion.QueryAsync<Alerta, Catalogo, Agente, Catalogo, Alerta>(@"SELECT * FROM Corresponsales.Alerta alerta " +
                    "left join Nomenclador.Catalogo estado on alerta.EstadoId = estado.Id " +
                    "left join Corresponsales.Agente agente on alerta.AgenteId = agente.Id " +
                    "left join Nomenclador.Catalogo tipo on alerta.TipoId = tipo.Id " +
                    "where alerta.EstaActivo='true'",
                   (alerta, estado, agente, tipo) =>
                   {
                       alerta.Estado = estado;
                       alerta.Agente = agente;
                       alerta.Tipo = tipo;
                       return alerta;
                   });
                return alertas.ToList();
            }
        }
        public async Task<Alerta> GetWithAssociations(string Id)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var alertas = await conexion.QueryAsync<Alerta, Catalogo, Agente, Catalogo, Alerta>(@"SELECT * FROM Corresponsales.Alerta alerta " +
                    "left join Nomenclador.Catalogo estado on alerta.EstadoId = estado.Id " +
                    "left join Corresponsales.Agente agente on alerta.AgenteId = agente.Id " +
                    "left join Nomenclador.Catalogo tipo on alerta.TipoId = tipo.Id " +
                    "where alerta.EstaActivo='true' and alerta.Id = @Id",
                   (alerta, estado, agente, tipo) =>
                   {
                       alerta.Estado = estado;
                       alerta.Agente = agente;
                       alerta.Tipo = tipo;
                       return alerta;
                   }, param: new { Id });

                return alertas.FirstOrDefault();
            }
        }

        public override async Task<Alerta> Get(string Id)
        {
            return await Context.Alertas.FirstOrDefaultAsync(d => d.Id.ToString() == Id);
        }

        public override async Task Remove(Alerta entidad)
        {
            var catalogo = _contexto.Set<Alerta>().FirstOrDefault(o => o.Id == entidad.Id);
            catalogo.EstaActivo = false;
            await _contexto.SaveChangesAsync();
        }
        public override async Task<string> Add(Alerta entidad)
        {
            entidad.Estado = Context.Catalogos.FirstOrDefault(c => c.Id == entidad.Estado.Id);
            entidad.Agente = Context.Agentes.FirstOrDefault(c => c.Id == entidad.Agente.Id);
            entidad.Tipo = Context.Catalogos.FirstOrDefault(c => c.Id == entidad.Tipo.Id);
            entidad.EstaActivo = true;
            Context.Alertas.Add(entidad);
            await Context.SaveChangesAsync();
            return entidad.Id.ToString();
        }

        public override async Task Update(Alerta entidad)
        {
            var alerta = Context.Alertas.FirstOrDefault(a => a.Id == entidad.Id);
            alerta.Estado = Context.Catalogos.FirstOrDefault(c => c.Id == entidad.Estado.Id);
            alerta.Comentario = entidad.Comentario;
            _contexto.Entry(alerta).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }


        public ContextoFBSConsolaCB Context => _contexto as ContextoFBSConsolaCB;

        public IDbConnection Conexion => new SqlConnection(_configuracion.GetConnectionString("DapperConnection"));
    }
}
