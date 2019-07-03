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
    public class EmpresaService : IEmpresaService
    {
        private readonly IRepositorioEmpresa _repositorio;
        private readonly IRepositorioOficina _oficinaRepositorio;
        private readonly IRepositorioPersona _corresponsalRepositorio;
        private readonly IMapper _mapper;

        public EmpresaService(IRepositorioEmpresa Repositorio, IRepositorioOficina oficinaRepositorio, IRepositorioPersona corresponsalRepositorio,
            IMapper mapper)
        {
            _repositorio = Repositorio;
            _oficinaRepositorio = oficinaRepositorio;
            _corresponsalRepositorio = corresponsalRepositorio;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ModeloEmpresa>> List()
        {
            var _model = await _repositorio.GetAllActive();
            return _mapper.Map<IEnumerable<ModeloEmpresa>>(_model);

        }
        public async Task<ModeloFuenteDatos<ModeloEmpresa>> List(ModeloPaginacion filtro)
        {
            IEnumerable<Empresa> _model = await _repositorio.GetAllActive();

            var _fuente = _mapper.Map<ModeloFuenteDatos<ModeloEmpresa>>(filtro);
            _fuente.TotalElementos = _model.Count();
            Filtro<Empresa>.ProcesarLista(ref _model, filtro);

            _fuente.Datos = _mapper.Map<IEnumerable<ModeloEmpresa>>(_model);
            return _fuente;
        }
        public async Task<ModeloEmpresa> Get(int Id)
        {
            var _model = await _repositorio.Get(Id);
            if (_model != null)
                return _mapper.Map<ModeloEmpresa>(_model);
            return null;
        }

        public async Task<ModeloEmpresa> Create(ModeloEmpresa model)
        {
            var _model = _mapper.Map<Empresa>(model);
            await _repositorio.Add(_model);
            return _mapper.Map<ModeloEmpresa>(_model);
        }
        public async Task<ModeloEmpresa> Update(ModeloEmpresa model)
        {
            var _model = await _repositorio.Get(model.Id);
            _mapper.Map(model, _model);
            await _repositorio.Update(_model);
            return model;
        }
        public async Task<ModeloEmpresa> Delete(int Id)
        {
            var _model = await _repositorio.Get(Id);
            if (_model != null)
            {
                await _repositorio.Remove(_model);
                var _oficinas = await _oficinaRepositorio.Where(o => o.Empresa == _model);
                if (_oficinas != null)
                {
                    foreach (var item in _oficinas)
                    {
                        await _oficinaRepositorio.Remove(item);
                        var corresponsal = await _corresponsalRepositorio.Where(o => o.Oficina == item);
                        foreach (var itemC in corresponsal)
                        {
                            await _corresponsalRepositorio.Remove(itemC);
                        }
                    }
                }
                return _mapper.Map<ModeloEmpresa>(_model);
            }
            return null;
        }

    }
}
