using InkafarmaProyecto.Modelos;

namespace InkafarmaProyecto.Interfaces;

public interface IReglaDevolucion
{
    string? Validar(DetalleVentaVista detalle, int cantidad, DateTime fechaDevolucion);
}