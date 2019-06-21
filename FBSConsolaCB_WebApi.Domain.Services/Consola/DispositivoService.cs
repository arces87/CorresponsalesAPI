using AutoMapper;
using FBS_Core.Base.Domain.Models.Filtro;
using FBS_Core.Base.Domain.Services.Utilidades;
using FBSConsolaCB_WebApi.DAL.Consola;
using FBSConsolaCB_WebApi.Domain.Models.Consola;
using FBSConsolaCB_WebApi.Domain.Services.Interfaces.Consola;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.Consola;
using System.Collections.Generic;
using System.Linq;

namespace FBSConsolaCB_WebApi.Domain.Services.Consola
{
    public class DispositivoService : IDispositivoService
    {
        private readonly IDispositivoRepository _repository;
        private readonly IMapper _mapper;

        public DispositivoService(IDispositivoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public IEnumerable<DispositivoModel> List(int Tipo)
        {
            var _model = _repository.GetAllWithAssociations();
            if (Tipo != 0)
                _model = _model.Where(c => c.TipoDispositivo.Id == Tipo).ToList();

            return _mapper.Map<IEnumerable<DispositivoModel>>(_model); ;

        }

        public FuenteDatosModel<DispositivoModel> List(PaginacionModel filtro)
        {
            IEnumerable<Dispositivo> _model = _repository.GetAllWithAssociations();

            var _fuente = _mapper.Map<FuenteDatosModel<DispositivoModel>>(filtro);
            _fuente.TotalElementos = _model.Count();
            Filtro<Dispositivo>.ProcesarLista(ref _model, filtro);

            _fuente.Datos = _mapper.Map<IEnumerable<DispositivoModel>>(_model);
            return _fuente;
        }

        public DispositivoModel Get(int Id)
        {
            var _model = _repository.GetWithAssociations(Id);
            if (_model != null)
                return _mapper.Map<DispositivoModel>(_model);
            return null;
        }

        public DispositivoModel Create(DispositivoModel model)
        {
            var _model = _mapper.Map<Dispositivo>(model);
            _repository.Add(_model);
            return _mapper.Map<DispositivoModel>(_model);
        }
        public DispositivoModel Update(DispositivoModel model)
        {
            var _model = _repository.GetWithAssociations(model.Id);
            _mapper.Map(model, _model);
            _repository.Update(_model);

            return model;
        }
        public DispositivoModel Delete(int Id)
        {
            var _model = _repository.Get(Id);
            if (_model != null)
            {
                _repository.Remove(_model);
                return _mapper.Map<DispositivoModel>(_model);
            }
            return null;
        }
    }
}
