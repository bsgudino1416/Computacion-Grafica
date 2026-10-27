using System;
using System.Drawing;

namespace GeometriaInteractiva.MathEngine
{
    public class Matrix3x3
    {
        private readonly double[,] m = new double[3, 3];

        public Matrix3x3()
        {
            SetIdentity();
        }

        public void SetIdentity()
        {
            m[0, 0] = 1; m[0, 1] = 0; m[0, 2] = 0;
            m[1, 0] = 0; m[1, 1] = 1; m[1, 2] = 0;
            m[2, 0] = 0; m[2, 1] = 0; m[2, 2] = 1;
        }

        public static Matrix3x3 CrearTraslacion(double tx, double ty)
        {
            var mat = new Matrix3x3();
            mat.m[2, 0] = tx;
            mat.m[2, 1] = ty;
            return mat;
        }

        public static Matrix3x3 CrearRotacion(double anguloGrados)
        {
            double rad = anguloGrados * (Math.PI / 180.0);
            double cos = Math.Cos(rad);
            double sen = Math.Sin(rad);

            var mat = new Matrix3x3();
            mat.m[0, 0] = cos; mat.m[0, 1] = sen;
            mat.m[1, 0] = -sen; mat.m[1, 1] = cos;
            return mat;
        }

        public static Matrix3x3 CrearEscalamiento(double sx, double sy)
        {
            var mat = new Matrix3x3();
            mat.m[0, 0] = sx;
            mat.m[1, 1] = sy;
            return mat;
        }

        public static Matrix3x3 Multiplicar(Matrix3x3 a, Matrix3x3 b)
        {
            var res = new Matrix3x3();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    res.m[i, j] = a.m[i, 0] * b.m[0, j] +
                                  a.m[i, 1] * b.m[1, j] +
                                  a.m[i, 2] * b.m[2, j];
                }
            }
            return res;
        }

        public PointF TransformarPunto(PointF p)
        {
            double x = p.X * m[0, 0] + p.Y * m[1, 0] + m[2, 0];
            double y = p.X * m[0, 1] + p.Y * m[1, 1] + m[2, 1];
            return new PointF((float)x, (float)y);
        }
    }
}