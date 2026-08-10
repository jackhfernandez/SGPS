using System.Drawing;
using System.Windows.Forms;

namespace Presentacion.Modulos;

public class ClientePortal : Form
{
    public ClientePortal()
    {
        Text = "Portal de cliente";
        Controls.Add(new Label
        {
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            Text = "Portal de cliente (TASK-UI-10 pendiente)"
        });
    }
}