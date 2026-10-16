using InkafarmaProyecto.Modelos;

namespace InkafarmaProyecto.Interfaces;

public interface IClienteRepositorio
{
    List<Cliente> Listar();
    void Insertar(Cliente c);
    void Actualizar(Cliente c);
    void Eliminar(int id);
}