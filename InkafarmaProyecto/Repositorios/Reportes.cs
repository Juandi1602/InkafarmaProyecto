using System.Data;
using InkafarmaProyecto.Interfaces;
using MySql.Data.MySqlClient;

namespace InkafarmaProyecto.Repositorios;

public abstract class ReporteSql : IReporte
{
    private readonly IConexionBD _conexion;

    protected ReporteSql(IConexionBD conexion)
    {
        _conexion = conexion;
    }

    public abstract string Nombre { get; }
    protected abstract string Sql { get; }

    public DataTable Ejecutar()
    {
        using var c = _conexion.CrearConexion();
        c.Open();
        using var da = new MySqlDataAdapter(Sql, c);
        var tabla = new DataTable();
        da.Fill(tabla);
        return tabla;
    }
}

public class ReporteDevolucionesPorMotivo : ReporteSql
{
    public ReporteDevolucionesPorMotivo(IConexionBD conexion) : base(conexion) { }

    public override string Nombre => "Devoluciones por motivo";

    protected override string Sql => @"
        SELECT Motivo, COUNT(*) AS Devoluciones, SUM(Cantidad) AS Unidades
        FROM Devolucion
        GROUP BY Motivo
        ORDER BY Devoluciones DESC";
}

public class ReporteDevolucionesPorBotica : ReporteSql
{
    public ReporteDevolucionesPorBotica(IConexionBD conexion) : base(conexion) { }

    public override string Nombre => "Devoluciones por botica";

    protected override string Sql => @"
        SELECT b.Nombre AS Botica, COUNT(*) AS Devoluciones, SUM(dv.Cantidad) AS Unidades
        FROM Devolucion dv
        JOIN DetalleVenta d ON d.IdDetalle = dv.IdDetalle
        JOIN Venta v ON v.IdVenta = d.IdVenta
        JOIN Botica b ON b.IdBotica = v.IdBotica
        GROUP BY b.Nombre
        ORDER BY Devoluciones DESC";
}

public class ReporteDevolucionesPorMes : ReporteSql
{
    public ReporteDevolucionesPorMes(IConexionBD conexion) : base(conexion) { }

    public override string Nombre => "Devoluciones por mes";

    protected override string Sql => @"
        SELECT DATE_FORMAT(Fecha, '%Y-%m') AS Mes, COUNT(*) AS Devoluciones, SUM(Cantidad) AS Unidades
        FROM Devolucion
        GROUP BY DATE_FORMAT(Fecha, '%Y-%m')
        ORDER BY Mes DESC";
}

public class ReporteProductosMasDevueltos : ReporteSql
{
    public ReporteProductosMasDevueltos(IConexionBD conexion) : base(conexion) { }

    public override string Nombre => "Productos más devueltos";

    protected override string Sql => @"
        SELECT p.Nombre AS Producto, COUNT(*) AS Devoluciones, SUM(dv.Cantidad) AS Unidades
        FROM Devolucion dv
        JOIN DetalleVenta d ON d.IdDetalle = dv.IdDetalle
        JOIN Producto p ON p.IdProducto = d.IdProducto
        GROUP BY p.Nombre
        ORDER BY Unidades DESC";
}