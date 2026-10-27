using System;
using System.Collections.Generic;
using System.Drawing;
using GeometriaInteractiva.MathEngine;

namespace GeometriaInteractiva.Models
{
    public class HexagonoSeisColores : FiguraGeometrica
    {
        public double Radio { get; set; }
        public Color[] ColoresTriangulos { get; set; }
        private List<PointF> verticesBase = new List<PointF>();

        public HexagonoSeisColores(PointF centro, double radio) : base(centro)
        {
            Radio = radio;
            ColoresTriangulos = new Color[] {
                Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Purple
            };
            GenerarVertices();
        }

        public override void GenerarVertices()
        {
            verticesBase.Clear();
            for (int k = 0; k < 6; k++)
            {
                double angulo = k * (Math.PI / 3.0);
                double x = Centro.X + Radio * Math.Cos(angulo);
                double y = Centro.Y + Radio * Math.Sin(angulo);
                verticesBase.Add(new PointF((float)x, (float)y));
            }
        }

        public override void Dibujar(Graphics g)
        {
            GenerarVertices();
            var transformados = TransformarVertices(verticesBase);

            for (int i = 0; i < 6; i++)
            {
                PointF p1 = transformados[i];
                PointF p2 = transformados[(i + 1) % 6];
                PointF[] triangulo = new PointF[] { Centro, p1, p2 };

                using (SolidBrush brush = new SolidBrush(ColoresTriangulos[i]))
                using (Pen pen = new Pen(Color.Black, 2))
                {
                    g.FillPolygon(brush, triangulo);
                    g.DrawPolygon(pen, triangulo);
                }
            }
        }

        public override double GetArea() => GeometryMath.CalcularArea(TransformarVertices(verticesBase));
        public override double GetPerimetro() => GeometryMath.CalcularPerimetro(TransformarVertices(verticesBase));
        public override bool ContienePunto(PointF p) => GeometryMath.PuntoEnPoligono(p, TransformarVertices(verticesBase));
    }
}