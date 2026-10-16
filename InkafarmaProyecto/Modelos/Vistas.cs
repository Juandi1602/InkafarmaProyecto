namespace InkafarmaProyecto.Modelos;

public class DetalleVentaVista
{
    public int IdDetalle { get; set; }
    public string NumeroBoleta { get; set; } = "";
    public string Cliente { get; set; } = "";
    public string Producto { get; set; } = "";
    public int Cantidad { get; set; }
    public DateTime FechaVenta { get; set; }
    public string Texto => $"{NumeroBoleta} | {Cliente} | {Producto} (x{Cantidad})";
}

public class DevolucionVista
{
    public int IdDevolucion { get; set; }
    public string Boleta { get; set; } = "";
    public string Cliente { get; set; } = "";
    public string Producto { get; set; } = "";
    public int Cantidad { get; set; }
    public string Motivo { get; set; } = "";
    public string Estado { get; set; } = "";
    public DateTime Fecha { get; set; }
}