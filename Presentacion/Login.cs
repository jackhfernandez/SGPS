using System;
using System.Windows.Forms;
using Logica;

namespace Presentacion
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                var usuarioLN = new UsuarioLN();
                var usuario = usuarioLN.Autenticar(txtCorreo.Text, txtContrasena.Text);

                if (usuario is null)
                {
                    MessageBox.Show(
                        "Correo o contraseña incorrectos.",
                        "Acceso denegado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    txtContrasena.Clear();
                    txtContrasena.Focus();
                    return;
                }

                var roles = usuarioLN.ObtenerRoles(usuario.UsuarioId);
                SesionContextoLN.IniciarSesion(usuario, roles);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo iniciar sesión: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
