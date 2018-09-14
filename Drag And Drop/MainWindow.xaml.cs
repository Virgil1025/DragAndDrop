using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Principal;
using System.Configuration;
using System.Security;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Drag_And_Drop
{
    public partial class MainWindow : Window
    {
        [DllImport("User32.dll")]
        private static extern bool RegisterHotKey(
    [In] IntPtr hWnd,
    [In] int id,
    [In] uint fsModifiers,
    [In] uint vk);

        [DllImport("User32.dll")]
        private static extern bool UnregisterHotKey(
            [In] IntPtr hWnd,
            [In] int id);

        private HwndSource _source;
        private const int HOTKEY_ID = 9000;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var helper = new WindowInteropHelper(this);
            _source = HwndSource.FromHwnd(helper.Handle);
            _source.AddHook(HwndHook);
            RegisterHotKey();
        }

        

        protected override void OnClosed(EventArgs e)
        {
            _source.RemoveHook(HwndHook);
            _source = null;
            UnregisterHotKey();
            base.OnClosed(e);
        }

        public static void RegisterHotKey()
        {
            String appPath = System.Windows.Forms.Application.StartupPath;
            appPath = appPath + "\\Drag And Drop.exe";
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(appPath);

            var helper = new WindowInteropHelper(Application.Current.MainWindow);
            uint KEY_1 = VKConverter.getUINT(configuration.AppSettings.Settings["HOTKEY1"].Value);
            uint KEY_2 = VKConverter.getUINT(configuration.AppSettings.Settings["HOTKEY2"].Value);
            if (!RegisterHotKey(helper.Handle, HOTKEY_ID, KEY_1, KEY_2))
            {
                // handle error
            }
        }

        private void UnregisterHotKey()
        {
            var helper = new WindowInteropHelper(this);
            UnregisterHotKey(helper.Handle, HOTKEY_ID);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID:
                            OnHotKeyPressed();
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        private void OnHotKeyPressed()
        {
            String appPath = System.Windows.Forms.Application.StartupPath;
            appPath = appPath + "\\Drag And Drop.exe";
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(appPath);
            String target = configuration.AppSettings.Settings["SEARCH_PAGE_URL"].Value;
            Process.Start(target);
        }

        public MainWindow()
        {
            this.ShowInTaskbar = false;
            this.Topmost = true;
            SizeChanged += (o, e) =>
            {
                var r = SystemParameters.WorkArea;
                Left = r.Right - ActualWidth;
                Top = r.Bottom - ActualHeight;
            };
            InitializeComponent();
            this.SourceInitialized += Window1_SourceInitialized;
            this.DataContext = this;

            var listbox = DropBox;
            ListBoxItem itm = new ListBoxItem();
            itm.Content = "Drag file to here.";
            listbox.Items.Add(itm);
        }



        private void DropBox_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
                var listbox = sender as ListBox;
                listbox.Background = new SolidColorBrush(Color.FromRgb(155, 155, 155));
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void DropBox_DragLeave(object sender, DragEventArgs e)
        {
            var listbox = sender as ListBox;
            listbox.Background = new SolidColorBrush(Color.FromRgb(226, 226, 226));
        }

        private void DropBox_Drop(object sender, DragEventArgs e)
        {
            var listbox = sender as ListBox;
            listbox.Background = new SolidColorBrush(Color.FromRgb(226, 226, 226));
            

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
           {
                listbox.Items.Clear();
                String appPath = System.Windows.Forms.Application.StartupPath;
                appPath = appPath + "\\Drag And Drop.exe";
                Configuration configuration = ConfigurationManager.OpenExeConfiguration(appPath);
                String LOCATION = configuration.AppSettings.Settings["PAPERLESS_ENTRANCE"].Value;
                String DOMAIN = configuration.AppSettings.Settings["DOMAIN"].Value;
                String ACCOUNT = configuration.AppSettings.Settings["ACCOUNT"].Value;
                String PASSWORD = configuration.AppSettings.Settings["PASSWORD"].Value;

                String[] files = (String[])e.Data.GetData(DataFormats.FileDrop);
                for (int i = 0; i < files.Length; i++)
                {
                    if (i == 0)
                    {
                        ListBoxItem itemFirstLine = new ListBoxItem();
                        if(files.Length == 1)
                        {
                            itemFirstLine.Content = "A file has been added into Paperless System : ";
                        }
                        else
                        {
                            itemFirstLine.Content = files.Length + " files have been added into Paperless System : ";
                        }
                        
                        listbox.Items.Add(itemFirstLine);
                        listbox.Items.Add("");
                    }

                    IntPtr userToken = IntPtr.Zero;
                    
                    bool success = ExternalHelper.LogonUser(
                      @ACCOUNT,
                      @DOMAIN,
                      @PASSWORD,
                      2,
                      1,
                      out userToken);

                    if (!success)
                    {
                        listbox.Items.Clear();
                        listbox.Items.Add("Drag file to here.");
                        MessageBox.Show("The access to '" + @LOCATION + "' is denied." + "\n" +
                                        "It may be caused by wrong user name, password or domain."
                            , "Access Denied");
                        return;
                    }
 
                    AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
                    WindowsIdentity idnt = new WindowsIdentity(userToken);
                    WindowsImpersonationContext context = idnt.Impersonate();
                    File.Copy(files[i], @LOCATION + "\\" + System.IO.Path.GetFileName(files[i]), true);
                    context.Undo();

                    ListBoxItem item = new ListBoxItem();
                    item.Content = i + 1 + ". " + System.IO.Path.GetFileName(files[i]);
                    listbox.Items.Add(item);
                }
            }
        }

        private void Window1_SourceInitialized(object sender, EventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            HwndSource source = HwndSource.FromHwnd(helper.Handle);
            source.AddHook(WndProc);
        }

        const int WM_SYSCOMMAND = 0x0112;
        const int SC_MOVE = 0xF010;

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {

            switch (msg)
            {
                case WM_SYSCOMMAND:
                    int command = wParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                    {
                        handled = true;
                    }
                    break;
                default:
                    break;
            }
            return IntPtr.Zero;
        }

        private void setting_onClick(object sender, EventArgs e)
        {
            this.IsEnabled = false;
            var destinationWindow = new DestinationWindow();
            destinationWindow.Show();
        }

        private void goSearchPage_onClick(object sender, RoutedEventArgs e)
        {
            String appPath = System.Windows.Forms.Application.StartupPath;
            appPath = appPath + "\\Drag And Drop.exe";
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(appPath);
            String target = configuration.AppSettings.Settings["SEARCH_PAGE_URL"].Value;
            Process.Start(target);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //e.Cancel = true;
            //this.Visibility = Visibility.Hidden;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
