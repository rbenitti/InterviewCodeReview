using InterviewCodeReview.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntervieCodeReviewTest.Stubs
{
    public class StubFileFactory : IFileFactory
    {
        public MemoryStream MemoryStream = new MemoryStream();

        public string FileName
        {
            get;
            set;
        }

        public StubFileFactory(string fileName)
        {
            FileName = fileName;
        }

        public StreamWriter GetOuputFile()
        {
            // public StreamWriter(Stream stream, Encoding encoding, int bufferSize, bool leaveOpen)
            // leaveOpen defines if the memoryWriter must remain open after StreamWriter is dispose
            return new StreamWriter(MemoryStream, Encoding.UTF8, 1024, true);
        }

        public void Dispose()
        {
            if (MemoryStream != null)
            {
                MemoryStream.Dispose();
            }
        }
    }
}
