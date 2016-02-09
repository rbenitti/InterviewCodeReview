using InterviewCodeReview.Exceptions;
using InterviewCodeReview.Interfaces;
using System;
using System.IO;

namespace InterviewCodeReview
{
    /// <summary>
    /// Implementation of IFileFactory. Provides append access the file defined when instantiating.
    /// The file is open for append every time GetOuputFile is called on the instance.
    /// </summary>
    public class AppendFileFactory : IFileFactory
    {
        private string _fileName;

        /// <summary>
        /// Instantiates a file factory for the
        /// </summary>
        /// <param name="fileName"></param>
        public AppendFileFactory(string fileName)
        {
            if (fileName == null)
            {
                throw new UndefinedFileNameException("File name not defined.");
            }

            _fileName = fileName;
        }

        public string FileName
        {
            get
            {
                return _fileName;
            }
        }

        public StreamWriter GetOuputFile()
        {
            return File.AppendText(_fileName);
        }
    }
}
