using Dapper;
using FBS.DAL.Nomenclador;
using FBS.Infraestructura.Repositorio;
using FBSConsolaCBWebApi.DAL;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Repositories.Nomenclador
{
    public class RepositorioCatalogo : Repositorio<Catalogo>, IRepositorioCatalogo
    {
        private readonly IConfiguration _configuracion;

        public RepositorioCatalogo(ContextoFBSConsolaCB context, IConfiguration configuracion) : base(context)
        {
            _configuracion = configuracion;
        }
        public async Task<IEnumerable<Catalogo>> GetAllActive()
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var catalogos = await conexion.QueryAsync<Catalogo>("SELECT * FROM Nomenclador.Catalogo where EstaActivo='true'");
                return catalogos.ToList();
            }
        }
        public async Task<IEnumerable<Catalogo>> GetAllWithAssociations(bool? Activo)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                if (Activo != null)
                {
                    var catalogos = await conexion.QueryAsync<Catalogo, TipoCatalogo, Catalogo>(@"SELECT * FROM Nomenclador.Catalogo join Nomenclador.TipoCatalogo " +
                    "on Nomenclador.Catalogo.TipoCatalogoId = Nomenclador.TipoCatalogo.Id " +
                    "where Nomenclador.Catalogo.EstaActivo=@Activo",
                    (catalogo, tipoCatalogo) =>
                    {
                        catalogo.TipoCatalogo = tipoCatalogo;
                        return catalogo;
                    }, param: new { Activo });
                    return catalogos.ToList();
                }
                else
                {
                    var catalogos = await conexion.QueryAsync<Catalogo, TipoCatalogo, Catalogo>(@"SELECT * FROM Nomenclador.Catalogo join Nomenclador.TipoCatalogo " +
                    "on Nomenclador.Catalogo.TipoCatalogoId = Nomenclador.TipoCatalogo.Id",
                    (catalogo, tipoCatalogo) =>
                    {
                        catalogo.TipoCatalogo = tipoCatalogo;
                        return catalogo;
                    });
                    return catalogos.ToList();
                }
            }
        }
        public async Task<Catalogo> GetWithAssociations(string Id)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var catalogos = await conexion.QueryAsync<Catalogo, TipoCatalogo, Catalogo>(@"SELECT * FROM Nomenclador.Catalogo " +
                    "join Nomenclador.TipoCatalogo on Nomenclador.Catalogo.TipoCatalogoId = Nomenclador.TipoCatalogo.Id " +
                    "where Nomenclador.Catalogo.EstaActivo='true' and Nomenclador.Catalogo.Id = @Id",
                    (catalogo, tipoCatalogo) =>
                    {
                        catalogo.TipoCatalogo = tipoCatalogo;
                        return catalogo;
                    }, param: new { Id });

                return catalogos.FirstOrDefault();
            }
        }

        public override async Task Remove(Catalogo entidad)
        {
            var catalogo = _contexto.Set<Catalogo>().FirstOrDefault(o => o.Id == entidad.Id);
            catalogo.EstaActivo = false;
            await _contexto.SaveChangesAsync();
        }

        public override async Task<string> Add(Catalogo entidad)
        {
            entidad.TipoCatalogo = Context.TiposCatalogo.FirstOrDefault(c => c.Id == entidad.TipoCatalogo.Id);
            entidad.EstaActivo = true;
            Context.Catalogos.Add(entidad);
            await Context.SaveChangesAsync();
            return entidad.Id.ToString();
        }
        public override async Task Update(Catalogo entidad)
        {
            entidad.TipoCatalogo = Context.TiposCatalogo.FirstOrDefault(c => c.Id == entidad.TipoCatalogo.Id);
            _contexto.Entry(entidad).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }

        public ContextoFBSConsolaCB Context => _contexto as ContextoFBSConsolaCB;

        public IDbConnection Conexion => new SqlConnection(_configuracion.GetConnectionString("DapperConnection"));
    }
}
