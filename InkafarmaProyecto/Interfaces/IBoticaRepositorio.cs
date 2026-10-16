using InkafarmaProyecto.Modelos;

namespace InkafarmaProyecto.Interfaces;

public interface IBoticaRepositorio
{
    List<Botica> Listar();
    void Insertar(Botica b);
    void Actualizar(Botica b);
    void Eliminar(int id);
}