using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using InterviewCodeReview;
using InterviewCodeReview.Interfaces;
using System.Collections.Generic;

namespace IntervieCodeReviewTest
{
    [TestClass]
    public class FileFactoryTests
    {
        private string testText = "This a the file text test 12345 &.";
        
        private string GetFileName()
        {
            Random rand = new Random(DateTime.Now.GetHashCode());
            string fileName = "test" + rand.Next(5000).ToString() + ".txt";

            return fileName;
        }

        private void PrepareEmptyFile(string fileName)
        {
            File.WriteAllText(fileName, String.Empty);
        }

        private void PrepareDirtyFile(string fileName)
        {
            File.WriteAllText(fileName, String.Empty);
            File.WriteAllLines(fileName, new string[] { "Text", "In this file" });
        }

        private void DeleteFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void AppendFileFactoryCanCreateNewFile()
        {
            string fileName = GetFileName();
            string fileText = String.Empty;

            DeleteFile(fileName);

            AppendFileFactory fileFactory = new AppendFileFactory(fileName);

            using (StreamWriter f = fileFactory.GetOuputFile())
            {
                f.WriteLine(string.Empty);
            }

            Assert.IsTrue(File.Exists(fileName));
        }

        [TestMethod]
        public void AppendFileFactoryCanOpenFileMultipleTimes()
        {
            string fileName = GetFileName();
            string actual = String.Empty;
            string expected = String.Empty;

            PrepareEmptyFile(fileName);

            AppendFileFactory fileFactory = new AppendFileFactory(fileName);

            using (StreamWriter f = fileFactory.GetOuputFile())
            {
                f.WriteLine(testText);
            }

            using (StreamWriter f = fileFactory.GetOuputFile())
            {
                f.WriteLine(testText);
            }

            expected = testText + Environment.NewLine + testText + Environment.NewLine;
            actual = File.ReadAllText(fileName);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AppendFileFactoryCanAppendTextToFile()
        {
            string fileName = GetFileName();
            string actual = String.Empty;
            string fileContent = String.Empty;
            string expected = String.Empty;

            PrepareDirtyFile(fileName);

            fileContent = File.ReadAllText(fileName);

            AppendFileFactory fileFactory = new AppendFileFactory(fileName);

            using (StreamWriter f = fileFactory.GetOuputFile())
            {
                f.WriteLine(testText);
            }
            
            expected = fileContent + testText + Environment.NewLine;
            actual = File.ReadAllText(fileName);

            Assert.AreEqual(expected, actual);
        }
    }
}
