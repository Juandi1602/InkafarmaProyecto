using InkafarmaProyecto.Interfaces;
using InkafarmaProyecto.Modelos;

namespace InkafarmaProyecto.Servicios;

public class VentaServicio
{
    private readonly IVentaRepositorio _repo;

    public VentaServicio(IVentaRepositorio repo)
    {
        _repo = repo;
    }

    public List<VentaVista> Listar() => _repo.Listar();

    public string Registrar(int idCliente, int idBotica, List<ItemVenta> items)
    {
        if (items.Count == 0)
            throw new InvalidOperationException("Agrega al menos un producto.");

        var venta = new Venta
        {
            IdCliente = idCliente,
            IdBotica = idBotica,
            Fecha = DateTime.Today,
            Total = items.Sum(i => i.Subtotal)
        };
        return _repo.Registrar(venta, items);
    }
}