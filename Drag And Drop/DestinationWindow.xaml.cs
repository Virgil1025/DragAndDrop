using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Configuration;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Drag_And_Drop
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>

    public partial class DestinationWindow : Window
    {
        String LOCATION = ConfigurationManager.AppSettings["PAPERLESS_ENTRANCE"];
        String DOMAIN = ConfigurationManager.AppSettings["DOMAIN"];
        String ACCOUNT = ConfigurationManager.AppSettings["ACCOUNT"];
        String PASSWORD = ConfigurationManager.AppSettings["PASSWORD"];

        String Key2_Value = ConfigurationManager.AppSettings["HOTKEY2"];

        public DestinationWindow()
        {
            this.ShowInTaskbar = false;
            InitializeComponent();
            setUI();
        }

        private void setUI()
        {
            String appPath = System.Windows.Forms.Application.StartupPath;
            appPath = appPath + "\\Drag And Drop.exe";
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(appPath);
            this.Location_entry.Text = configuration.AppSettings.Settings["PAPERLESS_ENTRANCE"].Value;
            this.Domain_entry.Text = configuration.AppSettings.Settings["DOMAIN"].Value;
            this.Account_entry.Text = configuration.AppSettings.Settings["ACCOUNT"].Value;
            this.Password_entry.Password = configuration.AppSettings.Settings["PASSWORD"].Value;
            String currentSelection = configuration.AppSettings.Settings["HOTKEY1"].Value;
            this.Key2.Content = configuration.AppSettings.Settings["HOTKEY2"].Value;
            this.Search_Page_entry.Text = configuration.AppSettings.Settings["SEARCH_PAGE_URL"].Value;
            setComboBox(currentSelection);
        }

        private void setComboBox(String s)
        {
            this.HotKey1_ComboBox.Items.Add("ALT");
            this.HotKey1_ComboBox.Items.Add("CTRL");
            this.HotKey1_ComboBox.Items.Add("SHIFT");

            if (s == "ALT") this.HotKey1_ComboBox.SelectedIndex = 0;
            if (s == "CTRL") this.HotKey1_ComboBox.SelectedIndex = 1;
            if (s == "SHIFT") this.HotKey1_ComboBox.SelectedIndex = 2;

        }

        private void set_onClick(object sender, EventArgs e)
        {
            Window mainWindow = System.Windows.Application.Current.MainWindow;
            mainWindow.IsEnabled = true;
            //mainWindow.Close();

            String appPath = System.Windows.Forms.Application.StartupPath;
            appPath = appPath + "\\Drag And Drop.exe";
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(appPath);
            configuration.AppSettings.Settings["PAPERLESS_ENTRANCE"].Value = this.Location_entry.Text;
            configuration.AppSettings.Settings["DOMAIN"].Value = this.Domain_entry.Text;
            configuration.AppSettings.Settings["ACCOUNT"].Value = this.Account_entry.Text;
            configuration.AppSettings.Settings["PASSWORD"].Value = this.Password_entry.Password;
            if (this.HotKey1_ComboBox.SelectedIndex == 0) configuration.AppSettings.Settings["HOTKEY1"].Value = "ALT";
            if (this.HotKey1_ComboBox.SelectedIndex == 1) configuration.AppSettings.Settings["HOTKEY1"].Value = "CTRL";
            if (this.HotKey1_ComboBox.SelectedIndex == 2) configuration.AppSettings.Settings["HOTKEY1"].Value = "SHIFT";
            configuration.AppSettings.Settings["HOTKEY2"].Value = this.Key2.Content.ToString(); /*string.Format("0x{0:X2}", KeyInterop.VirtualKeyFromKey((Key)this.Key2.Content));*/
            configuration.AppSettings.Settings["SEARCH_PAGE_URL"].Value = this.Search_Page_entry.Text;
            configuration.Save();

            MainWindow.RegisterHotKey();

            this.Close();
            //System.Windows.Forms.Application.Restart();

        }

        private void cancel_onClick(object sender, EventArgs e)
        {
            Window mainWindow = System.Windows.Application.Current.MainWindow;
            mainWindow.IsEnabled = true;
            this.Close();
        }

        private void textBox1_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (this.Key2.Content.Equals("<New Key>")) this.Key2.Content = e.Key.ToString();

            if (VKConverter.getUINT(this.Key2.Content.ToString()) == 0x00)
            {
                System.Windows.MessageBox.Show("This Key can't be used on hot key.");
                this.Key2.Content = Key2_Value;
            }



        }

        private void Key2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.Key2.Content.Equals("<New Key>")) this.Key2.Content = Key2_Value;

        }

        private void Key2_Click(object sender, RoutedEventArgs e)
        {
            Key2_Value = this.Key2.Content.ToString();
            this.Key2.Content = "<New Key>";
        }
    }
}
