using InkafarmaProyecto.Modelos;

namespace InkafarmaProyecto.Interfaces;

public interface IVentaRepositorio
{
    List<VentaVista> Listar();
    string Registrar(Venta venta, List<ItemVenta> items);
}