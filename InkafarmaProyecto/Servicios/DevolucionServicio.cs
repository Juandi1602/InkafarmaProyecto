using InkafarmaProyecto.Interfaces;
using InkafarmaProyecto.Modelos;

namespace InkafarmaProyecto.Servicios;

public class DevolucionServicio
{
    private readonly IDevolucionRepositorio _repo;
    private readonly IEnumerable<IReglaDevolucion> _reglas;

    public DevolucionServicio(IDevolucionRepositorio repo, IEnumerable<IReglaDevolucion> reglas)
    {
        _repo = repo;
        _reglas = reglas;
    }

    public List<DetalleVentaVista> ListarDetallesVenta() => _repo.ListarDetallesVenta();

    public List<DevolucionVista> Listar() => _repo.Listar();

    public string? Registrar(DetalleVentaVista detalle, int cantidad, string motivo, string observacion)
    {
        foreach (var regla in _reglas)
        {
            var error = regla.Validar(detalle, cantidad, DateTime.Today);
            if (error != null) return error;
        }

        _repo.Insertar(new Devolucion
        {
            IdDetalle = detalle.IdDetalle,
            Fecha = DateTime.Today,
            Cantidad = cantidad,
            Motivo = motivo,
            Observacion = observacion
        });
        return null;
    }

    public void Aprobar(int id, string resolucion, string destino) =>
        _repo.Resolver(id, "Aprobada", resolucion, destino);

    public void Rechazar(int id) =>
        _repo.Resolver(id, "Rechazada", null, null);
}