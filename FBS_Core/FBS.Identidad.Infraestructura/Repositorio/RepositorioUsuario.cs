using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBS.Infraestructura.Repositorio;
using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Infraestructura.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;

namespace FBS.Identidad.Infraestructura.Repositorio
{
    public class RepositorioUsuario : Repositorio<Usuario>, IRepositorioUsuario
    {
        private readonly IConfiguration _configuracion;
        private readonly UserManager<Usuario> _manejadorUsuario;
        public RepositorioUsuario(ContextoFBSIdentidad context, IConfiguration configuracion, UserManager<Usuario> manejadorUsuario) : base(context)
        {
            _configuracion = configuracion;
            _manejadorUsuario = manejadorUsuario;
        }

        public async Task<IEnumerable<Usuario>> GetAllActive()
        {
            return await GetContext.Users.Where(m => m.EstaActivo == true).ToListAsync();
        }

        public override async Task<Usuario> Get(string Id)
        {
            return await GetContext.Users.Include(u => u.Operadora).FirstOrDefaultAsync(u => u.Id == Id);
        }
        public async Task<Usuario> GetPorUsuario(string Usuario)
        {
            return await GetContext.Users.FirstOrDefaultAsync(u => u.UserName == Usuario);
        }
        public async Task SalvarOtp(string Usuario, string identificacion, string otp)
        {
            await EliminarOtp(identificacion);
            var usuario = GetContext.Users.FirstOrDefault(u => u.UserName == Usuario);
            GetContext.UserTokens.Add(new IdentityUserToken<string>()
            {
                Name = identificacion,
                LoginProvider = "OTP_Net",
                UserId = usuario.Id,
                Value = otp
            });
            await GetContext.SaveChangesAsync();
        }
        public async Task SalvarTokenRecuperarContrasenia(string idUsuario, string token)
        {
            await EliminarTokenRecuperarContrasenia(idUsuario);
            GetContext.UserTokens.Add(new IdentityUserToken<string>()
            {
                Name = "TkRC",
                LoginProvider = "TkRC_Net",
                UserId = idUsuario,
                Value = token
            });
            await GetContext.SaveChangesAsync();
        }

        public async Task<string> ComprobarTokenRecuperarContrasenia(string tokenComprobar)
        {
            var token = await GetContext.UserTokens.FirstOrDefaultAsync(u => u.Value == tokenComprobar && u.Name == "TkRC");
            if (token != null)
                return token.UserId;
            return null;
        }

        public async Task EliminarTokenRecuperarContrasenia(string Usuario)
        {
            var otp = GetContext.UserTokens.FirstOrDefault(u => u.UserId == Usuario && u.Name == "TkRC");
            if (otp != null)
            {
                GetContext.UserTokens.Remove(otp);
            }
            await GetContext.SaveChangesAsync();
        }

        public async Task EliminarOtp(string identificacion)
        {
            var otps = GetContext.UserTokens.Where(u => u.Name == identificacion && u.LoginProvider == "OTP_Net").ToList();
            if (otps.Count > 0)
            {
                GetContext.UserTokens.RemoveRange(otps);
            }
            await GetContext.SaveChangesAsync();
        }
        public async Task EliminarOtpCliente(string Usuario)
        {
            var usuario = GetContext.Users.FirstOrDefault(u => u.UserName == Usuario);
            var otp = GetContext.UserTokens.FirstOrDefault(u => u.UserId == usuario.Id && u.Name == "OTP_Cliente");
            if (otp != null)
            {
                GetContext.UserTokens.Remove(otp);
            }
            await GetContext.SaveChangesAsync();
        }
        public async Task<bool> ComprobarOtp(string Usuario, string identificacion, string otpComprobar)
        {
            var usuario = await GetContext.Users.FirstOrDefaultAsync(u => u.UserName == Usuario);
            var otps = GetContext.UserTokens.Where(u => u.UserId == usuario.Id && u.Name == identificacion);
            foreach (var item in otps)
            {
                if (item.Value == otpComprobar)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<string> ReferenciaOtp(string Usuario, string identificacion)
        {
            var usuario = await GetContext.Users.FirstOrDefaultAsync(u => u.UserName == Usuario);
            var otp = GetContext.UserTokens.Where(u => u.UserId == usuario.Id && u.Name == identificacion).FirstOrDefault();

            return otp?.Value;
            
        }

        public override async Task Update(Usuario entidad)
        {
            var usuario = GetContext.Users.FirstOrDefault(u => u.Id == entidad.Id);
            usuario.PhoneNumber = entidad.PhoneNumber;
            usuario.Operadora = GetContext.Catalogos.FirstOrDefault(c => c.Id == entidad.Operadora.Id);
            GetContext.Entry(usuario).State = EntityState.Modified;
            await GetContext.SaveChangesAsync();
        }
        public override async Task Remove(Usuario entidad)
        {
            var usuario = GetContext.Users.FirstOrDefault(u => u.Id == entidad.Id);
            usuario.EstaActivo = !usuario.EstaActivo;

            GetContext.Entry(usuario).State = EntityState.Modified;
            await GetContext.SaveChangesAsync();

            if (usuario.EstaActivo)
            {
                var usuarioTmp = await _manejadorUsuario.FindByIdAsync(usuario.Id);
                usuarioTmp.LockoutEnd = DateTime.Now.AddDays(-7);
                usuarioTmp.AccessFailedCount = 0;
                await _manejadorUsuario.UpdateAsync(usuarioTmp);
            }
        }

        public async Task AsignarCanal(CanalUsuario entidad)
        {
            entidad.Usuario = GetContext.Users.FirstOrDefault(u => u.Id == entidad.Usuario.Id);
            entidad.Canal = GetContext.Canales.FirstOrDefault(u => u.Id == entidad.Canal.Id);
            GetContext.CanalesUsuarios.Add(entidad);
            await GetContext.SaveChangesAsync();
        }

        public async Task<bool> VerificarCorreoElectronico(string email)
        {
            var usuario = await GetContext.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpper());
            return usuario != null ? true : false;
        }
        public async Task<bool> VerificarCorreoElectronico(string email, string IdUsuario)
        {
            var usuario = await GetContext.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpper()
                                                                    && u.Id != IdUsuario);
            return usuario != null ? true : false;
        }

        public async Task<bool> VerificarUserName(string username)
        {
            var usuario = await GetContext.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == username.ToUpper());
            return usuario != null ? true : false;
        }
        public async Task<bool> VerificarUserName(string username, string IdUsuario)
        {
            var usuario = await GetContext.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == username.ToUpper()
                                                                    && u.Id != IdUsuario);
            return usuario != null ? true : false;
        }

        public ContextoFBSIdentidad GetContext
        {
            get { return _contexto as ContextoFBSIdentidad; }
        }

    }
}
