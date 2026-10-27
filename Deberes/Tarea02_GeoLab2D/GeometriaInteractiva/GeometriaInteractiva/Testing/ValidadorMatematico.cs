using System;
using System.Collections.Generic;
using System.Drawing;
using GeometriaInteractiva.MathEngine;
using GeometriaInteractiva.Models;

namespace GeometriaInteractiva.Testing
{
    public static class ValidadorMatematico
    {
        public static string EjecutarCasosDePrueba()
        {
            string reporte = "=== CASOS DE PRUEBA Y VALIDACIÓN MATEMÁTICA ===\r\n\r\n";

            // 1. Prueba de Producto Punto (Ortogonalidad)
            Vector2D u = new Vector2D(10, 0);
            Vector2D v = new Vector2D(0, 10);
            double dot = Vector2D.ProductoPunto(u, v);
            reporte += $"[1] Producto Punto Perpendicular (10,0) . (0,10): {dot} | Expected: 0 -> {(dot == 0 ? "PASÓ" : "FALLÓ")}\r\n";

            // 2. Área de Hexágono Teórica vs Shoelace
            double r = 100;
            HexagonoSeisColores hex = new HexagonoSeisColores(new PointF(0, 0), r);
            double areaAnalitica = (3.0 * Math.Sqrt(3) / 2.0) * Math.Pow(r, 2);
            double areaShoelace = hex.GetArea();
            double error = Math.Abs(areaAnalitica - areaShoelace);
            reporte += $"[2] Área Hexágono (Teórica: {areaAnalitica:F2}, Shoelace: {areaShoelace:F2}) -> {(error < 0.01 ? "PASÓ" : "FALLÓ")}\r\n";

            // 3. Prueba de Invarianza por Rotación (Área no debe cambiar al rotar)
            double areaOriginal = hex.GetArea();
            hex.AnguloRotacion = 45;
            double areaRotada = hex.GetArea();
            double diffRot = Math.Abs(areaOriginal - areaRotada);
            reporte += $"[3] Invarianza de Área tras Rotación (Diff: {diffRot:F4}) -> {(diffRot < 0.01 ? "PASÓ" : "FALLÓ")}\r\n";

            // 4. Prueba de Punto en Polígono (Ray Casting)
            bool centroDentro = hex.ContienePunto(new PointF(0, 0));
            bool puntoFuera = hex.ContienePunto(new PointF(500, 500));
            reporte += $"[4] Ray-Casting (Centro dentro: {centroDentro}, Fuera: {puntoFuera}) -> {(centroDentro && !puntoFuera ? "PASÓ" : "FALLÓ")}\r\n";

            return reporte;
        }
    }
}