using FBS.DAL.Nomenclador;
using FBS.Identidad.DAL.Seguridad;
using FBSConsolaCBWebApi.DAL.Canales;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCBWebApi.DAL.Corresponsales
{
    [Table("Agente", Schema = "Corresponsales")]
    public class Agente
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Catalogo Estado { get; set; }

        public Usuario Usuario { get; set; }

        public Usuario Supervisor { get; set; }

        //public Dispositivo Dispositivo { get; set; }

        public string NombreAgente { get; set; }

        public string JsonAgente { get; set; }
        public string Identificacion { get; set; }
        public int TipoIdentificacion { get; set; }
        public string Ubicacion { get; set; }

        public bool EstaActivo { get; set; }
        [Timestamp]
        public byte[] Concurrencia { get; set; }

        //public bool ValdiarDispotivo(string imei, string mac)
        //{
        //    return Dispositivo != null && Dispositivo.EstaActivo && Dispositivo.Imei.ToUpper() == imei.ToUpper() && Dispositivo.MacAddress.ToUpper() == mac.ToUpper();
        //}

        //public bool ValdiarGeolocalizacion(Geolocalizacion geolocalizacion, double latitud, double longitud)
        //{

        //    if (geolocalizacion == null) return false;

        //    var latitud_inicio = geolocalizacion.Latitud - 1;
        //    var latitud_fin = geolocalizacion.Latitud + 1;
        //    var longitud_inicio = geolocalizacion.Longitud - 1;
        //    var longitud_fin = geolocalizacion.Longitud + 1;

        //    return  latitud >= latitud_inicio && latitud <= latitud_fin && longitud >= longitud_inicio && longitud <= longitud_fin;
        //}
    }
}
