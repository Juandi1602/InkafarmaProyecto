using System.Data;

namespace InkafarmaProyecto.Interfaces;

public interface IReporte
{
    string Nombre { get; }
    DataTable Ejecutar();
}