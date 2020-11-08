using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.WebApi.ManejadorExcepciones
{
    public class ExcepcionApp : Exception
    {
        public ExcepcionApp() : base() {}

        public ExcepcionApp(string message) : base(message) {}       
    }
}
