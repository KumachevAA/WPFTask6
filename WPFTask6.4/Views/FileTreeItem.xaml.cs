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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFTask6._4.Models;

namespace WPFTask6._4.Views
{
    /// <summary>
    /// Логика взаимодействия для FileTreeItem.xaml
    /// </summary>
    public partial class FileTreeItem : UserControl
    {
        public static readonly DependencyProperty ChildrenProperty = DependencyProperty.Register(
            nameof(Children),
            typeof(IEnumerable<FileSystemModel>),
            typeof(FileTreeItem),
            new PropertyMetadata(Array.Empty<FileSystemModel>())
            );

        public static readonly DependencyProperty ItemNameProperty = DependencyProperty.Register(
            nameof(ItemName),
            typeof(string),
            typeof(FileTreeItem),
            new PropertyMetadata(string.Empty)
            );

        public IEnumerable<FileSystemModel> Children
        {
            get => (IEnumerable<FileSystemModel>)GetValue(ChildrenProperty);
            set => SetValue(ChildrenProperty, value);
        }

        public string ItemName
        {
            get => (string)GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }

        public FileTreeItem()
        {
            InitializeComponent();
        }
    }
}
