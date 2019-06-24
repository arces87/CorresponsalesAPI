using FBS_Core.Base.Domain.Models.Filtro;
using FBS_Core.Identity.Domain.Services.MapperConfigurator;
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
            CreateMap<Corresponsal, CorresponsalModel>().ReverseMap();

            CreateMap<Empresa, EmpresaModel>().ReverseMap();

            CreateMap<AreaTrabajo, AreaTrabajoModel>().ReverseMap();

            CreateMap<Oficina, OficinaModel>().ReverseMap();

            CreateMap<Cargo, CargoModel>().ReverseMap();

            CreateMap<Catalogo, CatalogoModel>().ReverseMap();

            CreateMap<TipoCatalogo, TipoCatalogoModel>().ReverseMap();

            CreateMap<PaginacionModel, FuenteDatosModel<EmpresaModel>>();
            CreateMap<PaginacionModel, FuenteDatosModel<AreaTrabajoModel>>();
            CreateMap<PaginacionModel, FuenteDatosModel<OficinaModel>>();
            CreateMap<PaginacionModel, FuenteDatosModel<CargoModel>>();
            CreateMap<PaginacionModel, FuenteDatosModel<CorresponsalModel>>();
            CreateMap<PaginacionModel, FuenteDatosModel<CatalogoModel>>();
            CreateMap<PaginacionModel, FuenteDatosModel<TipoCatalogoModel>>();
            CreateMap<PaginacionModel, FuenteDatosModel<DispositivoModel>>();
        }
    }
}
