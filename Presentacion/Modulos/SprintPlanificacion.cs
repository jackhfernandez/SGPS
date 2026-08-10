using System.Drawing;
using System.Windows.Forms;

namespace Presentacion.Modulos;

public class SprintPlanificacion : Form
{
    public SprintPlanificacion()
    {
        Text = "Sprints";
        Controls.Add(new Label
        {
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            Text = "Planificación de Sprints (TASK-UI-06 pendiente)"
        });
    }
}