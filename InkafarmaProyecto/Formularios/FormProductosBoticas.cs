using InkafarmaProyecto.Interfaces;
using InkafarmaProyecto.Modelos;

namespace InkafarmaProyecto.Formularios;

public partial class FormProductosBoticas : Form
{
    private readonly IProductoRepositorio _productos;
    private readonly IBoticaRepositorio _boticas;
    private int _idProducto = 0;
    private int _idBotica = 0;

    private readonly TabControl tabs = new() { Dock = DockStyle.Fill };
    private readonly TabPage tabProd = new("Productos");
    private readonly TabPage tabBot = new("Boticas");

    // Productos
    private readonly TextBox txtCodigo = new() { Left = 100, Top = 15, Width = 220 };
    private readonly TextBox txtNombreP = new() { Left = 100, Top = 45, Width = 220 };
    private readonly TextBox txtPrecio = new() { Left = 100, Top = 75, Width = 220 };
    private readonly NumericUpDown nudStock = new() { Left = 100, Top = 105, Width = 80, Maximum = 100000 };
    private readonly Button btnGuardarP = new() { Text = "Guardar", Left = 340, Top = 14, Width = 90 };
    private readonly Button btnEliminarP = new() { Text = "Eliminar", Left = 340, Top = 44, Width = 90 };
    private readonly Button btnLimpiarP = new() { Text = "Limpiar", Left = 340, Top = 74, Width = 90 };
    private readonly DataGridView dgvP = new()
    {
        Left = 15,
        Top = 145,
        Width = 640,
        Height = 230,
        ReadOnly = true,
        AllowUserToAddRows = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    };

    // Boticas
    private readonly TextBox txtNombreB = new() { Left = 100, Top = 15, Width = 220 };
    private readonly TextBox txtDistrito = new() { Left = 100, Top = 45, Width = 220 };
    private readonly TextBox txtDireccion = new() { Left = 100, Top = 75, Width = 220 };
    private readonly Button btnGuardarB = new() { Text = "Guardar", Left = 340, Top = 14, Width = 90 };
    private readonly Button btnEliminarB = new() { Text = "Eliminar", Left = 340, Top = 44, Width = 90 };
    private readonly Button btnLimpiarB = new() { Text = "Limpiar", Left = 340, Top = 74, Width = 90 };
    private readonly DataGridView dgvB = new()
    {
        Left = 15,
        Top = 145,
        Width = 640,
        Height = 230,
        ReadOnly = true,
        AllowUserToAddRows = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    };

    public FormProductosBoticas(IProductoRepositorio productos, IBoticaRepositorio boticas)
    {
        InitializeComponent();
        _productos = productos;
        _boticas = boticas;

        Text = "Productos y boticas";
        ClientSize = new Size(690, 420);
        StartPosition = FormStartPosition.CenterScreen;

        tabProd.Controls.Add(new Label { Text = "Código", Left = 15, Top = 18, Width = 80 });
        tabProd.Controls.Add(new Label { Text = "Nombre", Left = 15, Top = 48, Width = 80 });
        tabProd.Controls.Add(new Label { Text = "Precio", Left = 15, Top = 78, Width = 80 });
        tabProd.Controls.Add(new Label { Text = "Stock", Left = 15, Top = 108, Width = 80 });
        tabProd.Controls.AddRange(new Control[] { txtCodigo, txtNombreP, txtPrecio, nudStock, btnGuardarP, btnEliminarP, btnLimpiarP, dgvP });

        tabBot.Controls.Add(new Label { Text = "Nombre", Left = 15, Top = 18, Width = 80 });
        tabBot.Controls.Add(new Label { Text = "Distrito", Left = 15, Top = 48, Width = 80 });
        tabBot.Controls.Add(new Label { Text = "Dirección", Left = 15, Top = 78, Width = 80 });
        tabBot.Controls.AddRange(new Control[] { txtNombreB, txtDistrito, txtDireccion, btnGuardarB, btnEliminarB, btnLimpiarB, dgvB });

        tabs.TabPages.Add(tabProd);
        tabs.TabPages.Add(tabBot);
        Controls.Add(tabs);

        btnGuardarP.Click += (s, e) => GuardarProducto();
        btnEliminarP.Click += (s, e) => EliminarProducto();
        btnLimpiarP.Click += (s, e) => LimpiarProducto();
        dgvP.CellClick += (s, e) => SeleccionarProducto();

        btnGuardarB.Click += (s, e) => GuardarBotica();
        btnEliminarB.Click += (s, e) => EliminarBotica();
        btnLimpiarB.Click += (s, e) => LimpiarBotica();
        dgvB.CellClick += (s, e) => SeleccionarBotica();

        CargarProductos();
        CargarBoticas();
    }

