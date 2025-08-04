using System;
using System.Windows.Forms;

namespace EthicalScannerFormsApp
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // This line should reference your main form's class name
            // (e.g., Form1 or EthicalScannerForm)
            Application.Run(new Form1());
        }
    }
}