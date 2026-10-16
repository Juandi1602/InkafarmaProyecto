using InkafarmaProyecto.Modelos;
using InkafarmaProyecto.Servicios;

namespace InkafarmaProyecto.Formularios;

public partial class FormDevoluciones : Form
{
    private readonly DevolucionServicio _servicio;

    private readonly ComboBox cmbDetalle = new() { Left = 100, Top = 15, Width = 450, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly NumericUpDown nudCantidad = new() { Left = 100, Top = 45, Width = 60, Minimum = 1, Maximum = 999 };
    private readonly ComboBox cmbMotivo = new() { Left = 100, Top = 75, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly TextBox txtObs = new() { Left = 100, Top = 105, Width = 450 };
    private readonly Button btnRegistrar = new() { Text = "Registrar", Left = 570, Top = 14, Width = 100 };
    private readonly ComboBox cmbResolucion = new() { Left = 100, Top = 350, Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox cmbDestino = new() { Left = 330, Top = 350, Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Button btnAprobar = new() { Text = "Aprobar", Left = 480, Top = 349, Width = 90 };
    private readonly Button btnRechazar = new() { Text = "Rechazar", Left = 580, Top = 349, Width = 90 };
    private readonly DataGridView dgv = new()
    {
        Left = 15,
        Top = 145,
        Width = 730,
        Height = 190,
        ReadOnly = true,
        AllowUserToAddRows = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    };

    public FormDevoluciones(DevolucionServicio servicio)
    {
        InitializeComponent();
        _servicio = servicio;

        Text = "Devoluciones de clientes";
        ClientSize = new Size(760, 395);
        StartPosition = FormStartPosition.CenterScreen;

        cmbMotivo.Items.AddRange(new object[] { "Producto dañado", "Error en la venta", "Producto equivocado", "Reacción adversa", "Otro" });
        cmbMotivo.SelectedIndex = 0;
        cmbResolucion.Items.AddRange(new object[] { "Reembolso", "Cambio" });
        cmbResolucion.SelectedIndex = 0;
        cmbDestino.Items.AddRange(new object[] { "Reponer stock", "Merma" });
        cmbDestino.SelectedIndex = 0;

        Controls.Add(new Label { Text = "Venta", Left = 15, Top = 18, Width = 80 });
        Controls.Add(new Label { Text = "Cantidad", Left = 15, Top = 48, Width = 80 });
        Controls.Add(new Label { Text = "Motivo", Left = 15, Top = 78, Width = 80 });
        Controls.Add(new Label { Text = "Observación", Left = 15, Top = 108, Width = 80 });
        Controls.Add(new Label { Text = "Resolución", Left = 15, Top = 353, Width = 80 });
        Controls.Add(new Label { Text = "Destino", Left = 250, Top = 353, Width = 80 });
        Controls.AddRange(new Control[] { cmbDetalle, nudCantidad, cmbMotivo, txtObs, btnRegistrar, cmbResolucion, cmbDestino, btnAprobar, btnRechazar, dgv });

        btnRegistrar.Click += (s, e) => Registrar();
        btnAprobar.Click += (s, e) => Aprobar();
        btnRechazar.Click += (s, e) => Rechazar();

        cmbDetalle.DataSource = _servicio.ListarDetallesVenta();
        cmbDetalle.DisplayMember = "Texto";
        Cargar();
    }

    private void Cargar()
    {
        dgv.DataSource = _servicio.Listar();
    }

    private void Registrar()
    {
        if (cmbDetalle.SelectedItem is not DetalleVentaVista d)
        {
            MessageBox.Show("Selecciona una venta.");
            return;
        }

        try
        {
            var error = _servicio.Registrar(d, (int)nudCantidad.Value, cmbMotivo.Text, txtObs.Text.Trim());
            if (error != null)
            {
                MessageBox.Show(error);
                return;
            }
            txtObs.Clear();
            Cargar();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message);
        }
    }

    private void Aprobar()
    {
        if (dgv.CurrentRow?.DataBoundItem is not DevolucionVista v) return;
        try
        {
            _servicio.Aprobar(v.IdDevolucion, cmbResolucion.Text, cmbDestino.Text);
            Cargar();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void Rechazar()
    {
        if (dgv.CurrentRow?.DataBoundItem is not DevolucionVista v) return;
        try
        {
            _servicio.Rechazar(v.IdDevolucion);
            Cargar();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}