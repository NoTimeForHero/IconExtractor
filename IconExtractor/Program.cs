using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IconExtractor
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*
            String path = @"C:\Users\artyo\OneDrive\Документы\Visual Studio 2017\Projects\IconExtractor\IconExtractor\bin\Debug\Test1Icon.exe";
            IconApi api = new IconApi(path);
            var icgrp = api.getIconGroups();
            Icon icon = api.getIcon(icgrp[0].icon_groups[0].ID);
            */

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }

}
