using System.Drawing;
using System.Windows.Forms;

namespace Presentacion.Modulos;

public class ProyectoCreacion : Form
{
    public ProyectoCreacion()
    {
        Text = "Nuevo proyecto";
        Controls.Add(new Label
        {
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            Text = "Creación de proyecto (TASK-UI-04 pendiente)"
        });
    }
}