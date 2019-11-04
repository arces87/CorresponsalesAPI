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
                var dispositivos = await conexion.QueryAsync<Dispositivo>("SELECT * FROM Canales.Dispositivo where EstaActivo='true'");
                return dispositivos.ToList();
            }
        }
        public async Task<IEnumerable<Dispositivo>> GetAllWithAssociations(bool? activo)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                if (activo != null)
                {
                    var dispositivos = await conexion.QueryAsync<Dispositivo, Catalogo, Catalogo, Dispositivo>(@"SELECT * FROM Canales.Dispositivo " +
                  "join Nomenclador.Catalogo marca on Canales.Dispositivo.MarcaId = marca.Id " +
                  "join Nomenclador.Catalogo sistemaOperativo on Canales.Dispositivo.SistemaOperativoId = sistemaOperativo.Id " +
                  "where Canales.Dispositivo.EstaActivo=@activo",
                  (dispositivo, marca, sistemaOperativo) =>
                  {
                      dispositivo.Marca = marca;
                      dispositivo.SistemaOperativo = sistemaOperativo;
                      return dispositivo;
                  }, param: new { activo });
                    return dispositivos.ToList();
                }
                else
                {
                    var dispositivos = await conexion.QueryAsync<Dispositivo, Catalogo, Catalogo, Dispositivo>(@"SELECT * FROM Canales.Dispositivo " +
                   "join Nomenclador.Catalogo marca on Canales.Dispositivo.MarcaId = marca.Id " +
                   "join Nomenclador.Catalogo sistemaOperativo on Canales.Dispositivo.SistemaOperativoId = sistemaOperativo.Id",
                   (dispositivo, marca, sistemaOperativo) =>
                   {
                       dispositivo.Marca = marca;
                       dispositivo.SistemaOperativo = sistemaOperativo;
                       return dispositivo;
                   });
                    return dispositivos.ToList();
                }
            }
        }
        public async Task<Dispositivo> GetWithAssociations(string Id)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var dispositivos = await conexion.QueryAsync<Dispositivo, Catalogo, Catalogo, Dispositivo>(@"SELECT * FROM Canales.Dispositivo " +
                    "join Nomenclador.Catalogo marca on Canales.Dispositivo.MarcaId = marca.Id " +
                    "join Nomenclador.Catalogo sistemaOperativo on Canales.Dispositivo.SistemaOperativoId = sistemaOperativo.Id " +
                    "where Canales.Dispositivo.EstaActivo='true' and Canales.Dispositivo.Id = @Id",
                    (dispositivo, marca, sistemaOperativo) =>
                    {
                        dispositivo.Marca = marca;
                        dispositivo.SistemaOperativo = sistemaOperativo;
                        return dispositivo;
                    }, param: new { Id });

                return dispositivos.FirstOrDefault();
            }
        }

        public override async Task<Dispositivo> Get(string Id)
        {
            return await Context.Dispositivos.FirstOrDefaultAsync(d => d.Id.ToString() == Id);
        }

        public override async Task Remove(Dispositivo entidad)
        {
            var dispositivo = _contexto.Set<Dispositivo>().FirstOrDefault(o => o.Id == entidad.Id);
            dispositivo.EstaActivo = false;
            _contexto.Entry(dispositivo).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }

        public override async Task<string> Add(Dispositivo entidad)
        {
            entidad.Marca = Context.Catalogos.FirstOrDefault(c => c.Id == entidad.Marca.Id);
            entidad.SistemaOperativo = Context.Catalogos.FirstOrDefault(c => c.Id == entidad.SistemaOperativo.Id);
            entidad.EstaActivo = true;
            Context.Dispositivos.Add(entidad);
            await Context.SaveChangesAsync();
            return entidad.Id.ToString();
        }
        public override async Task Update(Dispositivo entidad)
        {
            entidad.Marca = Context.Catalogos.FirstOrDefault(c => c.Id == entidad.Marca.Id);
            entidad.SistemaOperativo = Context.Catalogos.FirstOrDefault(c => c.Id == entidad.SistemaOperativo.Id);
            _contexto.Entry(entidad).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }

        public async Task<bool> VerificarDispositivo(string Marca, string Modelo, string NoSerie)
        {
            var dispositivo = await Context.Dispositivos.FirstOrDefaultAsync(d => d.Marca.Nombre == Marca
                                                                    && d.Modelo.Replace(" ", "").ToLower() == Modelo.Replace(" ", "").ToLower()
                                                                    && d.NumeroSerie.Replace(" ", "").ToLower() == NoSerie.Replace(" ", "").ToLower());
            return dispositivo != null ? true : false;
        }
        public async Task<bool> VerificarDispositivo(string Marca, string Modelo, string NoSerie, string IdDispositivo)
        {
            var dispositivo = await Context.Dispositivos.FirstOrDefaultAsync(d => d.Marca.Nombre == Marca
                                                                    && d.Modelo.Replace(" ", "").ToLower() == Modelo.Replace(" ", "").ToLower()
                                                                    && d.NumeroSerie.Replace(" ", "").ToLower() == NoSerie.Replace(" ", "").ToLower()
                                                                    && d.Id.ToString() != IdDispositivo);
            return dispositivo != null ? true : false;
        }

        public ContextoFBSConsolaCB Context => _contexto as ContextoFBSConsolaCB;

        public IDbConnection Conexion => new SqlConnection(_configuracion.GetConnectionString("DapperConnection"));
    }
}
