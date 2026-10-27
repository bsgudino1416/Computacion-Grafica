using System;
using System.Collections.Generic;
using System.Drawing;
using GeometriaInteractiva.MathEngine;

namespace GeometriaInteractiva.Models
{
    public class PoligonoEstrellado : FiguraGeometrica
    {
        public int Puntas { get; set; }
        public double RadioExterior { get; set; }
        public double RadioInterior { get; set; }
        public Color ColorRelleno { get; set; } = Color.Gold;

        private List<PointF> verticesBase = new List<PointF>();

        public PoligonoEstrellado(PointF centro, int puntas, double rExt, double rInt) : base(centro)
        {
            Puntas = puntas;
            RadioExterior = rExt;
            RadioInterior = rInt;
            GenerarVertices();
        }

        public override void GenerarVertices()
        {
            verticesBase.Clear();
            int totalVertices = Puntas * 2;
            double pasoAngular = Math.PI / Puntas;

            for (int k = 0; k < totalVertices; k++)
            {
                double r = (k % 2 == 0) ? RadioExterior : RadioInterior;
                double theta = k * pasoAngular;
                double x = Centro.X + r * Math.Cos(theta);
                double y = Centro.Y + r * Math.Sin(theta);
                verticesBase.Add(new PointF((float)x, (float)y));
            }
        }

        public override void Dibujar(Graphics g)
        {
            GenerarVertices();
            var transformados = TransformarVertices(verticesBase);

            using (SolidBrush brush = new SolidBrush(ColorRelleno))
            using (Pen pen = new Pen(Color.OrangeRed, 2))
            {
                g.FillPolygon(brush, transformados.ToArray());
                g.DrawPolygon(pen, transformados.ToArray());
            }
        }

        public override double GetArea() => GeometryMath.CalcularArea(TransformarVertices(verticesBase));
        public override double GetPerimetro() => GeometryMath.CalcularPerimetro(TransformarVertices(verticesBase));
        public override bool ContienePunto(PointF p) => GeometryMath.PuntoEnPoligono(p, TransformarVertices(verticesBase));

        public List<PointF> GetVerticesTransformados() => TransformarVertices(verticesBase);
    }
}