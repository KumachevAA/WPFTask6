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
using System.Windows.Shapes;
using WPFTask6._4.Models;

namespace WPFTask6._4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FileSystemModel[] RootDrives
        {
            get
            {
                DriveInfo[] infos = DriveInfo.GetDrives();
                FileSystemModel[] roots = new FileSystemModel[infos.Length];

                for (int i = 0; i < infos.Length; i++)
                {
                    roots[i] = new FileSystemModel(infos[i].Name, EntryType.Drive);
                }

                return roots;
            }
        }

        private Dictionary<TreeViewItem, FileSystemModel> ItemEntries { get; } = new Dictionary<TreeViewItem, FileSystemModel>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Explorer_Expanded(object sender, RoutedEventArgs e)
        {
            if (e.Source is TreeViewItem item)
            {
                foreach (TreeViewItem child in item.Items)
                {
                    FillItem(child);
                }
            }
        }

        private void FillItem(TreeViewItem item)
        {
            FileSystemModel fsEntry = ItemEntries[item];

            if (item.Items.Count == 0)
            {
                for (int i = 0; i < fsEntry.Children.Length; i++)
                {
                    TreeViewItem child = new TreeViewItem()
                    {
                        Header = fsEntry.Children[i].Name
                    };

                    item.Items.Add(child);
                    ItemEntries.Add(child, fsEntry.Children[i]);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < RootDrives.Length; i++)
            {
                TreeViewItem item = new TreeViewItem
                {
                    Header = RootDrives[i].FullName
                };

                ItemEntries.Add(item, RootDrives[i]);

                FillItem(item);
                Explorer.Items.Add(item);
            }
        }

        void SetData(TreeViewItem item)
        {
            FileSystemModel selected = ItemEntries[item];
            FileSystemInfo info;

            switch (selected.EntryType)
            {
                case EntryType.Directory:
                    info = new DirectoryInfo(selected.FullName);
                    break;

                case EntryType.Drive:
                    info = new DirectoryInfo(selected.FullName);
                    break;

                case EntryType.File:
                    info = new FileInfo(selected.FullName);
                    break;

                default:
                    info = null;
                    break;
            }

            if (info == null)
                return;

            List<string> attributes = new List<string>();

            if (info.Attributes.HasFlag(FileAttributes.ReadOnly))
                attributes.Add("Только чтение");

            if (info.Attributes.HasFlag(FileAttributes.Hidden))
                attributes.Add("Скрытый");

            if (info.Attributes.HasFlag(FileAttributes.System))
                attributes.Add("Системный");

            if (info.Attributes.HasFlag(FileAttributes.Archive))
                attributes.Add("Сжатый");

            string creation = info.CreationTime.ToString();
            string access = info.LastAccessTime.ToString();
            string write = info.LastWriteTime.ToString();

            dataBox.Text = $"Атрибуты: {string.Join(", ", attributes.ToArray())}" + Environment.NewLine +
                $"Время создания: {creation}" + Environment.NewLine +
                $"Последнее чтение: {access}" + Environment.NewLine +
                $"Последняя запись: {write}" + Environment.NewLine +
                $"Полное имя: {selected.FullName}" + Environment.NewLine +
                $"Имя: {selected.Name}";
        }

        private void Explorer_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is TreeViewItem item)
            {
                SetData(item);
            }
        }
    }
}
