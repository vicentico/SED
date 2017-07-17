using System;
using System.Windows.Forms;

namespace Template.Configurator
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmConfigurador());
        }
    }
}
