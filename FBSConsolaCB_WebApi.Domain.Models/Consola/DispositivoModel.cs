using FBSConsolaCB_WebApi.Domain.Models.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Domain.Models.Nomenclador;


namespace FBSConsolaCB_WebApi.Domain.Models.Consola
{
    public class DispositivoModel
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Imei { get; set; }

        public string Mac { get; set; }

        public string NumeroSerie { get; set; }

        public CatalogoModel TipoDispositivo { get; set; }

        public CorresponsalModel Corresponsal { get; set; }

        public bool EstaActivo { get; set; }
    }
}
