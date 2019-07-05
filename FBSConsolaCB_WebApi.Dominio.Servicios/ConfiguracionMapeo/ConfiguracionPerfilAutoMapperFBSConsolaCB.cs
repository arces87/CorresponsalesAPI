using FBS.Dominio.Modelos.Filtro;
using FBS.Identidad.Dominio.Servicios.ConfiguracionMapeo;
using FBSConsolaCB_WebApi.DAL.Consola;
using FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.DAL.Nomenclador;
using FBSConsolaCB_WebApi.Dominio.Modelos.Consola;
using FBSConsolaCB_WebApi.Dominio.Modelos.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Dominio.Modelos.Nomenclador;

namespace FBSConsolaCB_WebApi.Dominio.Servicios.ConfiguracionMapeo
{
    public class ConfiguracionPerfilAutoMapperFBSConsolaCB : ConfiguracionAutoMapper
    {
        public ConfiguracionPerfilAutoMapperFBSConsolaCB() : base()
        {
            CreateMap<Persona, ModeloPersona>().ReverseMap();

            CreateMap<Corresponsal, ModeloCorresponsal>().ReverseMap();

            CreateMap<Supervisor, ModeloSupervisor>().ReverseMap();

            CreateMap<Empresa, ModeloEmpresa>().ReverseMap();

            CreateMap<Oficina, ModeloOficina>().ReverseMap();

            CreateMap<Catalogo, ModeloCatalogo>().ReverseMap();

            CreateMap<TipoCatalogo, ModeloTipoCatalogo>().ReverseMap();

            CreateMap<Dispositivo, ModeloDispositivo>().ReverseMap();

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
