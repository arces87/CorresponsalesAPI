using System;
using System.Collections.Generic;
using System.Text;

namespace FBSConsolaCBWebApi.Infraestructura.Utiles
{
    public class ApiKeyGenerator : IApiKeyGenerator
    {
        public string _privateKey;
        public ApiKeyGenerator(string privateKey)
        {
            _privateKey = privateKey;
        }

        public string generateApiKey(string secretKey)
        {
            var imeiSubstringLength = secretKey.Length - 8;
            var imeiSubstring1 = secretKey.Substring(0, 8);
            var imeiSubstring2 = secretKey.Substring(8, imeiSubstringLength);
            var apiKey = $"{imeiSubstring1}-{imeiSubstring2}-{_privateKey}";

            return apiKey;
        }

        public Dictionary<string, List<string>> generateCustomHeaders(string value, Dictionary<string, List<string>> header = null)
        {
            if(header == null)
            {
                header = new Dictionary<string, List<string>>();
            }

            List<string> list;

            if (header.ContainsKey("Id"))
            {
                list = header["Id"];
            } else
            {
                list = new List<string>();
            }

            list.Clear();
            list.Add(value);

            header.Add("Id", list);

            return header;
        }
    }
}
