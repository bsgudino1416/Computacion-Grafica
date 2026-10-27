using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using GeometriaInteractiva.Models;
using GeometriaInteractiva.MathEngine;
using GeometriaInteractiva.Testing;

namespace GeometriaInteractiva.UI
{
    public class MainForm : Form
    {
        private Panel canvas;
        private ComboBox cbFiguras;
        private NumericUpDown numRotacion, numEscala, numPuntas;
        private Button btnEjecutarPruebas;
        private TextBox txtSalidaResultados;
        private Panel pnlControls;
        private Label lblFigura, lblRotacion, lblEscala, lblPuntas;

        private FiguraGeometrica figuraActual;
        private PointF puntoProyeccion = new PointF(150, 150);

        public MainForm()
        {
            InitializeComponent();      // 1. Inicializa los controles visuales para el Diseñador
            ConfigurarLogica();         // 2. Configura eventos, reflexión y lógica en tiempo de ejecución
            SeleccionarFigura(0);
        }

        // Método reservado para el Diseñador Gráfico de Visual Studio
        private void InitializeComponent()
        {
            this.pnlControls = new Panel();
            this.canvas = new Panel();
            this.lblFigura = new Label();
            this.cbFiguras = new ComboBox();
            this.lblRotacion = new Label();
            this.numRotacion = new NumericUpDown();
            this.lblEscala = new Label();
            this.numEscala = new NumericUpDown();
            this.lblPuntas = new Label();
            this.numPuntas = new NumericUpDown();
            this.btnEjecutarPruebas = new Button();
            this.txtSalidaResultados = new TextBox();

            this.SuspendLayout();

            // Configuración del Formulario Principal
            this.Text = "Computación Gráfica - Figuras e Interacción Geométrica";
            this.Size = new Size(1100, 700);
            this.MinimumSize = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Panel Lateral de Control
            this.pnlControls.Dock = DockStyle.Left;
            this.pnlControls.Width = 280;
            this.pnlControls.BackColor = Color.LightGray;

            // Lienzo Gráfico
            this.canvas.Dock = DockStyle.Fill;
            this.canvas.BackColor = Color.White;

            // Controles Individuales
            this.lblFigura.Text = "Seleccionar Figura:";
            this.lblFigura.Location = new Point(10, 15);
            this.lblFigura.AutoSize = true;

            this.cbFiguras.Location = new Point(10, 35);
            this.cbFiguras.Width = 250;
            this.cbFiguras.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbFiguras.Items.AddRange(new object[] {
                "Hexágono de 6 Colores",
                "Flor de Margarita",
                "Polígono Estrellado"
            });
            this.cbFiguras.SelectedIndex = 0;

            this.lblRotacion.Text = "Rotación (Grados):";
            this.lblRotacion.Location = new Point(10, 75);
            this.lblRotacion.AutoSize = true;

            this.numRotacion.Location = new Point(10, 95);
            this.numRotacion.Minimum = 0;
            this.numRotacion.Maximum = 360;
            this.numRotacion.Value = 0;
            this.numRotacion.Increment = 5;

            this.lblEscala.Text = "Escala:";
            this.lblEscala.Location = new Point(10, 135);
            this.lblEscala.AutoSize = true;

            this.numEscala.Location = new Point(10, 155);
            this.numEscala.Minimum = (decimal)0.1;
            this.numEscala.Maximum = 5;
            this.numEscala.Value = 1;
            this.numEscala.DecimalPlaces = 1;
            this.numEscala.Increment = (decimal)0.1;

            this.lblPuntas.Text = "Puntas (Solo Estrella):";
            this.lblPuntas.Location = new Point(10, 195);
            this.lblPuntas.AutoSize = true;

            this.numPuntas.Location = new Point(10, 215);
            this.numPuntas.Minimum = 3;
            this.numPuntas.Maximum = 20;
            this.numPuntas.Value = 5;

            this.btnEjecutarPruebas.Text = "Ejecutar Casos de Prueba";
            this.btnEjecutarPruebas.Location = new Point(10, 260);
            this.btnEjecutarPruebas.Width = 250;
            this.btnEjecutarPruebas.Height = 30;

            this.txtSalidaResultados.Location = new Point(10, 300);
            this.txtSalidaResultados.Width = 250;
            this.txtSalidaResultados.Height = 340;
            this.txtSalidaResultados.Multiline = true;
            this.txtSalidaResultados.ScrollBars = ScrollBars.Vertical;
            this.txtSalidaResultados.ReadOnly = true;
            this.txtSalidaResultados.Font = new Font("Consolas", 8);

            // Agregar Controles al Panel Lateral y a la Ventana
            this.pnlControls.Controls.Add(this.lblFigura);
            this.pnlControls.Controls.Add(this.cbFiguras);
            this.pnlControls.Controls.Add(this.lblRotacion);
            this.pnlControls.Controls.Add(this.numRotacion);
            this.pnlControls.Controls.Add(this.lblEscala);
            this.pnlControls.Controls.Add(this.numEscala);
            this.pnlControls.Controls.Add(this.lblPuntas);
            this.pnlControls.Controls.Add(this.numPuntas);
            this.pnlControls.Controls.Add(this.btnEjecutarPruebas);
            this.pnlControls.Controls.Add(this.txtSalidaResultados);

            this.Controls.Add(this.canvas);
            this.Controls.Add(this.pnlControls);

            this.ResumeLayout(false);
        }

