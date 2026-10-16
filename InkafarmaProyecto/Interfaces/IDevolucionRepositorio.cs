using InkafarmaProyecto.Modelos;

namespace InkafarmaProyecto.Interfaces;

public interface IDevolucionRepositorio
{
    List<DetalleVentaVista> ListarDetallesVenta();
    List<DevolucionVista> Listar();
    void Insertar(Devolucion d);
    void Resolver(int idDevolucion, string estado, string? resolucion, string? destino);
}