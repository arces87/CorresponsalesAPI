using System.Collections.Generic;

namespace Corresponsales.AccesoFinancial.Api
{
    internal static class ClienteConstants
    {
        public const string Web = "Web";
        public const string Movils = "Movil";
    }

    public class AuthInfo
    { 
        public string BaseUrl { get; set; }
        public string LoginEndpoint { get; set; }
        public string RefreshEndpoint { get; set; }
        public string UsuarioAdmin { get; set; }
        public Dictionary<string, User> Users { get; set; } = new Dictionary<string, User>();
    }

    public class User
    {
        public TokenData Token { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
    }
}


