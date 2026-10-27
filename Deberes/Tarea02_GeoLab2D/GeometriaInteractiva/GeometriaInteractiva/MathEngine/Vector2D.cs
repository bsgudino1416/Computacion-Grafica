using System;
using System.Drawing;

namespace GeometriaInteractiva.MathEngine
{
    public struct Vector2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector2D(PointF desde, PointF hasta)
        {
            X = hasta.X - desde.X;
            Y = hasta.Y - desde.Y;
        }

        // 1. Magnitud (Norma del vector)
        public double Magnitud() => Math.Sqrt(X * X + Y * Y);

        // 2. Dirección en grados [0, 360)
        public double DireccionGrados()
        {
            double angulo = Math.Atan2(Y, X) * (180.0 / Math.PI);
            return angulo < 0 ? angulo + 360.0 : angulo;
        }

        // 3. Producto Punto
        public static double ProductoPunto(Vector2D u, Vector2D v) => u.X * v.X + u.Y * v.Y;

        // 4. Determinante (Producto Cruz 2D)
        public static double Determinante(Vector2D u, Vector2D v) => u.X * v.Y - u.Y * v.X;

        public PointF APointF() => new PointF((float)X, (float)Y);
    }
}