using AutoMapper;
using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Dominio.Modelos.Seguridad;
using FBS.Identidad.Dominio.Servicios.Seguridad;
using FBS.Identidad.Infraestructura.Interfaces;
using FBSConsolaCB_WebApi.Dominio.Modelos.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Dominio.Servicios.Interfaces.Seguridad;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Dominio.Servicios.Consola
{
    public class ServicioUsuarioLocal : ServicioUsuario, IServicioUsuarioLocal
    {
        private readonly IRepositorioPersona _repositorio;
        private readonly IMapper _mapper;

        public ServicioUsuarioLocal(IRepositorioPersona Repositorio, IRepositorioUsuario repositorioUsuario,
            IRepositorioRol repositorioRol, IMapper mapper,
            SignInManager<Usuario> signInManager, UserManager<Usuario> userManager)
            : base(repositorioUsuario, repositorioRol, mapper, signInManager, userManager)
        {
            _repositorio = Repositorio;
            _mapper = mapper;
        }


        public async Task<ModeloPersona> Autenticar(ModeloUsuario model)
        {
            var user = (ModeloUsuario)(await Login(model));
            if (user.Token != null)
            {
                var corresponsal = _repositorio.GetForUserName(user.UserName);
                var _model = _mapper.Map<ModeloPersona>(corresponsal);
                _model.Usuario = user;
                return _model;
            }

            return null;
        }


    }
}
