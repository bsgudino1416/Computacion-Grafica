using System;
using System.Collections.Generic;
using System.Drawing;

namespace GeometriaInteractiva.MathEngine
{
    public static class GeometryMath
    {
        // Prueba de Vuelta / Orientación: >0 Antihorario, <0 Horario, ==0 Colineal
        public static double Orientacion(PointF p1, PointF p2, PointF p3)
        {
            return (p2.X - p1.X) * (p3.Y - p1.Y) - (p2.Y - p1.Y) * (p3.X - p1.X);
        }

        // Área mediante Fórmula de Gauss / Shoelace
        public static double CalcularArea(List<PointF> vertices)
        {
            int n = vertices.Count;
            if (n < 3) return 0;

            double area = 0;
            for (int i = 0; i < n; i++)
            {
                PointF actual = vertices[i];
                PointF siguiente = vertices[(i + 1) % n];
                area += (actual.X * siguiente.Y) - (siguiente.X * actual.Y);
            }
            return Math.Abs(area) / 2.0;
        }

        // Perímetro suma de distancias
        public static double CalcularPerimetro(List<PointF> vertices)
        {
            int n = vertices.Count;
            if (n < 2) return 0;

            double perimetro = 0;
            for (int i = 0; i < n; i++)
            {
                Vector2D v = new Vector2D(vertices[i], vertices[(i + 1) % n]);
                perimetro += v.Magnitud();
            }
            return perimetro;
        }

        // Pertenencia a Polígono mediante Algoritmo Ray-Casting
        public static bool PuntoEnPoligono(PointF p, List<PointF> poligono)
        {
            bool dentro = false;
            int n = poligono.Count;

            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                if (((poligono[i].Y > p.Y) != (poligono[j].Y > p.Y)) &&
                    (p.X < (poligono[j].X - poligono[i].X) * (p.Y - poligono[i].Y) / (poligono[j].Y - poligono[i].Y) + poligono[i].X))
                {
                    dentro = !dentro;
                }
            }
            return dentro;
        }

        // Proyección de Punto sobre Segmento AB
        public static PointF ProyectarPuntoEnSegmento(PointF p, PointF a, PointF b)
        {
            Vector2D v = new Vector2D(a, b);
            Vector2D w = new Vector2D(a, p);

            double mag2 = v.X * v.X + v.Y * v.Y;
            if (mag2 == 0) return a;

            double t = Vector2D.ProductoPunto(w, v) / mag2;
            t = Math.Max(0, Math.Min(1, t)); // Clampear t a [0, 1]

            return new PointF((float)(a.X + t * v.X), (float)(a.Y + t * v.Y));
        }
    }
}