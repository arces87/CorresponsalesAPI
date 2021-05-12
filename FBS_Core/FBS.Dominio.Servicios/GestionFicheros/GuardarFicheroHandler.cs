using FBS.Infraestructura.Excepciones;
using MediatR;
using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace FBS.Dominio.Servicios.GestionFicheros
{
    public class GuardarFicheroHandler : IRequestHandler<GuardarFicheroME, string>
    {

        public GuardarFicheroHandler()
        {
        }

        public async Task<string> Handle(GuardarFicheroME request, CancellationToken cancellationToken)
        {
            var _extension = "." + request.Fichero.FileName.Split('.')[request.Fichero.FileName.Split('.').Length - 1];
            var _fileName = Guid.NewGuid().ToString() + _extension;

            var pathToSave = Path.Combine(request.DireccionGuardar, _fileName);
            var mime = request.Fichero.ContentType;
            if (!mime.Equals("image/png") && !mime.Equals("image/jpg") && !mime.Equals("image/jpeg"))
            {
                throw new ExcepcionApp("El tipo de fichero es inválido.");
            }

            using (var bits = new FileStream(pathToSave, FileMode.Create))
            {

                await request.Fichero.CopyToAsync(bits);
            }

            AplicarPermisos(pathToSave);
                        
            return _fileName;
        }

        private static void AplicarPermisos(string pathToSave)
        {
            var security = new FileSecurity(pathToSave,
                          AccessControlSections.Owner |
                          AccessControlSections.Group |
                          AccessControlSections.Access);

            var authorizationRules = security.GetAccessRules(true, true, typeof(NTAccount));

            var owner = security.GetOwner(typeof(NTAccount));
            foreach (AuthorizationRule rule in authorizationRules)
            {
                FileSystemAccessRule fileRule = rule as FileSystemAccessRule;
                if (fileRule != null)
                {
                    if (owner != null && fileRule.IdentityReference == owner)
                    {
                        if (fileRule.FileSystemRights.HasFlag(FileSystemRights.ExecuteFile) ||
                           fileRule.FileSystemRights.HasFlag(FileSystemRights.ReadAndExecute) ||
                           fileRule.FileSystemRights.HasFlag(FileSystemRights.FullControl))
                        {
                            var flags = FileSystemRights.Read | FileSystemRights.Modify | FileSystemRights.Delete;

                            security.ModifyAccessRule(AccessControlModification.Add,
                                new FileSystemAccessRule(owner, flags, AccessControlType.Allow),
                                out bool modified);
                        }
                    }
                }
            }
        }
    }
}
