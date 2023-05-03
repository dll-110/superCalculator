using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace superCalculator
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 form = new Form1();
            GlobalKeyboardHook.KeyDown += form.OnKeyDown;
            GlobalKeyboardHook.Start();            
            Application.Run(form);
            GlobalKeyboardHook.Stop();

        }
        
    }
}
