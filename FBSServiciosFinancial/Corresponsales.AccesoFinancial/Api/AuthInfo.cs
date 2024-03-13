using System.Collections.Generic;

namespace Corresponsales.AccesoFinancial.Api
{
    public class AuthInfo
    {
        public string BaseUrl { get; set; }
        public string LoginEndpoint { get; set; }
        public string RefreshEndpoint { get; set; }
        public bool UseDefaultUser { get; set; }

        public Dictionary<string, User> Users { get; set; }
    }

    public class User
    {
        public TokenData Token { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
    }
}


