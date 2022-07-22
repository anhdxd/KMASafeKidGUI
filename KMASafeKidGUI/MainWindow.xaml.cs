using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IniParser;
using IniParser.Model;
using Microsoft.Win32;
namespace KMASafeGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private static string CONFIG_NAME = "config.ini";
        private static string PATH_CONFIG_FILE = ".\\" + CONFIG_NAME;// System.IO.Directory.GetCurrentDirectory() + "\\" + CONFIG_NAME;
        public static EventWaitHandle signalWaitResult = new EventWaitHandle(false, EventResetMode.AutoReset);
        private static string PassworDecrypt = "";
        private static bool bFlagNewPassword = false;
        public MainWindow()
        {
            InitializeComponent();


            InputLogin.Focus();
            textCreatePass.Visibility = Visibility.Hidden;
            PipeClient.OninitPipes();
            // chưa có pass thì close, mở lại lấy quyền admin để chạy
            if (Registry.LocalMachine.OpenSubKey("SOFTWARE\\KMASafe") == null)
            {
                bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
                if (!isAdmin)
                {
                    //PipeClient.FlagSend = (int)PipeClient.fText.OpenGUIAdmin;
                    //PipeClient.signal.Set();
                    //signalWaitResult.WaitOne(1000);
                    PipeClient.SendRequestOpenGUIWithAdmin();
                    Close();
                    Environment.Exit(0);
                }
                else
                {
                    textCreatePass.Visibility = Visibility.Visible;
                    bFlagNewPassword = true;
                }
            }
        }

        private void ButtonLogin(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bFlagNewPassword)
                {
                    if (InputLogin.Password == "")
                    {
                        textCreatePass.Text = "Mật khẩu không được trống !";
                        textCreatePass.Visibility = Visibility.Visible;
                        return;
                    }

                    string PasswordEncrypt = AES.EncryptBase64ToString(InputLogin.Password);
                    Registry.LocalMachine.CreateSubKey("SOFTWARE\\KMASafe").SetValue("pwd", PasswordEncrypt);
                    bFlagNewPassword = false;

                    textCreatePass.Text = "Oke, Nhập mật khẩu vừa tạo !";
                    textCreatePass.Foreground = Brushes.Red;
                    textCreatePass.Visibility = Visibility.Visible;

                    return;
                }
                else
                {
                    string passMH = Registry.LocalMachine.OpenSubKey("SOFTWARE\\KMASafe").GetValue("pwd").ToString();
                    PassworDecrypt = AES.DecryptBase64ToString(passMH);
                    bFlagNewPassword = false;
                }

                if (PassworDecrypt == InputLogin.Password)
                {
                    ScreenApp screenApp = new ScreenApp();
                    screenApp.Show();
                    Close();
                }
                else
                {
                    textCreatePass.Text = "Mật khẩu sai, nhập lại mật khẩu !";
                    textCreatePass.Foreground = Brushes.Red;
                    textCreatePass.Visibility = Visibility.Visible;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi không xác định");
            }
        }
        private void ClickClose(object sender, RoutedEventArgs e)
        {
            Close();
            Environment.Exit(0);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var current_process = Process.GetCurrentProcess();
            var other_process = Process.GetProcessesByName(current_process.ProcessName).FirstOrDefault(p => p.Id != current_process.Id);
            if (other_process != null && other_process.MainWindowHandle != IntPtr.Zero)
            {
                current_process.Close();
                Environment.Exit(0);
                return;
            }
        }
    }
}
