using System.ComponentModel;
using InkafarmaProyecto.Interfaces;
using InkafarmaProyecto.Modelos;
using InkafarmaProyecto.Servicios;

namespace InkafarmaProyecto.Formularios;

public partial class FormVentas : Form
{
    private readonly VentaServicio _servicio;
    private readonly IProductoRepositorio _productos;
    private readonly BindingList<ItemVenta> _items = new();

    private readonly ComboBox cmbCliente = new() { Left = 100, Top = 15, Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox cmbBotica = new() { Left = 100, Top = 45, Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox cmbProducto = new() { Left = 100, Top = 75, Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly NumericUpDown nudCantidad = new() { Left = 470, Top = 75, Width = 60, Minimum = 1, Maximum = 999 };
    private readonly Button btnAgregar = new() { Text = "Agregar", Left = 550, Top = 74, Width = 90 };
    private readonly Button btnQuitar = new() { Text = "Quitar", Left = 450, Top = 245, Width = 90 };
    private readonly Button btnRegistrar = new() { Text = "Registrar venta", Left = 550, Top = 245, Width = 150 };
    private readonly Label lblTotal = new() { Text = "Total: S/ 0.00", Left = 15, Top = 250, Width = 250, Font = new Font("Segoe UI", 11, FontStyle.Bold) };
    private readonly DataGridView dgvItems = new()
    {
        Left = 15,
        Top = 110,
        Width = 725,
        Height = 125,
        ReadOnly = true,
        AllowUserToAddRows = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    };
    private readonly DataGridView dgvVentas = new()
    {
        Left = 15,
        Top = 310,
        Width = 725,
        Height = 190,
        ReadOnly = true,
        AllowUserToAddRows = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    };

    public FormVentas(VentaServicio servicio, IClienteRepositorio clientes, IBoticaRepositorio boticas, IProductoRepositorio productos)
    {
        InitializeComponent();
        _servicio = servicio;
        _productos = productos;

        Text = "Ventas";
        ClientSize = new Size(760, 515);
        StartPosition = FormStartPosition.CenterScreen;

        Controls.Add(new Label { Text = "Cliente", Left = 15, Top = 18, Width = 80 });
        Controls.Add(new Label { Text = "Botica", Left = 15, Top = 48, Width = 80 });
        Controls.Add(new Label { Text = "Producto", Left = 15, Top = 78, Width = 80 });
        Controls.Add(new Label { Text = "Cant.", Left = 420, Top = 78, Width = 45 });
        Controls.Add(new Label { Text = "Historial de ventas", Left = 15, Top = 287, Width = 200 });
        Controls.AddRange(new Control[] { cmbCliente, cmbBotica, cmbProducto, nudCantidad, btnAgregar, dgvItems, lblTotal, btnQuitar, btnRegistrar, dgvVentas });

        cmbCliente.DataSource = clientes.Listar();
        cmbCliente.DisplayMember = "Nombres";
        cmbBotica.DataSource = boticas.Listar();
        cmbBotica.DisplayMember = "Nombre";
        CargarProductos();

        dgvItems.DataSource = _items;
        btnAgregar.Click += (s, e) => Agregar();
        btnQuitar.Click += (s, e) => Quitar();
        btnRegistrar.Click += (s, e) => Registrar();

        CargarVentas();
    }

    private void CargarProductos()
    {
        cmbProducto.DataSource = _productos.Listar();
        cmbProducto.DisplayMember = "Nombre";
    }

    private void CargarVentas()
    {
        dgvVentas.DataSource = _servicio.Listar();
    }

    private void ActualizarTotal()
    {
        lblTotal.Text = $"Total: S/ {_items.Sum(i => i.Subtotal):0.00}";
    }

    private void Agregar()
    {
        if (cmbProducto.SelectedItem is not Producto p) return;

        int cantidad = (int)nudCantidad.Value;
        var existente = _items.FirstOrDefault(i => i.IdProducto == p.IdProducto);
        int total = cantidad + (existente?.Cantidad ?? 0);

        if (total > p.Stock)
        {
            MessageBox.Show($"Stock insuficiente (disponible: {p.Stock}).");
            return;
        }

        if (existente != null)
        {
            existente.Cantidad = total;
            _items.ResetBindings();
        }
        else
        {
            _items.Add(new ItemVenta
            {
                IdProducto = p.IdProducto,
                Producto = p.Nombre,
                Cantidad = cantidad,
                PrecioUnitario = p.Precio
            });
        }
        ActualizarTotal();
    }

    private void Quitar()
    {
        if (dgvItems.CurrentRow?.DataBoundItem is ItemVenta it)
        {
            _items.Remove(it);
            ActualizarTotal();
        }
    }

    private void Registrar()
    {
        if (cmbCliente.SelectedItem is not Cliente c || cmbBotica.SelectedItem is not Botica b) return;

        try
        {
            string boleta = _servicio.Registrar(c.IdCliente, b.IdBotica, _items.ToList());
            MessageBox.Show($"Venta registrada: {boleta}");
            _items.Clear();
            ActualizarTotal();
            CargarVentas();
            CargarProductos();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}