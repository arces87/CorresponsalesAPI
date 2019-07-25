using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Oficinas.Commands
{
    public class ModificarOficinaCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Ciudad { get; set; }
        public int IdEmpresa { get; set; }
    }
}
