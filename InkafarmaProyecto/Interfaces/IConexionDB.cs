using MySql.Data.MySqlClient;

namespace InkafarmaProyecto.Interfaces;

public interface IConexionBD
{
    MySqlConnection CrearConexion();
}