using FBS_Core.Identity.Domain.Services.MapperConfigurator;
using FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.DAL.Nomenclador;
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
        }
    }
}
