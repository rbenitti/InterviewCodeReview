using InterviewCodeReview.Interfaces;
using System;
using System.IO;

namespace InterviewCodeReview.Loggers
{
    /// <summary>
    /// TextFileLogger writes messages to files. The files are determined dynamically by a file factory.
    /// </summary>
    public class TextFileLogger : ILogger
    {
        private IFileFactory _fileFactory;

        public string FileName
        {
            get { return _fileFactory.FileName; }
        }

        /// <summary>
        /// Create a TextFileLogger that write messages to the file determined by "fileFactory".
        /// </summary>
        /// <param name="fileFactory"></param>
        public TextFileLogger(IFileFactory fileFactory)
        {
            _fileFactory = fileFactory;
        }


        public void Log(IMessage message)
        {
            using (StreamWriter sw = _fileFactory.GetOuputFile())
            {
                sw.WriteLine(DateTime.Now.ToShortDateString() + " " + message.Text);
            }
        }
    }
}
