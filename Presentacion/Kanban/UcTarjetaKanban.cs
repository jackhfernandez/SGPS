using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Modelo;
using Presentacion.Ui;

namespace Presentacion.Kanban
{
    /// <summary>
    /// Tarjeta visual de una historia dentro del tablero. No es una pantalla:
    /// la instancia <see cref="TableroKanban"/> y por eso no está en el mapa de
    /// navegación. Se pinta a mano porque WinForms no ofrece esquinas
    /// redondeadas y porque una tarjeta compuesta de labels no se puede
    /// arrastrar de una pieza.
    /// </summary>
    public partial class UcTarjetaKanban : UserControl
    {
        /// <summary>Alto fijo de la tarjeta; el ancho lo impone la columna.</summary>
        public const int AltoTarjeta = 116;

        private const int Margen = 12;

        /// <summary>Alto de la franja inferior (epic y avatar del responsable).</summary>
        private const int AltoPie = 24;

        private UserStory? _historia;
        private bool _seleccionada;
        private bool _hover;
        private Point _origenArrastre;

        public UcTarjetaKanban()
        {
            InitializeComponent();

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint,
                true);

            Height = AltoTarjeta;
            Margin = new Padding(0, 0, 0, 8);
            Cursor = Cursors.Hand;
            BackColor = Tema.Blanco;
            AccessibleRole = AccessibleRole.ListItem;
        }

        public UcTarjetaKanban(UserStory historia) : this()
        {
            Enlazar(historia);
        }

        /// <summary>Historia que representa la tarjeta.</summary>
        public UserStory? Historia => _historia;

        // Las tarjetas se construyen siempre desde el tablero, nunca desde el
        // disenador, por eso ninguna propiedad se serializa.

        /// <summary>Habilita el arrastre entre columnas (requiere nivel Edición).</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool PermiteArrastre { get; set; }

        /// <summary>Se dispara al hacer doble clic: el tablero abre el desglose técnico.</summary>
        public event EventHandler? TarjetaAbierta;

        /// <summary>Se dispara al hacer clic simple sobre la tarjeta.</summary>
        public event EventHandler? TarjetaElegida;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Seleccionada
        {
            get => _seleccionada;
            set
            {
                if (_seleccionada == value)
                {
                    return;
                }

                _seleccionada = value;
                Invalidate();
            }
        }

        /// <summary>Vuelca los datos de la historia en la tarjeta.</summary>
        public void Enlazar(UserStory historia)
        {
            _historia = historia;

            AccessibleName = $"{historia.CodigoTicket} {historia.Titulo}";
            tipDetalle.SetToolTip(this, ConstruirDetalle(historia));

            Invalidate();
        }

        private static string ConstruirDetalle(UserStory historia)
        {
            var asignado = string.IsNullOrWhiteSpace(historia.UsuarioAsignadoNombre)
                ? "sin asignar"
                : historia.UsuarioAsignadoNombre;

            return $"{historia.CodigoTicket} · {historia.Titulo}\n" +
                   $"{historia.FormatoAgilTexto}\n" +
                   $"Epic: {historia.EpicNombre ?? "—"}  ·  Valor: {historia.ValorNegocio}  ·  " +
                   $"{historia.StoryPoints} SP  ·  Responsable: {asignado}\n" +
                   "Doble clic para abrir el desglose técnico.";
        }

        // ------------------------------------------------------------ Arrastre

