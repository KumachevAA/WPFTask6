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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace WPFTask6._3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DirectoryInfo currentDirectory;
        FileSystemInfo[] entries;

        void UpdateFileSystemEntries(string path)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo temp = currentDirectory;

                try
                {
                    currentDirectory = new DirectoryInfo(path);
                    entries = currentDirectory.EnumerateFileSystemInfos().ToArray();

                    FileExplorer.Items.Clear();

                    FileExplorer.Items.Add("..");
                    for (int i = 0; i < entries.Length; i++)
                    {
                        FileExplorer.Items.Add(entries[i].Name);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show($"Ошибка при чтении содержимого по пути {currentDirectory.FullName}");
                    currentDirectory = temp;
                }
            }

            PathInput.Text = currentDirectory.FullName;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateFileSystemEntries("C:/");
        }

        private void ListViewItem_Preview(object sender, MouseButtonEventArgs e)
        {
            int selected = FileExplorer.SelectedIndex;

            if (selected == 0)
            {
                UpdateFileSystemEntries(currentDirectory.Parent.FullName);
            }
            if (selected > 0)
            {
                FileSystemInfo entry = entries[selected - 1];

                if (entry.Exists)
                {
                    if (entry is DirectoryInfo dir)
                    {
                        UpdateFileSystemEntries(dir.FullName);
                    }
                    else if (entry is FileInfo file)
                    {
                        MessageBox.Show("File selected");
                    }
                }
            }
        }
    }
}
