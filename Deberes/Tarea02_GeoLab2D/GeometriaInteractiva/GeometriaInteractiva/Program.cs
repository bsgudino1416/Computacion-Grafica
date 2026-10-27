using System;
using System.Windows.Forms;

namespace GeometriaInteractiva
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Punto de arranque configurado hacia nuestra UI personalizada
            Application.Run(new UI.MainForm());
        }
    }
}