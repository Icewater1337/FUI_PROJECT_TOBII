using EyeXFramework.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeleniumApproach
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static FormsEyeXHost _eyeXHost = new FormsEyeXHost();

        public static FormsEyeXHost EyeXHost
        {
            get { return _eyeXHost; }
        }
        [STAThread]
        static void Main()
        {
            _eyeXHost.Start();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 form = new Form1();
            Application.Run(form);
            _eyeXHost.Dispose();
        }

    }
}
