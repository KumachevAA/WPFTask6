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
        private DirectoryInfo currentDirectory;
        private FileSystemInfo[] entries;
        private FileInfo openedFile;

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
                        openedFile = null;
                        textEditor.Text = string.Empty;

                        try
                        {
                            textEditor.Text = File.ReadAllText(file.FullName);
                            openedFile = file;
                        }
                        catch (Exception)
                        {
                            MessageBox.Show($"Не удалось открыть файл {file.FullName}");
                        }
                    }
                }
            }
        }

        private void TextEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (openedFile != null)
            {
                if (openedFile.Exists)
                {
                    try
                    {
                        File.WriteAllText(openedFile.FullName, textEditor.Text);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Ошибка записи в файл. Возможно, у вас отсутствует право на запись");
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateFileSystemEntries(PathInput.Text);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string path = Path.Combine(currentDirectory.FullName, FileNameInput.Text);

            if (!File.Exists(path))
            {
                StreamWriter str = null;
                try
                {
                    // создание нового файла
                    str = File.CreateText(path);
                    UpdateFileSystemEntries(currentDirectory.FullName);
                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось создать файл");
                }
                finally
                {
                    str?.Dispose();
                }
            }
        }
    }
}
