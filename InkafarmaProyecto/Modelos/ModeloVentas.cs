namespace InkafarmaProyecto.Modelos;

public class ItemVenta
{
    public int IdProducto { get; set; }
    public string Producto { get; set; } = "";
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal => Cantidad * PrecioUnitario;
}

public class VentaVista
{
    public int IdVenta { get; set; }
    public string NumeroBoleta { get; set; } = "";
    public string Cliente { get; set; } = "";
    public string Botica { get; set; } = "";
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
}