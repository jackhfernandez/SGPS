using System.Drawing;
using System.Windows.Forms;

namespace Presentacion.Modulos;

public class ProductBacklogGestion : Form
{
    public ProductBacklogGestion()
    {
        Text = "Backlog";
        Controls.Add(new Label
        {
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            Text = "Product Backlog (TASK-UI-05 pendiente)"
        });
    }
}