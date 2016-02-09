using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InterviewCodeReview;
using InterviewCodeReview.Interfaces;
using System.IO;
using IntervieCodeReviewTest.Stubs;
using InterviewCodeReview.Messages;
using InterviewCodeReview.Loggers;

namespace IntervieCodeReviewTest
{
    [TestClass]
    public class TexFileLoggerTests
    {

        [TestMethod]
        public void TextFileLoggerCanLog()
        {
            string msgText = "text file logger test";
            string expected = String.Empty;
            string actual = String.Empty;

            Message msg = new GeneralMessage(msgText);
            StubFileFactory fileFactory = new StubFileFactory("dummy.txt");
            TextFileLogger txtlogger = new TextFileLogger(fileFactory);

            using (MemoryStream ms = fileFactory.MemoryStream)
            {
                txtlogger.Log(msg);

                ms.Position = 0;
                using (StreamReader reader = new StreamReader(ms, Encoding.UTF8))
                {
                    actual = reader.ReadToEnd();
                }
            }

            expected = DateTime.Now.ToShortDateString() + " " + msgText + Environment.NewLine;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TextFileCanBeCreated()
        {
            IFileFactory fileFactory = new StubFileFactory("dummy.txt");

            TextFileLogger txtLogger = new TextFileLogger(fileFactory);

            Assert.IsNotNull(txtLogger);
            Assert.IsInstanceOfType(txtLogger, typeof(ILogger));
        }

    }
}