        // Lógica de eventos y características de ejecución
        private void ConfigurarLogica()
        {
            // Activar Doble Búfer mediante Reflexión para evitar parpadeo
            typeof(Panel).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(canvas, true, null);

            // Suscripción de Eventos
            canvas.Paint += Canvas_Paint;
            canvas.MouseDown += Canvas_MouseDown;

            cbFiguras.SelectedIndexChanged += (s, e) => SeleccionarFigura(cbFiguras.SelectedIndex);
            numRotacion.ValueChanged += ActualizarTransformaciones;
            numEscala.ValueChanged += ActualizarTransformaciones;
            numPuntas.ValueChanged += (s, e) => { if (cbFiguras.SelectedIndex == 2) SeleccionarFigura(2); };

            btnEjecutarPruebas.Click += (s, e) => txtSalidaResultados.Text = ValidadorMatematico.EjecutarCasosDePrueba();
        }

        private void SeleccionarFigura(int index)
        {
            PointF centro = new PointF(canvas.Width > 0 ? canvas.Width / 2 : 350, canvas.Height > 0 ? canvas.Height / 2 : 250);

            switch (index)
            {
                case 0:
                    figuraActual = new HexagonoSeisColores(centro, 120);
                    break;
                case 1:
                    figuraActual = new FlorMargarita(centro, 60, 50);
                    break;
                case 2:
                    figuraActual = new PoligonoEstrellado(centro, (int)numPuntas.Value, 130, 50);
                    break;
            }
            ActualizarTransformaciones(null, null);
        }

        private void ActualizarTransformaciones(object sender, EventArgs e)
        {
            if (figuraActual != null)
            {
                figuraActual.AnguloRotacion = (double)numRotacion.Value;
                figuraActual.Escala = (double)numEscala.Value;

                txtSalidaResultados.Text = $"=== PROPIEDADES DE LA FIGURA ===\r\n" +
                                           $"Área: {figuraActual.GetArea():N2} px²\r\n" +
                                           $"Perímetro: {figuraActual.GetPerimetro():N2} px\r\n" +
                                           $"Centro: ({figuraActual.Centro.X:F0}, {figuraActual.Centro.Y:F0})\r\n" +
                                           $"Rotación: {figuraActual.AnguloRotacion}°\r\n" +
                                           $"Escala: {figuraActual.Escala:F1}x";
                canvas.Invalidate();
            }
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                figuraActual.Centro = e.Location;
                ActualizarTransformaciones(null, null);
            }
            else if (e.Button == MouseButtons.Right)
            {
                puntoProyeccion = e.Location;
                canvas.Invalidate();
            }
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (figuraActual != null)
            {
                figuraActual.Dibujar(e.Graphics);

                if (figuraActual is PoligonoEstrellado estrella)
                {
                    var v = estrella.GetVerticesTransformados();
                    if (v.Count > 1)
                    {
                        PointF a = v[0];
                        PointF b = v[1];
                        PointF proyectado = GeometryMath.ProyectarPuntoEnSegmento(puntoProyeccion, a, b);

                        using (Pen pSegmento = new Pen(Color.Blue, 3))
                        using (Pen pLineaProy = new Pen(Color.Gray, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
                        {
                            e.Graphics.DrawLine(pSegmento, a, b);
                            e.Graphics.DrawLine(pLineaProy, puntoProyeccion, proyectado);
                            e.Graphics.FillEllipse(Brushes.Red, puntoProyeccion.X - 4, puntoProyeccion.Y - 4, 8, 8);
                            e.Graphics.FillEllipse(Brushes.Green, proyectado.X - 4, proyectado.Y - 4, 8, 8);
                        }
                    }
                }
            }
        }
    }
}