    // ---------- Productos ----------
    private void CargarProductos()
    {
        dgvP.DataSource = _productos.Listar();
    }

    private void GuardarProducto()
    {
        if (string.IsNullOrWhiteSpace(txtCodigo.Text) || string.IsNullOrWhiteSpace(txtNombreP.Text)
            || !decimal.TryParse(txtPrecio.Text, out var precio) || precio <= 0)
        {
            MessageBox.Show("Código, nombre y un precio válido son obligatorios.");
            return;
        }

        var p = new Producto
        {
            IdProducto = _idProducto,
            Codigo = txtCodigo.Text.Trim(),
            Nombre = txtNombreP.Text.Trim(),
            Precio = precio,
            Stock = (int)nudStock.Value
        };

        try
        {
            if (_idProducto == 0) _productos.Insertar(p);
            else _productos.Actualizar(p);
            LimpiarProducto();
            CargarProductos();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message);
        }
    }

    private void EliminarProducto()
    {
        if (_idProducto == 0) return;
        try
        {
            _productos.Eliminar(_idProducto);
            LimpiarProducto();
            CargarProductos();
        }
        catch (Exception ex)
        {
            MessageBox.Show("No se puede eliminar (tiene ventas asociadas): " + ex.Message);
        }
    }

    private void SeleccionarProducto()
    {
        if (dgvP.CurrentRow?.DataBoundItem is Producto p)
        {
            _idProducto = p.IdProducto;
            txtCodigo.Text = p.Codigo;
            txtNombreP.Text = p.Nombre;
            txtPrecio.Text = p.Precio.ToString();
            nudStock.Value = p.Stock;
        }
    }

    private void LimpiarProducto()
    {
        _idProducto = 0;
        txtCodigo.Clear();
        txtNombreP.Clear();
        txtPrecio.Clear();
        nudStock.Value = 0;
    }

    // ---------- Boticas ----------
    private void CargarBoticas()
    {
        dgvB.DataSource = _boticas.Listar();
    }

    private void GuardarBotica()
    {
        if (string.IsNullOrWhiteSpace(txtNombreB.Text) || string.IsNullOrWhiteSpace(txtDistrito.Text))
        {
            MessageBox.Show("Nombre y distrito son obligatorios.");
            return;
        }

        var b = new Botica
        {
            IdBotica = _idBotica,
            Nombre = txtNombreB.Text.Trim(),
            Distrito = txtDistrito.Text.Trim(),
            Direccion = txtDireccion.Text.Trim()
        };

        try
        {
            if (_idBotica == 0) _boticas.Insertar(b);
            else _boticas.Actualizar(b);
            LimpiarBotica();
            CargarBoticas();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message);
        }
    }

    private void EliminarBotica()
    {
        if (_idBotica == 0) return;
        try
        {
            _boticas.Eliminar(_idBotica);
            LimpiarBotica();
            CargarBoticas();
        }
        catch (Exception ex)
        {
            MessageBox.Show("No se puede eliminar (tiene ventas asociadas): " + ex.Message);
        }
    }

    private void SeleccionarBotica()
    {
        if (dgvB.CurrentRow?.DataBoundItem is Botica b)
        {
            _idBotica = b.IdBotica;
            txtNombreB.Text = b.Nombre;
            txtDistrito.Text = b.Distrito;
            txtDireccion.Text = b.Direccion;
        }
    }

    private void LimpiarBotica()
    {
        _idBotica = 0;
        txtNombreB.Clear();
        txtDistrito.Clear();
        txtDireccion.Clear();
    }
}