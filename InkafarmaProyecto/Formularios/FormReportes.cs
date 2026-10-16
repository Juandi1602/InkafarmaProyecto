using InkafarmaProyecto.Interfaces;

namespace InkafarmaProyecto.Formularios;

public partial class FormReportes : Form
{
    private readonly ComboBox cmbReporte = new() { Left = 100, Top = 15, Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Button btnGenerar = new() { Text = "Generar", Left = 420, Top = 14, Width = 100 };
    private readonly DataGridView dgv = new()
    {
        Left = 15,
        Top = 55,
        Width = 640,
        Height = 300,
        ReadOnly = true,
        AllowUserToAddRows = false,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    };

    public FormReportes(IEnumerable<IReporte> reportes)
    {
        InitializeComponent();

        Text = "Reportes de devoluciones";
        ClientSize = new Size(670, 370);
        StartPosition = FormStartPosition.CenterScreen;

        Controls.Add(new Label { Text = "Reporte", Left = 15, Top = 18, Width = 80 });
        Controls.AddRange(new Control[] { cmbReporte, btnGenerar, dgv });

        cmbReporte.DataSource = reportes.ToList();
        cmbReporte.DisplayMember = "Nombre";

        btnGenerar.Click += (s, e) => Generar();
        Generar();
    }

    private void Generar()
    {
        if (cmbReporte.SelectedItem is not IReporte r) return;
        try
        {
            dgv.DataSource = r.Ejecutar();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message);
        }
    }
}