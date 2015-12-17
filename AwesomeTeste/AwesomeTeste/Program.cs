using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Awesomium.Core;

namespace AwesomeTeste
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Checks if this is a child rendering process and if so,
            // transfers control of the process to Awesomium.
            if (WebCore.IsChildProcess)
            {
                WebCore.ChildProcessMain();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += OnApplicationExit;
            Application.Run(new Form1());
        }

        private static void OnApplicationExit(object sender, EventArgs e)
        {
            // Make sure we shutdown the core last.
            if (WebCore.IsInitialized)
                WebCore.Shutdown();
        }
    }
}
