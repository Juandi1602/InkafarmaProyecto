using InkafarmaProyecto.Interfaces;
using InkafarmaProyecto.Modelos;
using MySql.Data.MySqlClient;

namespace InkafarmaProyecto.Repositorios;

public class DevolucionRepositorio : IDevolucionRepositorio
{
    private readonly IConexionBD _conexion;

    public DevolucionRepositorio(IConexionBD conexion)
    {
        _conexion = conexion;
    }

    public List<DetalleVentaVista> ListarDetallesVenta()
    {
        var lista = new List<DetalleVentaVista>();
        using var c = _conexion.CrearConexion();
        c.Open();
        using var cmd = new MySqlCommand(@"
            SELECT d.IdDetalle, v.NumeroBoleta, c.Nombres, p.Nombre, d.Cantidad, v.Fecha
            FROM DetalleVenta d
            JOIN Venta v ON v.IdVenta = d.IdVenta
            JOIN Cliente c ON c.IdCliente = v.IdCliente
            JOIN Producto p ON p.IdProducto = d.IdProducto
            ORDER BY v.Fecha DESC", c);
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            lista.Add(new DetalleVentaVista
            {
                IdDetalle = r.GetInt32(0),
                NumeroBoleta = r.GetString(1),
                Cliente = r.GetString(2),
                Producto = r.GetString(3),
                Cantidad = r.GetInt32(4),
                FechaVenta = r.GetDateTime(5)
            });
        }
        return lista;
    }

    public List<DevolucionVista> Listar()
    {
        var lista = new List<DevolucionVista>();
        using var c = _conexion.CrearConexion();
        c.Open();
        using var cmd = new MySqlCommand(@"
            SELECT dv.IdDevolucion, v.NumeroBoleta, c.Nombres, p.Nombre,
                   dv.Cantidad, dv.Motivo, dv.Estado, dv.Fecha
            FROM Devolucion dv
            JOIN DetalleVenta d ON d.IdDetalle = dv.IdDetalle
            JOIN Venta v ON v.IdVenta = d.IdVenta
            JOIN Cliente c ON c.IdCliente = v.IdCliente
            JOIN Producto p ON p.IdProducto = d.IdProducto
            ORDER BY dv.IdDevolucion DESC", c);
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            lista.Add(new DevolucionVista
            {
                IdDevolucion = r.GetInt32(0),
                Boleta = r.GetString(1),
                Cliente = r.GetString(2),
                Producto = r.GetString(3),
                Cantidad = r.GetInt32(4),
                Motivo = r.GetString(5),
                Estado = r.GetString(6),
                Fecha = r.GetDateTime(7)
            });
        }
        return lista;
    }

    public void Insertar(Devolucion d)
    {
        using var c = _conexion.CrearConexion();
        c.Open();
        using var cmd = new MySqlCommand(@"
            INSERT INTO Devolucion (IdDetalle, Fecha, Cantidad, Motivo, Estado, Observacion)
            VALUES (@d, @f, @c, @m, 'Pendiente', @o)", c);
        cmd.Parameters.AddWithValue("@d", d.IdDetalle);
        cmd.Parameters.AddWithValue("@f", d.Fecha.Date);
        cmd.Parameters.AddWithValue("@c", d.Cantidad);
        cmd.Parameters.AddWithValue("@m", d.Motivo);
        cmd.Parameters.AddWithValue("@o", d.Observacion);
        cmd.ExecuteNonQuery();
    }

    public void Resolver(int idDevolucion, string estado, string? resolucion, string? destino)
    {
        using var c = _conexion.CrearConexion();
        c.Open();
        using var tx = c.BeginTransaction();
        try
        {
            using (var cmd = new MySqlCommand(@"
                UPDATE Devolucion SET Estado=@e, Resolucion=@r, DestinoProducto=@d
                WHERE IdDevolucion=@id AND Estado='Pendiente'", c, tx))
            {
                cmd.Parameters.AddWithValue("@e", estado);
                cmd.Parameters.AddWithValue("@r", (object?)resolucion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@d", (object?)destino ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@id", idDevolucion);
                if (cmd.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException("La devolución ya fue resuelta.");
            }

            if (estado == "Aprobada" && destino == "Reponer stock")
            {
                using var cmd2 = new MySqlCommand(@"
                    UPDATE Producto p
                    JOIN DetalleVenta d ON d.IdProducto = p.IdProducto
                    JOIN Devolucion dv ON dv.IdDetalle = d.IdDetalle
                    SET p.Stock = p.Stock + dv.Cantidad
                    WHERE dv.IdDevolucion = @id", c, tx);
                cmd2.Parameters.AddWithValue("@id", idDevolucion);
                cmd2.ExecuteNonQuery();
            }

            tx.Commit();
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }
}