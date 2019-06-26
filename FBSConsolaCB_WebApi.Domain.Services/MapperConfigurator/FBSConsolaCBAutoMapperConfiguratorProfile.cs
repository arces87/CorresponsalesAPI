using FBS_Core.Base.Domain.Models.Filtro;
using FBS_Core.Identity.Domain.Services.MapperConfigurator;
using FBSConsolaCB_WebApi.DAL.Consola;
using FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.DAL.Nomenclador;
using FBSConsolaCB_WebApi.Domain.Models.Consola;
using FBSConsolaCB_WebApi.Domain.Models.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Domain.Models.Nomenclador;

namespace FBSConsolaCB_WebApi.Domain.Services.MapperConfigurator
{
    public class FBSConsolaCBAutoMapperConfiguratorProfile : AutoMapperConfiguratorProfile
    {
        public FBSConsolaCBAutoMapperConfiguratorProfile() : base()
        {
            CreateMap<Persona, PersonaModel>().ReverseMap();

            CreateMap<Corresponsal, CorresponsalModel>().ReverseMap();

            CreateMap<Supervisor, SupervisorModel>().ReverseMap();

            CreateMap<Empresa, EmpresaModel>().ReverseMap();

            CreateMap<Oficina, OficinaModel>().ReverseMap();

            CreateMap<Catalogo, CatalogoModel>().ReverseMap();

            CreateMap<TipoCatalogo, TipoCatalogoModel>().ReverseMap();

            CreateMap<Dispositivo, DispositivoModel>().ReverseMap();

            CreateMap<PaginacionModel, FuenteDatosModel<EmpresaModel>>();
            CreateMap<PaginacionModel, FuenteDatosModel<OficinaModel>>();
            CreateMap<PaginacionModel, FuenteDatosModel<PersonaModel>>();
            CreateMap<PaginacionModel, FuenteDatosModel<CatalogoModel>>();
            CreateMap<PaginacionModel, FuenteDatosModel<TipoCatalogoModel>>();
            CreateMap<PaginacionModel, FuenteDatosModel<DispositivoModel>>();
        }
    }
}
