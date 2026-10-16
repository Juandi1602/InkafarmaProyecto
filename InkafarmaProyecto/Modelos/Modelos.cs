namespace InkafarmaProyecto.Modelos;

public class Cliente
{
    public int IdCliente { get; set; }
    public string Dni { get; set; } = "";
    public string Nombres { get; set; } = "";
    public string Telefono { get; set; } = "";
    public string Correo { get; set; } = "";
}

public class Botica
{
    public int IdBotica { get; set; }
    public string Nombre { get; set; } = "";
    public string Distrito { get; set; } = "";
    public string Direccion { get; set; } = "";
}

public class Producto
{
    public int IdProducto { get; set; }
    public string Codigo { get; set; } = "";
    public string Nombre { get; set; } = "";
    public decimal Precio { get; set; }
    public int Stock { get; set; }
}

public class Venta
{
    public int IdVenta { get; set; }
    public string NumeroBoleta { get; set; } = "";
    public int IdCliente { get; set; }
    public int IdBotica { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
}

public class DetalleVenta
{
    public int IdDetalle { get; set; }
    public int IdVenta { get; set; }
    public int IdProducto { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}

public class Devolucion
{
    public int IdDevolucion { get; set; }
    public int IdDetalle { get; set; }
    public DateTime Fecha { get; set; }
    public int Cantidad { get; set; }
    public string Motivo { get; set; } = "";
    public string Estado { get; set; } = "Pendiente";
    public string? Resolucion { get; set; }
    public string? DestinoProducto { get; set; }
    public string Observacion { get; set; } = "";
}