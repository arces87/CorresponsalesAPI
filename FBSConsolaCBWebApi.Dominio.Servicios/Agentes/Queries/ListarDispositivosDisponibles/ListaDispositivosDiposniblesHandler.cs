using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBS.Identidad.DAL.Seguridad;
using FBSConsolaCBWebApi.DAL.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Queries
{
    public class ListaDispositivosDiposniblesHandler : IRequestHandler<ListaDispositivosDiposniblesME, ListaDispositivosDiposniblesMS>
    {
        private readonly IRepositorioAgente _repositorio;
        private readonly IMapper _mapper;

        public ListaDispositivosDiposniblesHandler(IRepositorioAgente repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ListaDispositivosDiposniblesMS> Handle(ListaDispositivosDiposniblesME request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetDispositivosDisponibles();
            var _retorno = new ListaDispositivosDiposniblesMS();
            var totalElementos = 0;
            Filtro<Dispositivo>.ProcesarLista(ref _model, _mapper.Map<ModeloPaginacion>(request), ref totalElementos);
            _retorno.TotalElementos = totalElementos;
            _retorno.Dispositivos = _mapper.Map<List<ModeloListaDispositivosDiposnibles>>(_model);
            _retorno.CantidadElementos = request.CantidadElementos;
            _retorno.Pagina = request.Pagina;
            return _retorno;
        }
    }
}
