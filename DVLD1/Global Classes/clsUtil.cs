using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD1.Global_Classes
{
    public class clsUtil
    {
        public static string GenerateGUID()
        {

            Guid guid = new Guid();

            return guid.ToString();

        }

        public static bool CreateFolderIfDoesNotExist(string FolderPath)
        {

            if (!Directory.Exists(FolderPath))
            {
                try
                {
                    Directory.CreateDirectory(FolderPath);
                }
                catch
                {
                    return false;
                }
            }

            return false;


        }

        public static string ReplaceFileNameWithGUID(string sourceFile)
        {

            string fileName = sourceFile;
            FileInfo fi = new FileInfo(fileName);
            string extn = fi.Extension;
            return GenerateGUID() + extn;

        }

        public static bool CopyImageToProjectImagesFolder(ref string sourceFile)
        {
            string DestinationFolader = @"c:\DVDL1-Peple_Images\";
            //if (!CreateFolderIfDoesNotExist(DestinationFolader))
            //{
            //    return false;
            //}
            string DestinationFileName = DestinationFolader + ReplaceFileNameWithGUID(sourceFile);
            try
            {
                File.Copy(sourceFile, DestinationFileName, true);
            }
            catch (IOException iox)
            {
                MessageBox.Show(iox.Message, "Error", MessageBoxButtons.OK);
                return false;
            }
            sourceFile = DestinationFileName;
            return true;
        }
    }
}
