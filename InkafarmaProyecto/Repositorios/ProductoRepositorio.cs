using InkafarmaProyecto.Interfaces;
using InkafarmaProyecto.Modelos;
using MySql.Data.MySqlClient;

namespace InkafarmaProyecto.Repositorios;

public class ProductoRepositorio : IProductoRepositorio
{
    private readonly IConexionBD _conexion;

    public ProductoRepositorio(IConexionBD conexion)
    {
        _conexion = conexion;
    }

    public List<Producto> Listar()
    {
        var lista = new List<Producto>();
        using var c = _conexion.CrearConexion();
        c.Open();
        using var cmd = new MySqlCommand("SELECT IdProducto, Codigo, Nombre, Precio, Stock FROM Producto", c);
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            lista.Add(new Producto
            {
                IdProducto = r.GetInt32(0),
                Codigo = r.GetString(1),
                Nombre = r.GetString(2),
                Precio = r.GetDecimal(3),
                Stock = r.GetInt32(4)
            });
        }
        return lista;
    }

    public void Insertar(Producto p)
    {
        using var c = _conexion.CrearConexion();
        c.Open();
        using var cmd = new MySqlCommand("INSERT INTO Producto (Codigo, Nombre, Precio, Stock) VALUES (@c, @n, @p, @s)", c);
        cmd.Parameters.AddWithValue("@c", p.Codigo);
        cmd.Parameters.AddWithValue("@n", p.Nombre);
        cmd.Parameters.AddWithValue("@p", p.Precio);
        cmd.Parameters.AddWithValue("@s", p.Stock);
        cmd.ExecuteNonQuery();
    }

    public void Actualizar(Producto p)
    {
        using var c = _conexion.CrearConexion();
        c.Open();
        using var cmd = new MySqlCommand("UPDATE Producto SET Codigo=@c, Nombre=@n, Precio=@p, Stock=@s WHERE IdProducto=@id", c);
        cmd.Parameters.AddWithValue("@c", p.Codigo);
        cmd.Parameters.AddWithValue("@n", p.Nombre);
        cmd.Parameters.AddWithValue("@p", p.Precio);
        cmd.Parameters.AddWithValue("@s", p.Stock);
        cmd.Parameters.AddWithValue("@id", p.IdProducto);
        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        using var c = _conexion.CrearConexion();
        c.Open();
        using var cmd = new MySqlCommand("DELETE FROM Producto WHERE IdProducto=@id", c);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }
}