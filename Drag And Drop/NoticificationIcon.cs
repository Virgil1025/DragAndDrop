﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;

namespace Drag_And_Drop
{
    public partial class App : Application
    {

        System.Windows.Forms.NotifyIcon nIcon = new System.Windows.Forms.NotifyIcon();

        public App()
        {
            nIcon.Icon = Drag_And_Drop.Properties.Resources.icon1;
            nIcon.Visible = true;
            nIcon.ShowBalloonTip(5000, "Title", "Text", System.Windows.Forms.ToolTipIcon.Info);
            nIcon.Click += nIcon_Click;
        }

        void nIcon_Click(object sender, EventArgs e)
        {
            MainWindow.Visibility = Visibility.Visible;
            MainWindow.WindowState = WindowState.Normal;
            Window mainWindow = Application.Current.MainWindow;
            mainWindow.Activate();
        }

    }

}
