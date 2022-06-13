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
using System.Windows.Shapes;

namespace KMASafeKidGUI
{
    /// <summary>
    /// Interaction logic for AddBlockWindow.xaml
    /// </summary>
    public partial class AddBlockWindow : Window
    {
        public AddBlockWindow()
        {
            InitializeComponent();
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
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
                addAppTextBox.Text = openFileDlg.FileName;
                //addAppTextBox.Text = System.IO.File.ReadAllText(openFileDlg.FileName);
            }
        }
    }
}
