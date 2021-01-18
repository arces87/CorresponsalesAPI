using AutoMapper;
using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Dominio.Servicios.Utilidad;
using FBSConsolaCBWebApi.DAL.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands
{
    public class CrearLogHandler : IRequestHandler<CrearLogME, string>
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IRepositorioLog _repositorio;
        private readonly IMapper _mapper;
        private readonly byte[] _llave;
        private readonly UserManager<Usuario> _manejadorUsuario;

        public CrearLogHandler(IRepositorioLog repositorio, IMapper mapper, IHttpContextAccessor httpContext, UserManager<Usuario> manejadorUsuario)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _httpContext = httpContext;
            _llave = Encoding.UTF8.GetBytes("!A%D*G-KaPdSgVkY");
            _manejadorUsuario = manejadorUsuario;
        }

        public async Task<string> Handle(CrearLogME request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Log>(request);
            _model.UsuarioId = null;

            // Esto no es reponsabilidad del Logger, se le debería pasar como aprametro el Id del usuario.
            if (_httpContext.HttpContext.User.Identity.Name != null)
            {
                var usuario = _manejadorUsuario.Users.FirstOrDefault(u => u.Id == _httpContext.HttpContext.User.Identity.Name);

                if (usuario != null)
                {
                    _model.UsuarioId = usuario.Id;
                }
            }

            var logDispositivo = new LogDispositivo()
            {
                Agente = _httpContext.HttpContext.Request.Headers["User-Agent"],
                IpRemoto = _httpContext.HttpContext.Connection.RemoteIpAddress.ToString()
            };
            _model.Fecha = DateTime.Now;
            _model.Hora = _model.Fecha.TimeOfDay;
            _model.JsonDispositivo = JsonConvert.SerializeObject(logDispositivo);
            var _datosEncriptar = JsonConvert.SerializeObject(_model);
            var encriptado = Criptografia.EncryptStringToBytes_Aes(_datosEncriptar, _llave, _llave);
            _model.Criptografia = Encoding.UTF8.GetString(encriptado);
            var identificador = await _repositorio.Add(_model);
            return identificador;
        }
    }
}
