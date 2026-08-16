using System;
using System.Collections.Generic;
using System.Text;

namespace Presentacion.Seguridad
{
    public class UsuarioNuevo : Form
    {
        private readonly TextBox _txtNombres;
        private readonly TextBox _txtApellidos;
        private readonly TextBox _txtEmail;
        private readonly TextBox _txtPassword;

        public string Nombres => _txtNombres.Text.Trim();
        public string Apellidos => _txtApellidos.Text.Trim();
        public string Email => _txtEmail.Text.Trim();
        public string Password => _txtPassword.Text;

        public UsuarioNuevo()
        {
            Text = "Nuevo usuario";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(245, 241, 232);
            ClientSize = new Size(384, 248);

            _txtNombres = CrearCajaDeTexto(20);
            _txtApellidos = CrearCajaDeTexto(62);
            _txtEmail = CrearCajaDeTexto(104);
            _txtPassword = CrearCajaDeTexto(146);
            _txtPassword.MaxLength = 64;
            _txtPassword.PasswordChar = '●';

            Controls.Add(CrearEtiqueta("Nombres:", 18));
            Controls.Add(_txtNombres);
            Controls.Add(CrearEtiqueta("Apellidos:", 60));
            Controls.Add(_txtApellidos);
            Controls.Add(CrearEtiqueta("Correo electrónico:", 102));
            Controls.Add(_txtEmail);
            Controls.Add(CrearEtiqueta("Contraseña:", 144));
            Controls.Add(_txtPassword);

            var btnGuardar = CrearBoton("Crear usuario", Color.FromArgb(12, 110, 99), 132, 196);
            btnGuardar.Click += BtnGuardar_Click;

            var btnCancelar = CrearBoton("Cancelar", Color.FromArgb(200, 200, 200), 252, 196);
            btnCancelar.ForeColor = Color.FromArgb(44, 51, 47);
            btnCancelar.DialogResult = DialogResult.Cancel;

            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);

            AcceptButton = btnGuardar;
            CancelButton = btnCancelar;
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Nombres))
            {
                MostrarValidacion("El nombre es obligatorio.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Apellidos))
            {
                MostrarValidacion("Los apellidos son obligatorios.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Email) || !Email.Contains('@'))
            {
                MostrarValidacion("El correo electrónico no tiene un formato válido.");
                return;
            }

            if (Password.Length < 6)
            {
                MostrarValidacion("La contraseña debe tener al menos 6 caracteres.");
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private static void MostrarValidacion(string mensaje)
        {
            MessageBox.Show(mensaje, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private static Label CrearEtiqueta(string texto, int y)
        {
            return new Label
            {
                Text = texto,
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 51, 47),
                Location = new Point(20, y),
                Size = new Size(112, 22),
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        private static TextBox CrearCajaDeTexto(int y)
        {
            return new TextBox
            {
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10F),
                Location = new Point(140, y),
                Size = new Size(224, 26),
                MaxLength = 150
            };
        }

        private static Button CrearBoton(string texto, Color color, int x, int y)
        {
            return new Button
            {
                Text = texto,
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Location = new Point(x, y),
                Size = new Size(112, 34),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
        }
    }
}
