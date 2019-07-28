using Dapper;
using FBS.Infraestructura.Repositorio;
using FBSConsolaCBWebApi.DAL;
using FBSConsolaCBWebApi.DAL.Consola;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCBWebApi.DAL.Nomenclador;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Repositories.Consola
{
    public class RepositorioDispositivo : Repositorio<Dispositivo>, IRepositorioDispositivo
    {
        private readonly IConfiguration _configuracion;
        public RepositorioDispositivo(ContextoFBSConsolaCB context, IConfiguration configuracion) : base(context)
        {
            _configuracion = configuracion;
        }

        public async Task<IEnumerable<Dispositivo>> GetAllActive()
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var dispositivos = await conexion.QueryAsync<Dispositivo>("SELECT * FROM Consola.Dispositivo where Consola.Dispositivo.EstaActivo='true'");
                return dispositivos;
            }
        }
        public async Task<IEnumerable<Dispositivo>> GetAllWithAssociations()
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var dispositivos = await conexion
                    .QueryAsync<Dispositivo, Catalogo, Dispositivo>(@"SELECT * FROM Consola.Dispositivo d 
                        join Nomenclador.Catalogo t on d.TipoDispositivoId = t.Id where d.EstaActivo='true'",
                    (dispositivo, tipoCatalogo) =>
                    {
                        dispositivo.TipoDispositivo = tipoCatalogo;
                        return dispositivo;
                    });
                return dispositivos;
            }
        }

        public override async Task<Dispositivo> Get(int Id)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var dispositivos = await conexion.QueryAsync<Dispositivo>("SELECT * FROM Consola.Dispositivo where EstaActivo='true' and Id = @Id", param: new { Id });
                return dispositivos.FirstOrDefault();
            }
        }

        public override async Task Add(Dispositivo entity)
        {
            entity.TipoDispositivo = Context.Catalogos.FirstOrDefault(c => c.Id == entity.TipoDispositivo.Id);
            entity.EstaActivo = true;
            Context.Dispositivos.Add(entity);
            await Context.SaveChangesAsync();
        }
        public override async Task Update(Dispositivo entity)
        {
            entity.TipoDispositivo = Context.Catalogos.FirstOrDefault(c => c.Id == entity.TipoDispositivo.Id);
            _contexto.Entry(entity).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();

        }

        public async Task<Dispositivo> GetWithAssociations(int Id)
        {
            return await _contexto.Set<Dispositivo>().Where(a => a.EstaActivo == true)
               .Include(d => d.TipoDispositivo).FirstOrDefaultAsync(c => c.Id == Id);
        }

        public override async Task Remove(Dispositivo entity)
        {
            entity.EstaActivo = false;
            _contexto.Entry(entity).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }

        public async Task Asignar(int idDispositivo, int idCorresponsal)
        {
            var dispositivo = Context.Dispositivos.FirstOrDefault(d => d.Id == idDispositivo);
            var corresponsal = Context.Corresponsales.FirstOrDefault(c => c.Id == idCorresponsal);
            _contexto.Set<DispositivoCorresponsal>().Add(new DispositivoCorresponsal() { Dispositivo = dispositivo, Corresponsal = corresponsal });
            await _contexto.SaveChangesAsync();
        }
        public async Task Desasignar(int idDispositivo, int idCorresponsal)
        {
            var asignacion = Context.DispositivosCorresponsales.FirstOrDefault(d => d.Dispositivo.Id == idDispositivo && d.Corresponsal.Id == idCorresponsal);
            _contexto.Set<DispositivoCorresponsal>().Remove(asignacion);
            await _contexto.SaveChangesAsync();
        }

        public async Task<Corresponsal> ObtenerCorresponsal(int idDispositivo)
        {
            var dispositivo = await Context.DispositivosCorresponsales.FirstOrDefaultAsync(c => c.Dispositivo.Id == idDispositivo);
            if (dispositivo != null)
                return await Context.Corresponsales.FirstOrDefaultAsync(d => d.Id == dispositivo.Corresponsal.Id);
            return null;
        }

        public ContextoFBSConsolaCB Context => _contexto as ContextoFBSConsolaCB;

        public IDbConnection Conexion => new SqlConnection(_configuracion.GetConnectionString("DapperConnection"));
    }
}
