using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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

namespace WPFTask6._1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FileInfo[] files;
        bool canEdit = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();

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

            try
            {
                files = search.GetFiles(pattern, recursive);

                results.ItemsSource = from FileInfo file
                                      in files
                                      select file.FullName;
            }
            catch (UnauthorizedAccessException access)
            {
                MessageBox.Show(access.Message);
            }
        }

        private void ListViewItem_Preview(object sender, MouseButtonEventArgs e)
        {
            canEdit = false;

            if (results.SelectedItem != null)
            {
                string path = (string)results.SelectedItem;

                if (File.Exists(path))
                {
                    try
                    {
                        editor.Text = File.ReadAllText(path);
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message);
                    }
                }
            }

            canEdit = true;
        }

        private void Editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (canEdit)
            {
                if (results.SelectedItem != null)
                {
                    string path = (string)results.SelectedItem;

                    if (File.Exists(path))
                    {
                        try
                        {
                            File.WriteAllText(path, editor.Text);
                        }
                        catch (Exception exc)
                        {
                            MessageBox.Show(exc.Message);
                        }
                    }
                }
            }
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            if (results.SelectedItem != null)
            {
                string path = (string)results.SelectedItem;
                string output = path + ".zip";

                if (File.Exists(path) && !File.Exists(output))
                {
                    try
                    {
                        using (FileStream fs = File.OpenWrite(output))
                        {
                            using (ZipArchive writer = new ZipArchive(fs, ZipArchiveMode.Create, true))
                            {
                                writer.CreateEntryFromFile(path, Path.GetFileName(path));
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        if (File.Exists(output))
                        {
                            File.Delete(output);
                        }

                        MessageBox.Show(exc.Message);
                    }
                }
            }
        }
    }
}
