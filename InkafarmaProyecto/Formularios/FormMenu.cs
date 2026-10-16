namespace InkafarmaProyecto.Formularios;

public partial class FormMenu : Form
{
    public FormMenu(IEnumerable<(string Titulo, Func<Form> Crear)> opciones)
    {
        InitializeComponent();

        var lista = opciones.ToList();

        Text = "Inkafarma - Devolución de productos a clientes";
        ClientSize = new Size(360, 80 + lista.Count * 50);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        Controls.Add(new Label
        {
            Text = "Menú principal",
            Left = 40,
            Top = 20,
            Width = 280,
            Font = new Font("Segoe UI", 13, FontStyle.Bold)
        });

        for (int i = 0; i < lista.Count; i++)
        {
            var crear = lista[i].Crear;
            var boton = new Button
            {
                Text = lista[i].Titulo,
                Left = 40,
                Top = 65 + i * 50,
                Width = 280,
                Height = 40
            };
            boton.Click += (s, e) =>
            {
                using var ventana = crear();
                ventana.ShowDialog(this);
            };
            Controls.Add(boton);
        }
    }
}