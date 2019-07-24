using FBS.Dominio.Modelos.Filtro;
using FBS.Identidad.Dominio.Servicios.ConfiguracionMapeo;
using FBSConsolaCB_WebApi.DAL.Consola;
using FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.DAL.Nomenclador;
using FBSConsolaCB_WebApi.Dominio.Modelos.Consola;
using FBSConsolaCB_WebApi.Dominio.Modelos.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Dominio.Modelos.Nomenclador;
using Financial_Services_Banca.Models;

namespace FBSConsolaCB_WebApi.Dominio.Servicios.ConfiguracionMapeo
{
    public class ConfiguracionPerfilAutoMapperFBSConsolaCB : ConfiguracionAutoMapper
    {
        public ConfiguracionPerfilAutoMapperFBSConsolaCB() : base()
        {
            CreateMap<Persona, ModeloPersona>().ReverseMap();
            CreateMap<InformacionPersonaMS, ModeloPersona>()
                .ForMember(m => m.PrimerNombre, opt => opt.MapFrom(d => d.Nombres.Split()[0]))
                .ForMember(m => m.SegundoNombre, opt => opt.MapFrom(d => d.Nombres.Split().Length > 1 ? d.Nombres.Split()[1] : ""))
                .ForMember(m => m.PrimerApellido, opt => opt.MapFrom(d => d.Apellidos.Split()[0]))
                .ForMember(m => m.SegundoApellido, opt => opt.MapFrom(d => d.Apellidos.Split().Length > 1 ? d.Nombres.Split()[1] : ""))
                .ForMember(m => m.FechaNacimiento, opt => opt.MapFrom(d => d.FechaNacimientoCreacion));

            CreateMap<Corresponsal, ModeloCorresponsal>()
                .ForMember(m => m.EstaBloqueado, opt => opt.MapFrom(c => c.Persona.Usuario.LockoutEnabled)).ReverseMap();

            CreateMap<Supervisor, ModeloSupervisor>().ReverseMap();

            CreateMap<Empresa, ModeloEmpresa>().ReverseMap();

            CreateMap<Oficina, ModeloOficina>().ReverseMap();

            CreateMap<Catalogo, ModeloCatalogo>().ReverseMap();

            CreateMap<TipoCatalogo, ModeloTipoCatalogo>().ReverseMap();

            CreateMap<Dispositivo, ModeloDispositivo>().ReverseMap();
            CreateMap<LimiteExistencia, ModeloLimiteExistencia>().ReverseMap();
            CreateMap<LimiteTransaccional, ModeloLimiteTransaccional>().ReverseMap();
            CreateMap<Log, ModeloLog>().ReverseMap();

            CreateMap<ModeloPaginacion, ModeloFuenteDatos<ModeloEmpresa>>();
            CreateMap<ModeloPaginacion, ModeloFuenteDatos<ModeloOficina>>();
            CreateMap<ModeloPaginacion, ModeloFuenteDatos<ModeloPersona>>();
            CreateMap<ModeloPaginacion, ModeloFuenteDatos<ModeloSupervisor>>();
            CreateMap<ModeloPaginacion, ModeloFuenteDatos<ModeloCorresponsal>>();
            CreateMap<ModeloPaginacion, ModeloFuenteDatos<ModeloCatalogo>>();
            CreateMap<ModeloPaginacion, ModeloFuenteDatos<ModeloTipoCatalogo>>();
            CreateMap<ModeloPaginacion, ModeloFuenteDatos<ModeloDispositivo>>();
            CreateMap<ModeloPaginacion, ModeloFuenteDatos<ModeloLog>>();
            CreateMap<ModeloPaginacion, ModeloFuenteDatos<ModeloLimiteExistencia>>();
            CreateMap<ModeloPaginacion, ModeloFuenteDatos<ModeloLimiteTransaccional>>();
        }
    }
}
