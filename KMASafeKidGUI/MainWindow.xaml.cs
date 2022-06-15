using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
namespace KMASafeGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private static string CONFIG_NAME = "config.ini";
        private static string PATH_CONFIG_FILE = ".\\" + CONFIG_NAME;// System.IO.Directory.GetCurrentDirectory() + "\\" + CONFIG_NAME;

        public MainWindow()
        {
            InitializeComponent();
            InputLogin.Focus();
            textCreatePass.Visibility = Visibility.Hidden;
            var iniFile = new FileIniDataParser();
            IniData dataINI = iniFile.ReadFile(PATH_CONFIG_FILE);
            if (dataINI["User"]["pwd"] == null || dataINI["User"]["pwd"].ToString() == "")
            {
                textCreatePass.Visibility = Visibility.Visible;
            }
        }

        private void ButtonLogin(object sender, RoutedEventArgs e)
        {
            var iniFile = new FileIniDataParser();
            IniData dataINI = iniFile.ReadFile(PATH_CONFIG_FILE);
            if (dataINI["User"]["pwd"] == null || dataINI["User"]["pwd"] == "")
            {
                dataINI["User"]["pwd"] = InputLogin.Password;
                iniFile.WriteFile(PATH_CONFIG_FILE, dataINI);
            }
            if (dataINI["User"]["pwd"] == InputLogin.Password)
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
        private void ClickClose(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Enter_Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var iniFile = new FileIniDataParser();
                IniData dataINI = iniFile.ReadFile(PATH_CONFIG_FILE);
                if (dataINI["User"]["pwd"] == null || dataINI["User"]["pwd"] == "")
                {
                    dataINI["User"]["pwd"] = InputLogin.Password;
                    iniFile.WriteFile(PATH_CONFIG_FILE, dataINI);
                }
                if (dataINI["User"]["pwd"] == InputLogin.Password)
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
        }

    }
}
