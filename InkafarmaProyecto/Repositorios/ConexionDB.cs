using InkafarmaProyecto.Interfaces;
using MySql.Data.MySqlClient;

namespace InkafarmaProyecto.Repositorios;

public class ConexionBD : IConexionBD
{
    private readonly string _cadena;

    public ConexionBD(string cadena)
    {
        _cadena = cadena;
    }

    public MySqlConnection CrearConexion()
    {
        return new MySqlConnection(_cadena);
    }
}