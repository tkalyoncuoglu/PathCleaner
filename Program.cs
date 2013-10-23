﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PathCleaner
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            checkElevation();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private static bool isElevated()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private static void checkElevation()
        {
            if(isElevated())
            {
                return;
            }
            var startInfo = new ProcessStartInfo(Process.GetCurrentProcess().Modules[0].FileName)
            {
                Verb = "runas",
            };
            Process.Start(startInfo);
            Environment.Exit(0);
        }
    }
}