        protected override void OnMouseDown(MouseEventArgs e)
        {
            _origenArrastre = e.Location;
            TarjetaElegida?.Invoke(this, EventArgs.Empty);
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            // Se exige superar el umbral del sistema para no convertir en
            // arrastre cualquier clic con un temblor de raton.
            if (PermiteArrastre &&
                e.Button == MouseButtons.Left &&
                _historia is not null &&
                (Math.Abs(e.X - _origenArrastre.X) > SystemInformation.DragSize.Width ||
                 Math.Abs(e.Y - _origenArrastre.Y) > SystemInformation.DragSize.Height))
            {
                DoDragDrop(this, DragDropEffects.Move);
            }

            base.OnMouseMove(e);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            TarjetaAbierta?.Invoke(this, EventArgs.Empty);
            base.OnDoubleClick(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _hover = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _hover = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        // ------------------------------------------------------------- Pintado

        protected override void OnPaint(PaintEventArgs e)
        {
            // Nada de asignar propiedades del control aqui: los setters llaman
            // a Invalidate() y eso encadena un repintado infinito.
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            using (var fondo = new SolidBrush(Tema.Crema))
            {
                g.FillRectangle(fondo, ClientRectangle);
            }

            var marco = new Rectangle(0, 0, Width - 1, Height - 1);

            using (var camino = PanelTarjeta.CrearCamino(marco, 8))
            {
                using (var relleno = new SolidBrush(_hover ? Tema.CremaHover : Tema.Blanco))
                {
                    g.FillPath(relleno, camino);
                }

                using var lapiz = new Pen(_seleccionada ? Tema.Teal : Tema.Borde, _seleccionada ? 2 : 1);
                g.DrawPath(lapiz, camino);
            }

            if (_historia is null)
            {
                base.OnPaint(e);
                return;
            }

            DibujarFranjaValor(g, _historia);
            DibujarEncabezado(g, _historia);
            DibujarTitulo(g, _historia);
            DibujarPie(g, _historia);

            base.OnPaint(e);
        }

        /// <summary>Franja izquierda con el color del valor de negocio.</summary>
        private void DibujarFranjaValor(Graphics g, UserStory historia)
        {
            using var franja = new SolidBrush(ColorDeValor(historia.ValorNegocio));
            using var camino = PanelTarjeta.CrearCamino(new Rectangle(0, 0, 10, Height - 1), 8);

            g.FillPath(franja, camino);
            g.FillRectangle(franja, new Rectangle(5, 1, 5, Height - 3));
        }

        private static Color ColorDeValor(string valorNegocio) => valorNegocio switch
        {
            ValorNegocioConstantes.Alto => Tema.Coral,
            ValorNegocioConstantes.Bajo => Tema.Oliva,
            _ => Tema.Dorado
        };

        /// <summary>Codigo de ticket a la izquierda y chip de puntos a la derecha.</summary>
        private void DibujarEncabezado(Graphics g, UserStory historia)
        {
            using (var pincelCodigo = new SolidBrush(Tema.TextoTenue))
            {
                g.DrawString(historia.CodigoTicket, Tema.Etiqueta, pincelCodigo,
                    new RectangleF(Margen + 8, 10, Width - Margen * 2 - 60, 16));
            }

            var textoPuntos = $"{historia.StoryPoints} SP";
            var anchoChip = (int)g.MeasureString(textoPuntos, Tema.Etiqueta).Width + 14;
            var chip = new Rectangle(Width - Margen - anchoChip, 8, anchoChip, 19);

            using (var camino = PanelTarjeta.CrearCamino(chip, 9))
            using (var fondoChip = new SolidBrush(Tema.SeleccionSuave))
            {
                g.FillPath(fondoChip, camino);
            }

            using var pincelChip = new SolidBrush(Tema.Teal);
            using var centro = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            g.DrawString(textoPuntos, Tema.Etiqueta, pincelChip, chip, centro);
        }

        /// <summary>
        /// El titulo ocupa todo lo que queda entre el encabezado y el pie, para
        /// que un titulo de dos lineas no se monte sobre el nombre del epic.
        /// </summary>
        private void DibujarTitulo(Graphics g, UserStory historia)
        {
            const int arriba = 30;
            var alto = Height - Margen - AltoPie - 4 - arriba;

            using var pincel = new SolidBrush(Tema.TextoOscuro);
            using var formato = new StringFormat { Trimming = StringTrimming.EllipsisWord };

            g.DrawString(historia.Titulo, Tema.CuerpoSemi, pincel,
                new RectangleF(Margen + 8, arriba, Width - Margen * 2 - 8, alto), formato);
        }

        /// <summary>Epic a la izquierda y avatar del responsable a la derecha.</summary>
        private void DibujarPie(Graphics g, UserStory historia)
        {
            const int diametro = AltoPie;
            var avatar = new Rectangle(Width - Margen - diametro, Height - Margen - diametro, diametro, diametro);

            var asignado = historia.UsuarioAsignadoNombre;
            var hayResponsable = !string.IsNullOrWhiteSpace(asignado);

            using (var fondoAvatar = new SolidBrush(hayResponsable ? Tema.VerdeProfundo : Tema.Borde))
            {
                g.FillEllipse(fondoAvatar, avatar);
            }

            using (var pincelIniciales = new SolidBrush(hayResponsable ? Tema.Blanco : Tema.TextoTenue))
            using (var centro = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            })
            {
                g.DrawString(hayResponsable ? Tema.Iniciales(asignado) : "?", Tema.Etiqueta,
                    pincelIniciales, avatar, centro);
            }

            using var pincelEpic = new SolidBrush(Tema.TextoTenue);
            using var formato = new StringFormat
            {
                Trimming = StringTrimming.EllipsisCharacter,
                FormatFlags = StringFormatFlags.NoWrap,
                LineAlignment = StringAlignment.Center
            };

            g.DrawString(historia.EpicNombre ?? "Sin epic", Tema.Etiqueta, pincelEpic,
                new RectangleF(Margen + 8, avatar.Top, Width - Margen * 2 - diametro - 16, diametro), formato);
        }
    }
}
