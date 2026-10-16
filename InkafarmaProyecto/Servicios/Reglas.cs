using InkafarmaProyecto.Interfaces;
using InkafarmaProyecto.Modelos;

namespace InkafarmaProyecto.Servicios;

public class ReglaPlazo : IReglaDevolucion
{
    private readonly int _dias;

    public ReglaPlazo(int dias)
    {
        _dias = dias;
    }

    public string? Validar(DetalleVentaVista detalle, int cantidad, DateTime fechaDevolucion)
    {
        int transcurridos = (fechaDevolucion.Date - detalle.FechaVenta.Date).Days;
        return transcurridos > _dias ? $"Fuera de plazo (máximo {_dias} días)." : null;
    }
}

public class ReglaCantidad : IReglaDevolucion
{
    public string? Validar(DetalleVentaVista detalle, int cantidad, DateTime fechaDevolucion)
    {
        return cantidad < 1 || cantidad > detalle.Cantidad
            ? $"Cantidad inválida (máximo {detalle.Cantidad})."
            : null;
    }
}