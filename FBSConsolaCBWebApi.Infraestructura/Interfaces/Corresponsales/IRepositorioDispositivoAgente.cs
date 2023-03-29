using FBS.Identidad.DAL.Seguridad;
using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.Canales;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales
{
    public interface IRepositorioDispositivoAgente : IRepositorio<DispositivoAgente>
    {
        Task<IEnumerable<DispositivoAgente>> GetAllAsignado();
        Task<DispositivoAgente> Get(Guid id);       
        Task<IEnumerable<Dispositivo>> GetDispositivosDisponibles();       
        Task<DispositivoAgente> GetForAgente(string Id);
        Task<DispositivoAgente> GetForDispositivo(string Id);       
    }
}
