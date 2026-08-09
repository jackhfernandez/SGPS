using System;
using System.Windows.Forms;
using Logica;

namespace Presentacion
{
    public partial class Principal : Form
    {
        public Principal()
        {
            InitializeComponent();
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            SesionContextoLN.CerrarSesion();
            Close();
        }
    }
}
