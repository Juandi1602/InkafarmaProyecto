using InkafarmaProyecto.Interfaces;
using InkafarmaProyecto.Modelos;
using MySql.Data.MySqlClient;

namespace InkafarmaProyecto.Repositorios;

public class VentaRepositorio : IVentaRepositorio
{
    private readonly IConexionBD _conexion;

    public VentaRepositorio(IConexionBD conexion)
    {
        _conexion = conexion;
    }

    public List<VentaVista> Listar()
    {
        var lista = new List<VentaVista>();
        using var c = _conexion.CrearConexion();
        c.Open();
        using var cmd = new MySqlCommand(@"
            SELECT v.IdVenta, v.NumeroBoleta, c.Nombres, b.Nombre, v.Fecha, v.Total
            FROM Venta v
            JOIN Cliente c ON c.IdCliente = v.IdCliente
            JOIN Botica b ON b.IdBotica = v.IdBotica
            ORDER BY v.IdVenta DESC", c);
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            lista.Add(new VentaVista
            {
                IdVenta = r.GetInt32(0),
                NumeroBoleta = r.GetString(1),
                Cliente = r.GetString(2),
                Botica = r.GetString(3),
                Fecha = r.GetDateTime(4),
                Total = r.GetDecimal(5)
            });
        }
        return lista;
    }

    public string Registrar(Venta venta, List<ItemVenta> items)
    {
        using var c = _conexion.CrearConexion();
        c.Open();
        using var tx = c.BeginTransaction();
        try
        {
            int siguiente;
            using (var cmdN = new MySqlCommand("SELECT COALESCE(MAX(IdVenta),0)+1 FROM Venta", c, tx))
                siguiente = Convert.ToInt32(cmdN.ExecuteScalar());
            string boleta = "B001-" + siguiente.ToString("D4");

            long idVenta;
            using (var cmd = new MySqlCommand(@"
                INSERT INTO Venta (NumeroBoleta, IdCliente, IdBotica, Fecha, Total)
                VALUES (@b, @c, @bo, @f, @t)", c, tx))
            {
                cmd.Parameters.AddWithValue("@b", boleta);
                cmd.Parameters.AddWithValue("@c", venta.IdCliente);
                cmd.Parameters.AddWithValue("@bo", venta.IdBotica);
                cmd.Parameters.AddWithValue("@f", venta.Fecha.Date);
                cmd.Parameters.AddWithValue("@t", venta.Total);
                cmd.ExecuteNonQuery();
                idVenta = cmd.LastInsertedId;
            }

            foreach (var it in items)
            {
                using (var cmdD = new MySqlCommand(@"
                    INSERT INTO DetalleVenta (IdVenta, IdProducto, Cantidad, PrecioUnitario)
                    VALUES (@v, @p, @c, @pu)", c, tx))
                {
                    cmdD.Parameters.AddWithValue("@v", idVenta);
                    cmdD.Parameters.AddWithValue("@p", it.IdProducto);
                    cmdD.Parameters.AddWithValue("@c", it.Cantidad);
                    cmdD.Parameters.AddWithValue("@pu", it.PrecioUnitario);
                    cmdD.ExecuteNonQuery();
                }

                using var cmdS = new MySqlCommand(
                    "UPDATE Producto SET Stock = Stock - @c WHERE IdProducto = @p AND Stock >= @c", c, tx);
                cmdS.Parameters.AddWithValue("@c", it.Cantidad);
                cmdS.Parameters.AddWithValue("@p", it.IdProducto);
                if (cmdS.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException($"Stock insuficiente de {it.Producto}.");
            }

            tx.Commit();
            return boleta;
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }
}