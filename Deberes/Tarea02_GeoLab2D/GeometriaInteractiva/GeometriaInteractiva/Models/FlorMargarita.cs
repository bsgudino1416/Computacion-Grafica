using System;
using System.Collections.Generic;
using System.Drawing;
using GeometriaInteractiva.MathEngine;

namespace GeometriaInteractiva.Models
{
    public class FlorMargarita : FiguraGeometrica
    {
        public double DistanciaCentro { get; set; }
        public double RadioPentagono { get; set; }
        public Color ColorPetalos { get; set; } = Color.Pink;

        public FlorMargarita(PointF centro, double distanciaCentro, double radioPentagono) : base(centro)
        {
            DistanciaCentro = distanciaCentro;
            RadioPentagono = radioPentagono;
            GenerarVertices();
        }

        public override void GenerarVertices() { }

        private List<List<PointF>> ObtenerPentagonosBase()
        {
            List<List<PointF>> listaPentagonos = new List<List<PointF>>();

            for (int i = 0; i < 5; i++)
            {
                double alpha = i * (2.0 * Math.PI / 5.0);
                double cx = Centro.X + DistanciaCentro * Math.Cos(alpha);
                double cy = Centro.Y + DistanciaCentro * Math.Sin(alpha);

                List<PointF> pentagono = new List<PointF>();
                for (int j = 0; j < 5; j++)
                {
                    double theta = alpha + j * (2.0 * Math.PI / 5.0);
                    double x = cx + RadioPentagono * Math.Cos(theta);
                    double y = cy + RadioPentagono * Math.Sin(theta);
                    pentagono.Add(new PointF((float)x, (float)y));
                }
                listaPentagonos.Add(pentagono);
            }
            return listaPentagonos;
        }

        public override void Dibujar(Graphics g)
        {
            var pentagonos = ObtenerPentagonosBase();

            using (SolidBrush brush = new SolidBrush(ColorPetalos))
            using (Pen pen = new Pen(Color.DarkMagenta, 2))
            {
                foreach (var p in pentagonos)
                {
                    var trans = TransformarVertices(p);
                    g.FillPolygon(brush, trans.ToArray());
                    g.DrawPolygon(pen, trans.ToArray());
                }
            }
        }

        public override double GetArea()
        {
            double areaTotal = 0;
            foreach (var p in ObtenerPentagonosBase())
            {
                areaTotal += GeometryMath.CalcularArea(TransformarVertices(p));
            }
            return areaTotal;
        }

        public override double GetPerimetro()
        {
            double perimetroTotal = 0;
            foreach (var p in ObtenerPentagonosBase())
            {
                perimetroTotal += GeometryMath.CalcularPerimetro(TransformarVertices(p));
            }
            return perimetroTotal;
        }

        public override bool ContienePunto(PointF p)
        {
            foreach (var pentagono in ObtenerPentagonosBase())
            {
                if (GeometryMath.PuntoEnPoligono(p, TransformarVertices(pentagono)))
                    return true;
            }
            return false;
        }
    }
}