using Dapper;
using FBS.DAL.Nomenclador;
using FBS.Infraestructura.Repositorio;
using FBSConsolaCBWebApi.DAL;
using FBSConsolaCBWebApi.DAL.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Repositories.Canales
{
    public class RepositorioImagen : Repositorio<Imagen>, IRepositorioImagen
    {
        private readonly IConfiguration _configuracion;

        public RepositorioImagen(ContextoFBSConsolaCB context, IConfiguration configuracion) : base(context)
        {
            _configuracion = configuracion;
        }
        public async Task<IEnumerable<Imagen>> GetAllActive()
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var Imagens = await conexion.QueryAsync<Imagen>("SELECT * FROM Canales.Imagen where EstaActivo='true'");
                return Imagens.ToList();
            }
        }
        public async Task<IEnumerable<Imagen>> GetAllWithAssociations()
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var imagenes = await conexion.QueryAsync<Imagen, Dispositivo, Imagen>(@"SELECT * FROM Canales.Imagen " +
                    "join Canales.Dispositivo on Canales.Imagen.DispositivoId = Canales.Dispositivo.Id" +
                    " where Canales.Imagen.EstaActivo='true'",
                    (imagen, dispositivo) =>
                    {
                        imagen.Dispositivo = dispositivo;
                        return imagen;
                    });
                return imagenes.ToList();
            }
        }
        public async Task<Imagen> GetWithAssociations(string Id)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var imagenes = await conexion.QueryAsync<Imagen, Dispositivo, Imagen>(@"SELECT * FROM Canales.Imagen " +
                   "join Canales.Dispositivo on Canales.Imagen.DispositivoId = Canales.Dispositivo.Id" +
                   " where Canales.Imagen.EstaActivo='true' and Canales.Imagen.Id = @Id",
                   (imagen, dispositivo) =>
                   {
                       imagen.Dispositivo = dispositivo;
                       return imagen;
                   }, param: new { Id });

                return imagenes.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Imagen>> GetForDispositivo(string IdDispositivo)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var imagenes = await conexion.QueryAsync<Imagen, Dispositivo, Imagen>(@"SELECT * FROM Canales.Imagen " +
                   "join Canales.Dispositivo on Canales.Imagen.DispositivoId = Canales.Dispositivo.Id" +
                   " where Canales.Imagen.EstaActivo='true' and Canales.Imagen.DispositivoId = @IdDispositivo",
                   (imagen, dispositivo) =>
                   {
                       imagen.Dispositivo = dispositivo;
                       return imagen;
                   }, param: new { IdDispositivo });

                return imagenes.ToList();
            }
        }

        public override async Task Remove(Imagen entidad)
        {
            var catalogo = _contexto.Set<Imagen>().FirstOrDefault(o => o.Id == entidad.Id);
            catalogo.EstaActivo = false;
            await _contexto.SaveChangesAsync();
        }
        public async Task RemoveAllForDispositivo(string IdDispositivo)
        {
            var imagenes = Context.Imagenes.Where(i => i.Dispositivo.Id.ToString() == IdDispositivo).ToList();
            Context.Imagenes.RemoveRange(imagenes);
            await _contexto.SaveChangesAsync();
        }

        public override async Task<string> Add(Imagen entidad)
        {
            entidad.Dispositivo = Context.Dispositivos.FirstOrDefault(c => c.Id == entidad.Dispositivo.Id);
            entidad.EstaActivo = true;
            Context.Imagenes.Add(entidad);
            await Context.SaveChangesAsync();
            return entidad.Id.ToString();
        }
        public override async Task Update(Imagen entidad)
        {
            entidad.Dispositivo = Context.Dispositivos.FirstOrDefault(c => c.Id == entidad.Dispositivo.Id);
            _contexto.Entry(entidad).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }

        public ContextoFBSConsolaCB Context => _contexto as ContextoFBSConsolaCB;

        public IDbConnection Conexion => new SqlConnection(_configuracion.GetConnectionString("DapperConnection"));
    }
}
