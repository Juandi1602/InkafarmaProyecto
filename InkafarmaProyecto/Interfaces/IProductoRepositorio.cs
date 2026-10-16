using InkafarmaProyecto.Modelos;

namespace InkafarmaProyecto.Interfaces;

public interface IProductoRepositorio
{
    List<Producto> Listar();
    void Insertar(Producto p);
    void Actualizar(Producto p);
    void Eliminar(int id);
}