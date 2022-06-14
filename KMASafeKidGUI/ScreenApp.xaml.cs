﻿using System;
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
using IniParser;
using IniParser.Model;
namespace KMASafeGUI
{
    /// <summary>
    /// Interaction logic for MainApp.xaml
    /// </summary>
    public partial class ScreenApp : Window
    {
        private static string CONFIG_NAME = "config.ini";
        private static string PATH_CONFIG_FILE = ".\\" + CONFIG_NAME;// System.IO.Directory.GetCurrentDirectory() + "\\" + CONFIG_NAME;

        private DataView dataView;// = new DataView();
        private List<AppDiaryModel> diarys = new List<AppDiaryModel>();
        private static SQLiteConnection sQLiteCon = new SQLiteConnection();
        private FileIniDataParser iniFile = new FileIniDataParser();
        public ScreenApp()
        {
            InitializeComponent();
            dataView = new DataView();
            //diarys.Add(new AppDiaryModel() { AppName = "asas", TimeStart = "John Doe", TimeUsed = "" });
            //diarys.Add(new AppDiaryModel() { AppName = "asas", TimeStart = "Jane Doe", TimeUsed = "" });

            IniData data = iniFile.ReadFile(PATH_CONFIG_FILE);
            Toggle_Adult.IsChecked = data["WebConfig"]["BlockAdult"].ToString() == "1" ? true : false;
            Toggle_Social.IsChecked = data["WebConfig"]["BlockSocial"].ToString() == "1" ? true : false;
            Toggle_Inprivate.IsChecked = data["WebConfig"]["Inprivate"].ToString() == "1" ? true : false;
            Toggle_Game.IsChecked = data["AppConfig"]["BlockGame"].ToString() == "1" ? true : false;
            Toggle_SafeSearch.IsChecked = data["WebConfig"]["Safe"].ToString() == "1" ? true : false;

            sQLiteCon.ConnectionString = "Data Source = " + @".\BlockDB.sqlite";
            sQLiteCon.Open();
            SQLiteCommand command = new SQLiteCommand(sQLiteCon);
            command.CommandText = string.Format("SELECT * FROM tb_DiaryApp ");
            SQLiteDataReader DataReader = command.ExecuteReader();
            DateTime dStart = new DateTime();
            TimeSpan timeSpanUsed = new TimeSpan();
            while (DataReader.Read())
            {
                dStart = DateTime.FromFileTime((long)DataReader["TimeStart"]);
                timeSpanUsed = TimeSpan.FromSeconds((long)DataReader["TimeUsed"]);
                diarys.Add(new AppDiaryModel() { AppName = DataReader["AppName"].ToString(), TimeStart = dStart.ToString(), TimeUsed = timeSpanUsed.ToString()+"s"});
            }
            dataView.ViewAppDiary = diarys;
            DataContext = dataView;
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
            IniData data = iniFile.ReadFile(PATH_CONFIG_FILE);
            data["WebConfig"]["BlockAdult"] = "1";
            iniFile.WriteFile(PATH_CONFIG_FILE, data);
        }
        private void Adult_Setting_Unchecked(object sender, RoutedEventArgs e)
        {
            IniData data = iniFile.ReadFile(PATH_CONFIG_FILE);
            data["WebConfig"]["BlockAdult"] = "0";
            iniFile.WriteFile(PATH_CONFIG_FILE, data);
        }
        // Social----------------------------------------------------
        private void Social_Setting_Checked(object sender, RoutedEventArgs e)
        {
            IniData data = iniFile.ReadFile(PATH_CONFIG_FILE);
            data["WebConfig"]["BlockSocial"] = "1";
            iniFile.WriteFile(PATH_CONFIG_FILE, data);
        }
        private void Social_Setting_Unchecked(object sender, RoutedEventArgs e)
        {
            IniData data = iniFile.ReadFile(PATH_CONFIG_FILE);
            data["WebConfig"]["BlockSocial"] = "0";
            iniFile.WriteFile(PATH_CONFIG_FILE, data);
        }
        // Game-------------------------------------------------------
        private void Game_Setting_Checked(object sender, RoutedEventArgs e)
        {
            IniData data = iniFile.ReadFile(PATH_CONFIG_FILE);
            data["AppConfig"]["BlockGame"] = "1";
            iniFile.WriteFile(PATH_CONFIG_FILE, data);
        }
        private void Game_Setting_Unchecked(object sender, RoutedEventArgs e)
        {
            IniData data = iniFile.ReadFile(PATH_CONFIG_FILE);
            data["AppConfig"]["BlockGame"] = "0";
            iniFile.WriteFile(PATH_CONFIG_FILE, data);
        }
        //Safe Search-------------------------------------------------------
        private void SafeSearch_Setting_Checked(object sender, RoutedEventArgs e)
        {
            IniData data = iniFile.ReadFile(PATH_CONFIG_FILE);
            data["WebConfig"]["Safe"] = "1";
            iniFile.WriteFile(PATH_CONFIG_FILE, data);
        }
        private void SafeSearch_Setting_Unchecked(object sender, RoutedEventArgs e)
        {
            IniData data = iniFile.ReadFile(PATH_CONFIG_FILE);
            data["WebConfig"]["Safe"] = "0";
            iniFile.WriteFile(PATH_CONFIG_FILE, data);
        }
        //Video ---------------------------------------------------------------------
        private void Video_Setting_Checked(object sender, RoutedEventArgs e)
        {
            IniData data = iniFile.ReadFile(PATH_CONFIG_FILE);
            data["WebConfig"]["BlockAdult"] = "1";
            iniFile.WriteFile(PATH_CONFIG_FILE, data);
        }
        private void Video_Setting_Unchecked(object sender, RoutedEventArgs e)
        {
            IniData data = iniFile.ReadFile(PATH_CONFIG_FILE);
            data["WebConfig"]["BlockAdult"] = "0";
            iniFile.WriteFile(PATH_CONFIG_FILE, data);
        }
        //Button------------------------------------------------------------------
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
