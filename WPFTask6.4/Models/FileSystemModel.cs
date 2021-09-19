using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFTask6._4.Models
{
    public enum EntryType
    {
        Directory,
        File,
        Drive
    }

    public class FileSystemModel
    {
        public EntryType EntryType { get; }

        public string FullName { get; }

        public string Name
        {
            get
            {
                if (EntryType == EntryType.Directory)
                {
                    return Path.GetFileName(FullName);
                }
                else if (EntryType == EntryType.File)
                {
                    return Path.GetFileName(FullName);
                }
                else if (EntryType == EntryType.Drive)
                {
                    return FullName;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private FileSystemModel[] children;

        public FileSystemModel[] Children
        {
            get
            {
                if (EntryType == EntryType.Directory)
                {
                    if (children == null)
                    {
                        try
                        {
                            DirectoryInfo dir = new DirectoryInfo(FullName);
                            FileSystemInfo[] infos = dir.GetFileSystemInfos();

                            children = new FileSystemModel[infos.Length];

                            for (int i = 0; i < infos.Length; i++)
                            {
                                EntryType type = infos[i] is DirectoryInfo ? EntryType.Directory : EntryType.File;
                                children[i] = new FileSystemModel(infos[i].FullName, type);
                            }
                        }
                        catch (Exception)
                        {
                            children = Array.Empty<FileSystemModel>();
                        }
                    }
                }
                else if (EntryType == EntryType.Drive)
                {
                    try
                    {
                        DirectoryInfo dir = new DirectoryInfo(FullName);
                        FileSystemInfo[] infos = dir.GetFileSystemInfos();

                        children = new FileSystemModel[infos.Length];

                        for (int i = 0; i < infos.Length; i++)
                        {
                            EntryType type = infos[i] is DirectoryInfo ? EntryType.Directory : EntryType.File;
                            children[i] = new FileSystemModel(infos[i].FullName, type);
                        }
                    }
                    catch (Exception)
                    {
                        children = Array.Empty<FileSystemModel>();
                    }
                }
                else
                {
                    children = Array.Empty<FileSystemModel>();
                }

                return children;
            }
        }

        public FileSystemModel(string fullName, EntryType entryType)
        {
            FullName = fullName;
            EntryType = entryType;
        }
    }
}
