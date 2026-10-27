using System.Collections.Generic;
using System.Drawing;
using GeometriaInteractiva.MathEngine;

namespace GeometriaInteractiva.Models
{
    public abstract class FiguraGeometrica
    {
        public PointF Centro { get; set; }
        public double AnguloRotacion { get; set; } = 0;
        public double Escala { get; set; } = 1.0;

        protected FiguraGeometrica(PointF centro)
        {
            Centro = centro;
        }

        public abstract void GenerarVertices();
        public abstract void Dibujar(Graphics g);
        public abstract double GetArea();
        public abstract double GetPerimetro();
        public abstract bool ContienePunto(PointF p);

        // Aplica transformaciones compuestas alrededor del centro
        protected List<PointF> TransformarVertices(List<PointF> verticesOriginales)
        {
            Matrix3x3 t1 = Matrix3x3.CrearTraslacion(-Centro.X, -Centro.Y);
            Matrix3x3 s = Matrix3x3.CrearEscalamiento(Escala, Escala);
            Matrix3x3 r = Matrix3x3.CrearRotacion(AnguloRotacion);
            Matrix3x3 t2 = Matrix3x3.CrearTraslacion(Centro.X, Centro.Y);

            Matrix3x3 compuesta = Matrix3x3.Multiplicar(t1, s);
            compuesta = Matrix3x3.Multiplicar(compuesta, r);
            compuesta = Matrix3x3.Multiplicar(compuesta, t2);

            List<PointF> transformados = new List<PointF>();
            foreach (var v in verticesOriginales)
            {
                transformados.Add(compuesta.TransformarPunto(v));
            }
            return transformados;
        }
    }
}