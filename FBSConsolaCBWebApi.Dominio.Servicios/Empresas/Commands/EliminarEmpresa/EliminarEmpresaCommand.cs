using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Empresas.Commands
{
    public class EliminarEmpresaCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
