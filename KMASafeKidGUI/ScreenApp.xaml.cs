using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using KMASafeGUI.Model;
using System.Data.SQLite;
using System.Timers;

namespace KMASafeGUI
{
    /// <summary>
    /// Interaction logic for MainApp.xaml
    /// </summary>
    public partial class ScreenApp : Window
    {
        private DataView dataView;// = new DataView();
        private List<AppDiaryModel> diarys = new List<AppDiaryModel>();
        private static SQLiteConnection sQLiteCon = new SQLiteConnection();
        public ScreenApp()
        {
            InitializeComponent();
            dataView = new DataView();
            //diarys.Add(new AppDiaryModel() { AppName = "asas", TimeStart = "John Doe", TimeUsed = "" });
            //diarys.Add(new AppDiaryModel() { AppName = "asas", TimeStart = "Jane Doe", TimeUsed = "" });


            sQLiteCon.ConnectionString = "Data Source = " + @".\BlockDB.sqlite";
            sQLiteCon.Open();
            SQLiteCommand command = new SQLiteCommand(sQLiteCon);
            command.CommandText = string.Format("SELECT * FROM tb_DiaryApp ");
            SQLiteDataReader DataReader = command.ExecuteReader();
            while (DataReader.Read())
            {
                diarys.Add(new AppDiaryModel() { AppName = DataReader["AppName"].ToString(), TimeStart = DataReader["TimeStart"].ToString(), TimeUsed = DataReader["TimeUsed"].ToString() });
            }
            dataView.ViewAppDiary = diarys;
            DataContext = dataView;
        }

        private void AutoupdateDiary(object source, ElapsedEventArgs e)
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



            //DataGridDiary.ItemsSource = diarys;

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

        private void Btn_InstallApp(object sender, RoutedEventArgs e)
        {
            InstallAppWindow installAppWindow = new InstallAppWindow { Owner = this };
            installAppWindow.ShowDialog();
        }

        private void Btn_AddBlock(object sender, RoutedEventArgs e)
        {
            AddBlockWindow addBlockWindow = new AddBlockWindow { Owner = this };
            addBlockWindow.ShowDialog();
        }
        private void ClickClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
