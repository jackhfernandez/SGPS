namespace Modelo;

public class Rol
{
    public int RolId { get; set; }
    public string NombreRol { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool EsActivo { get; set; }
    public DateTime FechaCreacion { get; set; }
}
