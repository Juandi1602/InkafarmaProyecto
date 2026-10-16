using InkafarmaProyecto.Interfaces;
using InkafarmaProyecto.Modelos;
using MySql.Data.MySqlClient;

namespace InkafarmaProyecto.Repositorios;

public class ClienteRepositorio : IClienteRepositorio
{
    private readonly IConexionBD _conexion;

    public ClienteRepositorio(IConexionBD conexion)
    {
        _conexion = conexion;
    }

    public List<Cliente> Listar()
    {
        var lista = new List<Cliente>();
        using var c = _conexion.CrearConexion();
        c.Open();
        using var cmd = new MySqlCommand("SELECT IdCliente, Dni, Nombres, Telefono, Correo FROM Cliente", c);
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            lista.Add(new Cliente
            {
                IdCliente = r.GetInt32(0),
                Dni = r.GetString(1),
                Nombres = r.GetString(2),
                Telefono = r.IsDBNull(3) ? "" : r.GetString(3),
                Correo = r.IsDBNull(4) ? "" : r.GetString(4)
            });
        }
        return lista;
    }

    public void Insertar(Cliente x)
    {
        using var c = _conexion.CrearConexion();
        c.Open();
        using var cmd = new MySqlCommand("INSERT INTO Cliente (Dni, Nombres, Telefono, Correo) VALUES (@d, @n, @t, @c)", c);
        cmd.Parameters.AddWithValue("@d", x.Dni);
        cmd.Parameters.AddWithValue("@n", x.Nombres);
        cmd.Parameters.AddWithValue("@t", x.Telefono);
        cmd.Parameters.AddWithValue("@c", x.Correo);
        cmd.ExecuteNonQuery();
    }

    public void Actualizar(Cliente x)
    {
        using var c = _conexion.CrearConexion();
        c.Open();
        using var cmd = new MySqlCommand("UPDATE Cliente SET Dni=@d, Nombres=@n, Telefono=@t, Correo=@c WHERE IdCliente=@id", c);
        cmd.Parameters.AddWithValue("@d", x.Dni);
        cmd.Parameters.AddWithValue("@n", x.Nombres);
        cmd.Parameters.AddWithValue("@t", x.Telefono);
        cmd.Parameters.AddWithValue("@c", x.Correo);
        cmd.Parameters.AddWithValue("@id", x.IdCliente);
        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        using var c = _conexion.CrearConexion();
        c.Open();
        using var cmd = new MySqlCommand("DELETE FROM Cliente WHERE IdCliente=@id", c);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }
}