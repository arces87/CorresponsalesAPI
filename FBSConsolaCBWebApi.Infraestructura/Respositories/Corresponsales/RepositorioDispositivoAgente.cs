
using Dapper;
using FBS.DAL.Nomenclador;
using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.DAL.Seguridad;
using FBS.Infraestructura.Repositorio;
using FBSConsolaCBWebApi.DAL;
using FBSConsolaCBWebApi.DAL.Canales;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.DAL.ModeloUsuario;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Repositories.Corresponsales
{
    public class RepositorioDispositivoAgente : Repositorio<DispositivoAgente>, IRepositorioDispositivoAgente
    {
        private readonly IConfiguration _configuracion;
        private readonly IJsonConfiguracion _jsonConfiguracion;

        public RepositorioDispositivoAgente(ContextoFBSConsolaCB context, IConfiguration configuracion, IJsonConfiguracion jsonConfiguracion) : base(context)
        {
            _configuracion = configuracion;
            _jsonConfiguracion = jsonConfiguracion;
        }
        public async Task<IEnumerable<DispositivoAgente>> GetAllAsignado()
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var DispositivoAgente = await conexion.QueryAsync<DispositivoAgente>("SELECT * FROM Corresponsales.DispositivoAgente");
                return DispositivoAgente.ToList();
            }
        }
               

        public async Task<DispositivoAgente> Get(Guid Id)
        {
            return await Context.DispositivoAgente.FirstOrDefaultAsync(d => d.Id == Id);
        }


        public async Task<IEnumerable<Dispositivo>> GetDispositivosDisponibles()
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var dispositivos = await conexion.QueryAsync<Dispositivo>(@"SELECT * FROM Canales.Dispositivo, DispositivoAgente " +
                    "left join Corresponsales.DispositivoAgente on Canales.Dispositivo.Id = Corresponsales.DispositivoAgente.DispositivoId " +                    
                    "where Canales.Dispositivo.EstaActivo='true'");
                return dispositivos.ToList();
            }
        }

        public async Task<DispositivoAgente> GetForAgente(string agenteId)
        {
            return await Context.DispositivoAgente.FirstOrDefaultAsync(d => d.AgenteId == Guid.Parse(agenteId));
        }

        public async Task<DispositivoAgente> GetForDispositivo(string dispositivoId)
        {
            return await Context.DispositivoAgente.FirstOrDefaultAsync(d => d.DispositivoId == Guid.Parse(dispositivoId));
        }        
              
        public ContextoFBSConsolaCB Context => _contexto as ContextoFBSConsolaCB;

        public IDbConnection Conexion => new SqlConnection(_configuracion.GetConnectionString("DapperConnection"));
    }
}
