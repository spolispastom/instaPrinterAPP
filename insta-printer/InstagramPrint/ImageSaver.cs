using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstagramPrint
{
    class ImageSaver
    {
        public ImageSaver()
        {
            SavedDirectoryDialog = new FolderBrowserDialog();
        }

        private FolderBrowserDialog SavedDirectoryDialog;
        private DirectoryInfo savedDirectory = null;
        public bool IsSetSavedDirectory
        { get { return savedDirectory != null; } }
        public bool SetDirectoryOfString(string directoryName)
        {
            savedDirectory = Directory.CreateDirectory(directoryName);
            return IsSetSavedDirectory;
        }

        public bool SetDirectory(string path)
        {
            DirectoryInfo dir = Directory.CreateDirectory(path);
            if (dir != null)
            {
                savedDirectory = dir;
                return true;
            }
            else return false;
        }

        public bool SetDirectoryShowDialog()
        {
            if (SavedDirectoryDialog.ShowDialog()  == DialogResult.OK)
            {
                savedDirectory = Directory.CreateDirectory(SavedDirectoryDialog.SelectedPath);
            }
            return IsSetSavedDirectory;
        }

        public void SaveImage(Bitmap image, string fileName)
        {
            if (IsSetSavedDirectory)
            {
                string name = fileName.Replace('*', ' ').Replace('|', ' ').Replace('\\', ' ').Replace('/', ' ').Replace(':', ' ').Replace('"', ' ').Replace('\'', ' ').Replace('>', ' ').Replace('<', ' ').Replace('?', ' ');
                string fillFileName = string.Format("{0}\\{1}.jpeg", savedDirectory.FullName, name);
                image.Save(fillFileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }
    }
}
