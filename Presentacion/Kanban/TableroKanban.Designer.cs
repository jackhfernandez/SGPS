namespace Presentacion.Kanban
{
    partial class TableroKanban
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pnlSelector = new Panel();
            lblProyecto = new Label();
            cboProyecto = new ComboBox();
            lblSprint = new Label();
            cboSprint = new ComboBox();
            chkSoloMias = new CheckBox();
            btnActualizar = new Button();
            tlpColumnas = new TableLayoutPanel();
            lblResumen = new Label();
            pnlSelector.SuspendLayout();
            SuspendLayout();
            //
            // pnlSelector
            //
            pnlSelector.BackColor = Color.FromArgb(245, 241, 232);
            pnlSelector.Controls.Add(cboProyecto);
            pnlSelector.Controls.Add(lblProyecto);
            pnlSelector.Controls.Add(cboSprint);
            pnlSelector.Controls.Add(lblSprint);
            pnlSelector.Controls.Add(chkSoloMias);
            pnlSelector.Controls.Add(btnActualizar);
            pnlSelector.Dock = DockStyle.Top;
            pnlSelector.Location = new Point(0, 0);
            pnlSelector.Name = "pnlSelector";
            pnlSelector.Size = new Size(1000, 56);
            pnlSelector.TabIndex = 0;
            //
            // lblProyecto
            //
            lblProyecto.Font = new Font("Segoe UI Semibold", 9.5F);
            lblProyecto.ForeColor = Color.FromArgb(44, 51, 47);
            lblProyecto.Location = new Point(24, 18);
            lblProyecto.Name = "lblProyecto";
            lblProyecto.Size = new Size(70, 21);
            lblProyecto.TabIndex = 0;
            lblProyecto.Text = "Proyecto:";
            lblProyecto.TextAlign = ContentAlignment.MiddleLeft;
            //
            // cboProyecto
            //
            cboProyecto.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProyecto.Font = new Font("Segoe UI", 10F);
            cboProyecto.FormattingEnabled = true;
            cboProyecto.Location = new Point(98, 14);
            cboProyecto.Name = "cboProyecto";
            cboProyecto.Size = new Size(260, 25);
            cboProyecto.TabIndex = 1;
            cboProyecto.SelectedIndexChanged += cboProyecto_SelectedIndexChanged;
            //
            // lblSprint
            //
            lblSprint.Font = new Font("Segoe UI Semibold", 9.5F);
            lblSprint.ForeColor = Color.FromArgb(44, 51, 47);
            lblSprint.Location = new Point(376, 18);
            lblSprint.Name = "lblSprint";
            lblSprint.Size = new Size(50, 21);
            lblSprint.TabIndex = 2;
            lblSprint.Text = "Sprint:";
            lblSprint.TextAlign = ContentAlignment.MiddleLeft;
            //
            // cboSprint
            //
            cboSprint.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSprint.Font = new Font("Segoe UI", 10F);
            cboSprint.FormattingEnabled = true;
            cboSprint.Location = new Point(430, 14);
            cboSprint.Name = "cboSprint";
            cboSprint.Size = new Size(250, 25);
            cboSprint.TabIndex = 3;
            cboSprint.SelectedIndexChanged += cboSprint_SelectedIndexChanged;
            //
            // chkSoloMias
            //
            chkSoloMias.Font = new Font("Segoe UI", 9.75F);
            chkSoloMias.ForeColor = Color.FromArgb(44, 51, 47);
            chkSoloMias.Location = new Point(700, 16);
            chkSoloMias.Name = "chkSoloMias";
            chkSoloMias.Size = new Size(150, 24);
            chkSoloMias.TabIndex = 4;
            chkSoloMias.Text = "Solo mis historias";
            chkSoloMias.UseVisualStyleBackColor = true;
            chkSoloMias.CheckedChanged += chkSoloMias_CheckedChanged;
            //
            // btnActualizar
            //
            btnActualizar.BackColor = Color.FromArgb(12, 110, 99);
            btnActualizar.FlatAppearance.BorderSize = 0;
            btnActualizar.FlatStyle = FlatStyle.Flat;
            btnActualizar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnActualizar.ForeColor = Color.White;
            btnActualizar.Location = new Point(862, 12);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(110, 30);
            btnActualizar.TabIndex = 5;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = false;
            btnActualizar.Click += btnActualizar_Click;
            //
            // tlpColumnas
            //
            tlpColumnas.BackColor = Color.FromArgb(245, 241, 232);
            tlpColumnas.ColumnCount = 4;
            tlpColumnas.Dock = DockStyle.Fill;
            tlpColumnas.Location = new Point(0, 56);
            tlpColumnas.Name = "tlpColumnas";
            tlpColumnas.Padding = new Padding(16, 8, 16, 8);
            tlpColumnas.RowCount = 1;
            tlpColumnas.Size = new Size(1000, 550);
            tlpColumnas.TabIndex = 1;
            //
            // lblResumen
            //
            lblResumen.BackColor = Color.FromArgb(245, 241, 232);
            lblResumen.Dock = DockStyle.Bottom;
            lblResumen.Font = new Font("Segoe UI", 9F);
            lblResumen.ForeColor = Color.FromArgb(44, 51, 47);
            lblResumen.Location = new Point(0, 606);
            lblResumen.Name = "lblResumen";
            lblResumen.Padding = new Padding(20, 8, 20, 0);
            lblResumen.Size = new Size(1000, 34);
            lblResumen.TabIndex = 2;
            lblResumen.Text = "Selecciona un proyecto para ver su tablero.";
            //
            // TableroKanban
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 241, 232);
            ClientSize = new Size(1000, 640);
            Controls.Add(tlpColumnas);
            Controls.Add(pnlSelector);
            Controls.Add(lblResumen);
            MinimumSize = new Size(880, 480);
            Name = "TableroKanban";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Tablero Kanban";
            pnlSelector.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlSelector;
        private Label lblProyecto;
        private ComboBox cboProyecto;
        private Label lblSprint;
        private ComboBox cboSprint;
        private CheckBox chkSoloMias;
        private Button btnActualizar;
        private TableLayoutPanel tlpColumnas;
        private Label lblResumen;
    }
}
