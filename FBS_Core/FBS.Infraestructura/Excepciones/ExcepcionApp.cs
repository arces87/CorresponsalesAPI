using System;

namespace FBS.Infraestructura.Excepciones
{
    public enum TipoError
    {
        Desconocido = 1,
        Advertir = 100,
        Error = 200,
        EmailNotification = 300,
        SMSNotification = 400,
    }

    [Serializable]
    public class ExcepcionApp : Exception
    {
        public TipoError TipoError { get; set; }

        public ExcepcionApp(TipoError nivelError=TipoError.Desconocido) : base() {
            TipoError = nivelError;
        }

        public ExcepcionApp(string message, TipoError nivelError = TipoError.Desconocido) : base(message) {
            TipoError = nivelError;
        }

        public ExcepcionApp(string message, Exception innerException, TipoError nivelError = TipoError.Desconocido) : base(message, innerException)
        {
            TipoError = nivelError;
        }
    }
}
