using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
using System.Windows.Shapes;
using Newtonsoft.Json.Linq;
namespace KMASafeGUI
{
    /// <summary>
    /// Interaction logic for AddBlockWindow.xaml
    /// </summary>
    public partial class AddBlockWindow : Window
    {
        public static EventWaitHandle signalWaitResult = new EventWaitHandle(false, EventResetMode.AutoReset);
        private List<DataBlock> WebBlock = new List<DataBlock>();
        private List<DataBlock> AppBlock = new List<DataBlock>();
        private List<DataBlock> PathApp = new List<DataBlock>();
        private ModifySQlite SQlite = new ModifySQlite();
        public AddBlockWindow()
        {
            InitializeComponent();
            tbInputBlockWeb.Focus();
            SQlite.ConnectToDB(ModifySQlite.PATH_DB);
            SortedSet<string> ssUserApp = SQlite.CGetAllBlockRule(ModifySQlite.ObjectType.APP);//AES.DecryptFileToSortedSet(".\\DBB\\UADB.dat");
            SortedSet<string> ssUserHost = SQlite.CGetAllBlockRule(ModifySQlite.ObjectType.WEB);//AES.DecryptFileToSortedSet(".\\DBB\\UDB.dat");
            // Chuy
            foreach (var item in ssUserHost)
            {
                WebBlock.Add(new DataBlock { WebName = item});
            }
            foreach (var item in ssUserApp)
            {
                AppBlock.Add(new DataBlock { AppName = item });
            }

            dgBlockShow.ItemsSource = WebBlock;
            dgAppShow.ItemsSource = AppBlock;
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point pos = e.GetPosition(this);
            if (pos.X < Width && pos.Y < 30)
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    DragMove();
                }
            }
        }

        private void btn_OpenFolder_click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            openFileDlg.Filter = "Execute (*.exe)|*.exe";
            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDlg.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (result == true)
            {
                tbInputAppBlock.Text = openFileDlg.FileName;
                //addAppTextBox.Text = System.IO.File.ReadAllText(openFileDlg.FileName);
            }
        }

        private void btn_AddWeb_Click(object sender, RoutedEventArgs e)
        {
            string InputText = tbInputBlockWeb.Text;
            if (Uri.IsWellFormedUriString(InputText, UriKind.Absolute))
                InputText = new Uri(InputText).Host;

            if (InputText.StartsWith("www."))
                InputText = InputText.Substring("www.".Length);

            JObject jSendToSV = new JObject();
            jSendToSV["flag"] = (int)PipeClient.fText.AddHostToDB;
            jSendToSV["sDomain"] = InputText;
            PipeClient.SendRequestToServer(jSendToSV.ToString());

            System.Threading.Thread.Sleep(50);
            SortedSet<string> ssUserHost = SQlite.CGetAllBlockRule(ModifySQlite.ObjectType.WEB);//AES.DecryptFileToSortedSet(".\\DBB\\UDB.dat");
            WebBlock.Clear();
            foreach (var item in ssUserHost)
            {
                WebBlock.Add(new DataBlock { WebName = item });
            }
            dgBlockShow.ItemsSource = WebBlock;
            dgBlockShow.Items.Refresh();

        }

        private void btn_AddApp_Click(object sender, RoutedEventArgs e)
        {
            JObject jSendToSV = new JObject();
            jSendToSV["flag"] = (int)PipeClient.fText.AddAppToDB;
            jSendToSV["sPath"] = tbInputAppBlock.Text;

            PipeClient.SendRequestToServer(jSendToSV.ToString());

            System.Threading.Thread.Sleep(50);
            SortedSet<string> ssUserApp = SQlite.CGetAllBlockRule(ModifySQlite.ObjectType.APP);//AES.DecryptFileToSortedSet(".\\DBB\\UADB.dat");
            AppBlock.Clear();
            foreach (var item in ssUserApp)
            {
                AppBlock.Add(new DataBlock { AppName = item });
            }
            dgAppShow.ItemsSource = AppBlock;
            dgAppShow.Items.Refresh();
        }

        private void DeleteWebClick(object sender, RoutedEventArgs e)
        {
            DataBlock db = dgBlockShow.CurrentItem as DataBlock;

            JObject jSendToSV = new JObject();
            jSendToSV["flag"] = (int)PipeClient.fText.DeleteHostDB;
            jSendToSV["sDomain"] = db.WebName;
            PipeClient.SendRequestToServer(jSendToSV.ToString());

            System.Threading.Thread.Sleep(50);
            SortedSet<string> ssUserHost = SQlite.CGetAllBlockRule(ModifySQlite.ObjectType.WEB);//AES.DecryptFileToSortedSet(".\\DBB\\UDB.dat");
            WebBlock.Clear();
            foreach (var item in ssUserHost)
            {
                WebBlock.Add(new DataBlock { WebName = item });
            }
            dgBlockShow.ItemsSource = WebBlock;
            dgBlockShow.Items.Refresh();
        }

        private void DeleteAppClick(object sender, RoutedEventArgs e)
        {
            DataBlock db = dgAppShow.CurrentItem as DataBlock;

            JObject jSendToSV = new JObject();
            jSendToSV["flag"] = (int)PipeClient.fText.DeleteAppDB;
            jSendToSV["sPath"] = db.AppName;
            PipeClient.SendRequestToServer(jSendToSV.ToString());

            System.Threading.Thread.Sleep(50);
            SortedSet<string> ssUserApp = SQlite.CGetAllBlockRule(ModifySQlite.ObjectType.APP);//AES.DecryptFileToSortedSet(".\\DBB\\UADB.dat");
            AppBlock.Clear();
            foreach (var item in ssUserApp)
            {
                AppBlock.Add(new DataBlock { AppName = item });
            }
            dgAppShow.ItemsSource = AppBlock;
            dgAppShow.Items.Refresh();
        }
    }
    public class DataBlock
    {
        public string WebName { get; set; }
        public string AppName { get; set; }
        public string PathApp { get; set; }
    }

}
