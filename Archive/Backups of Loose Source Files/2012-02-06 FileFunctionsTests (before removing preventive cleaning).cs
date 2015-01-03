//
//  Circle.Framework.Storage.Files.UnitTests.FileFunctionsTests
//
//      Author: Jan-Joost van Zon
//      Date: 2012-01-21 - 2012-02-05
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Framework.Testing;
using System.IO;

namespace Circle.Framework.Storage.Files.UnitTests
{
    [TestClass]
    public class FileFunctionsTests
    {
        [TestMethod]
        public void Test_FileFunctions_ClearFolder()
        {
            string tempFolderPath = TempFolder + "\\" + UniqueName;
            string file1Path = TempFolder + "\\" + UniqueName + "\\" + UniqueName + " (1).txt";
            string file2Path = TempFolder + "\\" + UniqueName + "\\" + UniqueName + " (2).txt";
            try
            {
                Directory.CreateDirectory(tempFolderPath);
                File.Create(file1Path);
                File.Create(file2Path);

                DirectoryInfo directory = new DirectoryInfo(tempFolderPath);
                Assert.AreEqual(2, directory.GetFiles().Length, "temp file count");
                FileFunctions.ClearFolder(tempFolderPath);
                Assert.AreEqual(0, directory.GetFiles().Length, "temp file count");
            }
            finally
            {
                if (File.Exists(file1Path)) File.Delete(file1Path);
                if (File.Exists(file2Path)) File.Delete(file2Path);
                if (Directory.Exists(tempFolderPath)) Directory.Delete(tempFolderPath);
            }
        }

        [TestMethod]
        public void Test_FileFunctions_EnsureFolder()
        {
            string tempFolderPath = TempFolder + "\\" + UniqueName;
            string tempFolderPath2 = TempFolder + "\\" + UniqueName + "\\" + UniqueName;

            try
            {
                if (Directory.Exists(tempFolderPath2)) Directory.Delete(tempFolderPath2);
                if (Directory.Exists(tempFolderPath)) Directory.Delete(tempFolderPath);
                Assert.IsFalse(Directory.Exists(tempFolderPath));

                FileFunctions.EnsureFolder(tempFolderPath2);
                Assert.IsTrue(Directory.Exists(tempFolderPath2));
            }
            finally
            {
                if (Directory.Exists(tempFolderPath2)) Directory.Delete(tempFolderPath2);
                if (Directory.Exists(tempFolderPath)) Directory.Delete(tempFolderPath);
            }
        }

        [TestMethod]
        public void Test_FileFunctions_EnsureFolder_LastSubFolderLooksLikeFile()
        {
            string tempFolderPath = TempFolder + "\\" + UniqueName;
            string tempFolderPath2 = TempFolder + "\\" + UniqueName + "\\" + UniqueName + ".txt";

            try
            {
                if (Directory.Exists(tempFolderPath2)) Directory.Delete(tempFolderPath2);
                if (Directory.Exists(tempFolderPath)) Directory.Delete(tempFolderPath);
                Assert.IsFalse(Directory.Exists(tempFolderPath));

                FileFunctions.EnsureFolder(tempFolderPath2);
                Assert.IsTrue(Directory.Exists(tempFolderPath2));
            }
            finally
            {
                if (Directory.Exists(tempFolderPath2)) Directory.Delete(tempFolderPath2);
                if (Directory.Exists(tempFolderPath)) Directory.Delete(tempFolderPath);
            }
        }

        [TestMethod]
        public void Test_FileFunctions_ApplicationPath()
        {
            string exeName = typeof(FileFunctionsTests).Assembly.ManifestModule.Name;
            Assert.IsTrue(File.Exists(FileFunctions.ApplicationPath + "\\" + exeName));
        }

        [TestMethod]
        public void Test_FileFunctions_IsFolder_True_Existing()
        {
            string path = TempFolder + "\\" + UniqueName;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            Assert.IsTrue(FileFunctions.IsFolder(path));
        }

        [TestMethod]
        public void Test_FileFunctions_IsFolder_True_NonExistent()
        {
            string path = TempFolder + "\\" + UniqueName;
            if (Directory.Exists(path)) Directory.Delete(path);
            Assert.IsTrue(FileFunctions.IsFolder(path));
        }

        [TestMethod]
        public void Test_FileFunctions_IsFolder_True_LooksLikeFile_Existent()
        {
            string path = TempFolder + "\\" + UniqueName + ".txt";
            try
            {
                if (File.Exists(path)) File.Delete(path);

                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                Assert.IsTrue(FileFunctions.IsFolder(path));
            }
            finally
            {
                if (Directory.Exists(path)) Directory.Delete(path);
            }
        }

        [TestMethod]
        public void Test_FileFunctions_IsFolder_False_LooksLikeFile_NonExistent()
        {
            string path = TempFolder + "\\" + UniqueName + ".txt";
            if (File.Exists(path)) File.Delete(path);

            if (Directory.Exists(path)) Directory.Delete(path);

            Assert.IsFalse(FileFunctions.IsFolder(path));
        }

        [TestMethod]
        public void Test_FileFunctions_IsFile_True_Existing()
        {
            string path = TempFolder + "\\" + UniqueName + ".txt";
            try
            {
                if (!File.Exists(path)) File.Create(path).Close();
                Assert.IsTrue(FileFunctions.IsFile(path));
            }
            finally
            {
                if (File.Exists(path)) File.Delete(path);
            }
        }

        [TestMethod]
        public void Test_FileFunctions_IsFile_True_NonExistent()
        {
            string path = TempFolder + "\\" + UniqueName + ".txt";
            if (File.Exists(path)) File.Delete(path);
            Assert.IsTrue(FileFunctions.IsFile(path));
        }

        [TestMethod]
        public void Test_FileFunctions_IsFile_True_LooksLikeFolder_Existing()
        {
            string path = TempFolder + "\\" + UniqueName;
            if (Directory.Exists(path)) Directory.Delete(path);
            File.Create(path).Close();
            Assert.IsTrue(FileFunctions.IsFile(path));
        }

        [TestMethod]
        public void Test_FileFunctions_IsFile_False_LooksLikeFolder_NonExistent()
        {
            string path = TempFolder + "\\" + UniqueName;
            if (Directory.Exists(path)) Directory.Delete(path);
            Assert.IsFalse(FileFunctions.IsFile(path));
        }

        [TestMethod]
        public void Test_FileFunctions_GetFolderSize()
        {
            string path = TempFolder + "\\" + UniqueName;
            if (Directory.Exists(path)) 
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Test_FileFunctions_HideFile_ShowFile()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Test_FileFunctions_MakeReadOnly_MakeWritable()
        {
            throw new NotImplementedException();
        }

        // Helpers

        private string UniqueName;
        private string TempFolder;

        public FileFunctionsTests()
        {
            UniqueName = Guid.NewGuid().ToString();
            TempFolder = String.Format(@"C:\temp\{0}", UniqueName);
            FileFunctions.EnsureFolder(@"C:\temp");
            Directory.CreateDirectory(TempFolder);
        }

        ~FileFunctionsTests()
        {
            if (Directory.Exists(TempFolder)) Directory.Delete(TempFolder);
        }

        private void DeleteFileOrFolder(string path)
        {
            if (File.Exists(path)) File.Delete(path);
            if (Directory.Exists(path)) Directory.Delete(path);
        }
    }
}
