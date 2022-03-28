using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.WebApi.ManejadorExcepciones
{
    public class ExcepcionFinancial
    {
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }
        public string StackTrace { get; set; }
        public ExcepcionInternaFinancial InnerException { get; set; }
    }

    public class ExcepcionInternaFinancial
    {
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }
        public string StackTrace { get; set; }
    }
}
