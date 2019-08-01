using Dapper;
using FBS.Infraestructura.Repositorio;
using FBSConsolaCBWebApi.DAL;
using FBSConsolaCBWebApi.DAL.Nomenclador;
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
        public async Task<IEnumerable<Catalogo>> GetAllWithAssociations()
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var catalogos = await conexion.QueryAsync<Catalogo, TipoCatalogo, Catalogo>(@"SELECT * FROM Nomenclador.Catalogo join Nomenclador.TipoCatalogo " +
                    "on Nomenclador.Catalogo.TipoCatalogoId = Nomenclador.TipoCatalogo.Id where Nomenclador.Catalogo.EstaActivo='true'",
                    (catalogo, tipoCatalogo) =>
                    {
                        catalogo.TipoCatalogo = tipoCatalogo;
                        return catalogo;
                    });
                return catalogos.ToList();
            }
        }
        public async Task<Catalogo> GetWithAssociations(int Id)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var catalogos = await conexion.QueryAsync<Catalogo, TipoCatalogo, Catalogo>(@"SELECT * FROM Nomenclador.Catalogo c join Nomenclador.TipoCatalogo t" +
                    "on c.TipoCatalogoId = t.Id where EstaActivo='true' and Id = @Id",
                    (catalogo, tipoCatalogo) =>
                    {
                        catalogo.TipoCatalogo = tipoCatalogo;
                        return catalogo;
                    }, param: new { Id });

                return catalogos.FirstOrDefault();
            }
        }

        public override async Task Remove(Catalogo entity)
        {
            var catalogo = _contexto.Set<Catalogo>().FirstOrDefault(o => o.Id == entity.Id);
            catalogo.EstaActivo = false;
            await _contexto.SaveChangesAsync();
        }

        public override async Task<string> Add(Catalogo entity)
        {
            entity.TipoCatalogo = Context.TiposCatalogos.FirstOrDefault(c => c.Id == entity.TipoCatalogo.Id);
            entity.EstaActivo = true;
            Context.Catalogos.Add(entity);
            await Context.SaveChangesAsync();
            return entity.Id.ToString();
        }
        public override async Task Update(Catalogo entity)
        {
            entity.TipoCatalogo = Context.TiposCatalogos.FirstOrDefault(c => c.Id == entity.TipoCatalogo.Id);
            _contexto.Entry(entity).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }

        public ContextoFBSConsolaCB Context => _contexto as ContextoFBSConsolaCB;

        public IDbConnection Conexion => new SqlConnection(_configuracion.GetConnectionString("DapperConnection"));
    }
}
