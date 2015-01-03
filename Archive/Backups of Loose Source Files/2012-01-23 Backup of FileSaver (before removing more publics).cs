//
//  Circle.Framework.Storage.Files.FileSaver
//
//      Author: Jan-Joost van Zon
//      Date: 2012-01-21 - 2012-01-21
//
//  -----

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Circle.Framework.Code.Conditions;
using Circle.Framework.Data.Text;

namespace Circle.Framework.Storage.Files
{
    /// <summary>
    /// This class allows a safe file overwrite,
    /// by first writing to a temporary file and when all
    /// went well, the original file is overwritten.
    /// </summary>
    public class FileSaver : IDisposable
    {
        // Construction, Destruction

        public FileSaver(string originalFilePath)
        {
            InitializeOriginalFilePath(originalFilePath);
            OpenTempFile();
        }

        ~FileSaver()
        {
            Dispose();
        }

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            if (!IsDisposed) return;
            IsDisposed = true;

            if (TempFileIsOpen)
            {
                CloseTempFile();
            }
        }

        // OriginalFile

        public string OriginalFilePath { get; private set; }

        private void InitializeOriginalFilePath(string originalFilePath)
        {
            Condition.NotNullOrEmpty(originalFilePath, "originalFilePath");
            Condition.FileExists(originalFilePath, "originalFilePath");

            OriginalFilePath = originalFilePath;

            OriginalFileInfo = new FileInfo(OriginalFilePath);
        }

        private FileInfo OriginalFileInfo;

        // Temp File

        public string TempFilePath { get; private set; }
        public bool TempFileIsOpen { get; private set; }

        private Stream OpenTempFile()
        {
            if (TempFileIsOpen)
            {
                throw new Exception("Temp file already open.");
            }
                
            TempFilePath = GetTempFilePath();
            File.Create(TempFilePath).Close();
            TempFileInfo = new FileInfo(TempFilePath);

            DuplicateAttributes();
            SetHiddenAttribute();

            TempFileStream = new FileStream(TempFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            TempFileIsOpen = true;

            return TempFileStream;
        }

        private FileInfo TempFileInfo;
        private FileStream TempFileStream;

        public void CloseTempFile()
        {
            if (!TempFileIsOpen)
            {
                throw new Exception("Temp file is not open.");
            }

            TempFileStream.Close();
            TempFileIsOpen = false;
            TempFilePath = null;

            RestoreOriginalHiddenAttribute();
        }

        public void DeleteOriginalFile()
        {
            File.Delete(OriginalFilePath);
        }

        public void RestoreOriginalFile()
        {
            if (TempFileIsOpen)
            {
                CloseTempFile();
            }

            File.Delete(TempFilePath);
        }

        // Random File Path

        private string GetTempFilePath()
        {
            Condition.NotNullOrEmpty(OriginalFilePath, "baseFilePath");

            string folderPath = Path.GetDirectoryName(OriginalFilePath).CutRight(@"\"); // Remove slash from root (e.g. @"C:\")
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(OriginalFilePath);
            string fileExtension = Path.GetExtension(OriginalFilePath);

            string tempFilePath;
            do
            {
                tempFilePath = folderPath + @"\" + "~" + GetRandom12DigitHex() + fileNameWithoutExtension + fileExtension;
            }
            while (File.Exists(tempFilePath));
            return tempFilePath;
        }

        private static string GetRandom12DigitHex()
        {
            return Guid.NewGuid().ToString().Right(12);
        }

        // Attributes

        private void DuplicateAttributes()
        {
            TempFileInfo.Attributes = OriginalFileInfo.Attributes;
            TempFileInfo.CreationTime  = OriginalFileInfo.CreationTime;
        }

        private void SetHiddenAttribute()
        {
            OriginalHiddenAttribute = TempFileInfo.Attributes & FileAttributes.Hidden;
            TempFileInfo.Attributes = TempFileInfo.Attributes | FileAttributes.Hidden;
        }

        private FileAttributes OriginalHiddenAttribute;

        private void RestoreOriginalHiddenAttribute()
        {
            // Turn off the 'hidden' bit (with an and-not mask).
            // Optionally turn it on again (with an or-operation).
            TempFileInfo.Attributes = TempFileInfo.Attributes & ~FileAttributes.Hidden | OriginalHiddenAttribute;
        }
    }
}
