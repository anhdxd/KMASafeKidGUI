using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KMASafeKidGUI
{
    /// <summary>
    /// Interaction logic for MainApp.xaml
    /// </summary>
    public partial class ScreenApp : Window
    {
        //private readonly view viewModel;
        public ScreenApp()
        {
            InitializeComponent();
            List<Diary> diarys = new List<Diary>();
            //diarys.Add(new Diary() { appName = "", timeStart = "John Doe", timeEnd = "" });
            //diarys.Add(new Diary() { appName = "", timeStart = "Jane Doe", timeEnd = "" });
            //diarys.Add(new Diary() { appName = "", timeStart = "Sammy Doe", timeEnd ="" });

            //DataDiary.ItemsSource = diarys;
        }
        private void ClickClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Dataweb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point pos = e.GetPosition(this);
            if (pos.X < Width && pos.Y <30)
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    DragMove();
                }
            }

        }

        private void Adult_Setting_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void Social_Setting_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Game_Setting_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void SafeSearch_Setting_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Video_Setting_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void btn_InstallApp(object sender, RoutedEventArgs e)
        {
            InstallAppWindow installAppWindow = new InstallAppWindow { Owner = this };
            installAppWindow.ShowDialog();
        }

        private void btn_AddBlock(object sender, RoutedEventArgs e)
        {
            AddBlockWindow addBlockWindow = new AddBlockWindow { Owner = this };
            addBlockWindow.ShowDialog();
        }
    }
    public class Diary
    {
        public string AppName { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
    }

}
