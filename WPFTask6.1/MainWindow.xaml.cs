using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFTask6._1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = dialog.SelectedPath;
                folderBox.Text = path;
            }
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(folderBox.Text))
                return;

            DirectoryInfo search = new DirectoryInfo(folderBox.Text);
            string pattern = searchBox.Text;
            SearchOption recursive = recursiveSearch.IsChecked == true ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            FileInfo[] files = search.GetFiles(pattern, recursive);
            results.Text = string.Empty;

            for (int i = 0; i < files.Length; i++)
            {
                results.Text += files[i].FullName + Environment.NewLine;
            }
        }
    }
}
