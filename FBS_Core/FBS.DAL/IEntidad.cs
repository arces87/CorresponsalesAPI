namespace FBS.DAL
{
    public interface IEntidad
    {
        bool EstaActivo { get; set; }
        byte[] Concurrencia { get; set; }
    }
}
