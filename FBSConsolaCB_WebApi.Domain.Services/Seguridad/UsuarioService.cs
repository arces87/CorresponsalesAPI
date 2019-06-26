using AutoMapper;
using FBS_Core.Base.Domain.Models.Filtro;
using FBS_Core.Base.Domain.Services.Utilidades;
using FBS_Core.Identity.DAL.Seguridad;
using FBS_Core.Identity.Domain.Models.Seguridad;
using FBS_Core.Identity.Domain.Services.Seguridad;
using FBS_Core.Identity.Infraestructure.Interfaces;
using FBSConsolaCB_WebApi.Domain.Models.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Domain.Services.Interfaces.Seguridad;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Domain.Services.Consola
{
    public class UsuarioService : UserService, IUsuarioService
    {
        private readonly IPersonaRepository _repository;
        private readonly IMapper _mapper;

        public UsuarioService(IPersonaRepository repository, IUserRepository userRepository,
            IRoleRepository roleRepository, IMapper mapper,
            SignInManager<User> signInManager, UserManager<User> userManager)
            : base(userRepository, roleRepository, mapper, signInManager, userManager)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<PersonaModel> Autenticar(UserModel model)
        {
            var user = (UserModel)(await Login(model));
            if (user.Token != null)
            {
                var corresponsal = _repository.GetForUserName(user.UserName);
                var _model = _mapper.Map<PersonaModel>(corresponsal);
                _model.Usuario = user;
                return _model;
            }

            return null;
        }


    }
}
