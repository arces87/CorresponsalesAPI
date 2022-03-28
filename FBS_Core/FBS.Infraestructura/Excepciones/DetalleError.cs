using Newtonsoft.Json;

namespace FBS.Infraestructura.Excepciones
{
    public class DetalleError
    {
		public int CodigoEstado { get; set; }
		public string SeguimientoPila { get; set; }
		public string MensajeExcepcion { get; set; }
		public string Mensaje { get; set; }

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this); //JsonConvert is part of Newtonsoft.Json package.
		}
	}
}
