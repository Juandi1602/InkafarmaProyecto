using InkafarmaProyecto.Interfaces;
using InkafarmaProyecto.Modelos;

namespace InkafarmaProyecto.Formularios;

public partial class FormClientes : Form
{
    private readonly IClienteRepositorio _repo;
    private int _idSeleccionado = 0;

    private readonly TextBox txtDni = new() { Left = 100, Top = 15, Width = 220, MaxLength = 8 };
    private readonly TextBox txtNombres = new() { Left = 100, Top = 45, Width = 220 };
    private readonly TextBox txtTelefono = new() { Left = 100, Top = 75, Width = 220 };
    private readonly TextBox txtCorreo = new() { Left = 100, Top = 105, Width = 220 };
    private readonly Button btnGuardar = new() { Text = "Guardar", Left = 340, Top = 14, Width = 90 };
    private readonly Button btnEliminar = new() { Text = "Eliminar", Left = 340, Top = 44, Width = 90 };
    private readonly Button btnLimpiar = new() { Text = "Limpiar", Left = 340, Top = 74, Width = 90 };
    private readonly DataGridView dgv = new()
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

    public FormClientes(IClienteRepositorio repo)
    {
        InitializeComponent();
        _repo = repo;

        Text = "Clientes";
        ClientSize = new Size(670, 390);
        StartPosition = FormStartPosition.CenterScreen;

        Controls.Add(new Label { Text = "DNI", Left = 15, Top = 18, Width = 80 });
        Controls.Add(new Label { Text = "Nombres", Left = 15, Top = 48, Width = 80 });
        Controls.Add(new Label { Text = "Teléfono", Left = 15, Top = 78, Width = 80 });
        Controls.Add(new Label { Text = "Correo", Left = 15, Top = 108, Width = 80 });
        Controls.AddRange(new Control[] { txtDni, txtNombres, txtTelefono, txtCorreo, btnGuardar, btnEliminar, btnLimpiar, dgv });

        btnGuardar.Click += (s, e) => Guardar();
        btnEliminar.Click += (s, e) => Eliminar();
        btnLimpiar.Click += (s, e) => Limpiar();
        dgv.CellClick += (s, e) => Seleccionar();

        Cargar();
    }

    private void Cargar()
    {
        dgv.DataSource = _repo.Listar();
    }

    private void Guardar()
    {
        if (txtDni.Text.Length != 8 || string.IsNullOrWhiteSpace(txtNombres.Text))
        {
            MessageBox.Show("DNI de 8 dígitos y nombres obligatorios.");
            return;
        }

        var x = new Cliente
        {
            IdCliente = _idSeleccionado,
            Dni = txtDni.Text.Trim(),
            Nombres = txtNombres.Text.Trim(),
            Telefono = txtTelefono.Text.Trim(),
            Correo = txtCorreo.Text.Trim()
        };

        try
        {
            if (_idSeleccionado == 0) _repo.Insertar(x);
            else _repo.Actualizar(x);
            Limpiar();
            Cargar();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message);
        }
    }

    private void Eliminar()
    {
        if (_idSeleccionado == 0) return;
        try
        {
            _repo.Eliminar(_idSeleccionado);
            Limpiar();
            Cargar();
        }
        catch (Exception ex)
        {
            MessageBox.Show("No se puede eliminar (tiene ventas asociadas): " + ex.Message);
        }
    }

    private void Seleccionar()
    {
        if (dgv.CurrentRow?.DataBoundItem is Cliente x)
        {
            _idSeleccionado = x.IdCliente;
            txtDni.Text = x.Dni;
            txtNombres.Text = x.Nombres;
            txtTelefono.Text = x.Telefono;
            txtCorreo.Text = x.Correo;
        }
    }

    private void Limpiar()
    {
        _idSeleccionado = 0;
        txtDni.Clear();
        txtNombres.Clear();
        txtTelefono.Clear();
        txtCorreo.Clear();
    }
}