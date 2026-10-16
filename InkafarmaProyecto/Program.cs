using InkafarmaProyecto.Formularios;
using InkafarmaProyecto.Interfaces;
using InkafarmaProyecto.Repositorios;
using InkafarmaProyecto.Servicios;

namespace InkafarmaProyecto;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var clave = File.ReadAllText("clave.txt").Trim();
        var conexion = new ConexionBD($"Server=localhost;Database=inkafarma_proyecto;Uid=root;Pwd={clave};");

        var clientes = new ClienteRepositorio(conexion);
        var productos = new ProductoRepositorio(conexion);
        var boticas = new BoticaRepositorio(conexion);

        var ventaServicio = new VentaServicio(new VentaRepositorio(conexion));

        var reglas = new List<IReglaDevolucion> { new ReglaPlazo(7), new ReglaCantidad() };
        var devolucionServicio = new DevolucionServicio(new DevolucionRepositorio(conexion), reglas);

        var reportes = new List<IReporte>
        {
            new ReporteDevolucionesPorMotivo(conexion),
            new ReporteDevolucionesPorBotica(conexion),
            new ReporteDevolucionesPorMes(conexion),
            new ReporteProductosMasDevueltos(conexion)
        };

        var opciones = new List<(string, Func<Form>)>
        {
            ("1. Clientes", () => new FormClientes(clientes)),
            ("2. Productos y boticas", () => new FormProductosBoticas(productos, boticas)),
            ("3. Ventas", () => new FormVentas(ventaServicio, clientes, boticas, productos)),
            ("4. Devoluciones", () => new FormDevoluciones(devolucionServicio)),
            ("5. Reportes", () => new FormReportes(reportes))
        };

        Application.Run(new FormMenu(opciones));
    }
}