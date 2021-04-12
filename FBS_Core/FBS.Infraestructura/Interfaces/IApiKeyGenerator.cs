using System;
using System.Collections.Generic;
using System.Text;

namespace FBS.Infraestructura.Interfaces
{
    public interface IApiKeyGenerator
    {
        string generateApiKey(string secretKey);
        Dictionary<string, List<string>> generateCustomHeaders(string value, Dictionary<string, List<string>> header = null);
    }
}
