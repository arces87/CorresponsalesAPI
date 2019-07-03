using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Dominio.Modelos.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Dominio.Servicios.Interfaces.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Dominio.Servicios.EstructuraEmpresarial
{
    public class ServicioOficina : IServicioOficina
    {
        private readonly IRepositorioOficina _repositorio;
        private readonly IMapper _mapper;

        public ServicioOficina(IRepositorioOficina Repositorio, IMapper mapper)
        {
            _repositorio = Repositorio;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ModeloOficina>> List()
        {
            var _oficinas = await _repositorio.GetAllWithAssociations();
            return _mapper.Map<IEnumerable<ModeloOficina>>(_oficinas); ;

        }
        public async Task<ModeloFuenteDatos<ModeloOficina>> List(ModeloPaginacion filtro)
        {
            IEnumerable<Oficina> _model = await _repositorio.GetAllWithAssociations();

            var _fuente = _mapper.Map<ModeloFuenteDatos<ModeloOficina>>(filtro);
            _fuente.TotalElementos = _model.Count();
            Filtro<Oficina>.ProcesarLista(ref _model, filtro);

            _fuente.Datos = _mapper.Map<IEnumerable<ModeloOficina>>(_model);
            return _fuente;
        }
        public async Task<ModeloOficina> Get(int Id)
        {
            var _oficina = await _repositorio.Get(Id);
            if (_oficina != null)
                return _mapper.Map<ModeloOficina>(_oficina);
            return null;
        }

        public async Task<ModeloOficina> Create(ModeloOficina oficina)
        {
            var _oficina = _mapper.Map<Oficina>(oficina);
            await _repositorio.Add(_oficina);
            return _mapper.Map<ModeloOficina>(_oficina); ;
        }
        public async Task<ModeloOficina> Update(ModeloOficina oficina)
        {
            var _oficina = await _repositorio.Get(oficina.Id);
            _mapper.Map(oficina, _oficina);
            await _repositorio.Update(_oficina);

            return oficina;
        }
        public async Task<ModeloOficina> Delete(int Id)
        {
            var _oficina = await _repositorio.Get(Id);
            if (_oficina != null)
            {
                await _repositorio.Remove(_oficina);

                return _mapper.Map<ModeloOficina>(_oficina);
            }
            return null;
        }

    }
}
