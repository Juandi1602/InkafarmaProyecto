using InkafarmaProyecto.Interfaces;
using InkafarmaProyecto.Modelos;
using MySql.Data.MySqlClient;

namespace InkafarmaProyecto.Repositorios;

public class BoticaRepositorio : IBoticaRepositorio
{
    private readonly IConexionBD _conexion;

    public BoticaRepositorio(IConexionBD conexion)
    {
        _conexion = conexion;
    }

    public List<Botica> Listar()
    {
        var lista = new List<Botica>();
        using var c = _conexion.CrearConexion();
        c.Open();
        using var cmd = new MySqlCommand("SELECT IdBotica, Nombre, Distrito, Direccion FROM Botica", c);
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            lista.Add(new Botica
            {
                IdBotica = r.GetInt32(0),
                Nombre = r.GetString(1),
                Distrito = r.GetString(2),
                Direccion = r.IsDBNull(3) ? "" : r.GetString(3)
            });
        }
        return lista;
    }

    public void Insertar(Botica b)
    {
        using var c = _conexion.CrearConexion();
        c.Open();
        using var cmd = new MySqlCommand("INSERT INTO Botica (Nombre, Distrito, Direccion) VALUES (@n, @d, @di)", c);
        cmd.Parameters.AddWithValue("@n", b.Nombre);
        cmd.Parameters.AddWithValue("@d", b.Distrito);
        cmd.Parameters.AddWithValue("@di", b.Direccion);
        cmd.ExecuteNonQuery();
    }

    public void Actualizar(Botica b)
    {
        using var c = _conexion.CrearConexion();
        c.Open();
        using var cmd = new MySqlCommand("UPDATE Botica SET Nombre=@n, Distrito=@d, Direccion=@di WHERE IdBotica=@id", c);
        cmd.Parameters.AddWithValue("@n", b.Nombre);
        cmd.Parameters.AddWithValue("@d", b.Distrito);
        cmd.Parameters.AddWithValue("@di", b.Direccion);
        cmd.Parameters.AddWithValue("@id", b.IdBotica);
        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        using var c = _conexion.CrearConexion();
        c.Open();
        using var cmd = new MySqlCommand("DELETE FROM Botica WHERE IdBotica=@id", c);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }
}