using System;
using System.Collections.Generic;
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
        public AddBlockWindow()
        {
            InitializeComponent();
            tbInputBlockWeb.Focus();
            SortedSet<string> ssUserApp =  AES.DecryptFileToSortedSet(".\\DBB\\UADB.dat");
            SortedSet<string> ssUserHost =  AES.DecryptFileToSortedSet(".\\DBB\\UDB.dat");
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
            PipeClient.FlagSend = (int)PipeClient.fText.AddHostToDB;
            PipeClient.StringSend = tbInputBlockWeb.Text;
            PipeClient.signal.Set();
            signalWaitResult.WaitOne(700);
            if (PipeClient.bResult)
            {
                SortedSet<string> ssUserApp = AES.DecryptFileToSortedSet(".\\DBB\\UADB.dat");
                SortedSet<string> ssUserHost = AES.DecryptFileToSortedSet(".\\DBB\\UDB.dat");
                // Chuy
                WebBlock.Clear();
                foreach (var item in ssUserHost)
                {
                    WebBlock.Add(new DataBlock { WebName = item });
                }
                foreach (var item in ssUserApp)
                {
                    WebBlock.Add(new DataBlock { WebName = item });
                }
                dgBlockShow.ItemsSource = WebBlock;
                //appBlock.Add(new DataBlock { Name = tbInputBlockWeb.Text });
                dgBlockShow.Items.Refresh();
                labelResult.Content = "Thêm thành công !";
                labelResult.Visibility = Visibility.Visible;
            }
            else
            {
                //labelResult.Content = "Thêm thất bại !";
                MessageBox.Show("Thêm thất bại !");
                labelResult.Visibility = Visibility.Hidden;
            }    
        }

        private void btn_AddApp_Click(object sender, RoutedEventArgs e)
        {
            PipeClient.FlagSend = (int)PipeClient.fText.AddAppToDB;
            PipeClient.StringSend = tbInputAppBlock.Text;
            PipeClient.signal.Set();

            signalWaitResult.WaitOne(700);
            if (PipeClient.bResult)
            {
                SortedSet<string> ssUserApp = AES.DecryptFileToSortedSet(".\\DBB\\UADB.dat");
                SortedSet<string> ssUserHost = AES.DecryptFileToSortedSet(".\\DBB\\UDB.dat");
                // Chuy
                WebBlock.Clear();
                foreach (var item in ssUserHost)
                {
                    WebBlock.Add(new DataBlock { WebName = item });
                }
                foreach (var item in ssUserApp)
                {
                    WebBlock.Add(new DataBlock { WebName = item });
                }
                dgBlockShow.ItemsSource = WebBlock;
                //appBlock.Add(new DataBlock { Name = tbInputBlockWeb.Text });
                dgBlockShow.Items.Refresh();
                labelResult.Content = "Thêm thành công !";
                labelResult.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Thêm thất bại !");
                labelResult.Visibility = Visibility.Hidden;
            }
        }
        
    }
    public class DataBlock
    {
        public string WebName { get; set; }
        public string AppName { get; set; }
    }

}
