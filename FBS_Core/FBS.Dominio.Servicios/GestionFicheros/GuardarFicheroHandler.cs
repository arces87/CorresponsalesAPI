using FBS.Infraestructura.Excepciones;
using MediatR;
using System;
using System.IO;
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
            var _fileName = "";
            try
            {
                var _extension = "." + request.Fichero.FileName.Split('.')[request.Fichero.FileName.Split('.').Length - 1];
                _fileName = Guid.NewGuid().ToString() + _extension;

                var pathToSave = Path.Combine(request.DireccionGuardar, _fileName);

                using (var bits = new FileStream(pathToSave, FileMode.Create))
                {
                    await request.Fichero.CopyToAsync(bits);
                }
            }
            catch (Exception e)
            {
                throw new ExcepcionApp(e.Message);
            }

            return _fileName;
        }
    }
}
