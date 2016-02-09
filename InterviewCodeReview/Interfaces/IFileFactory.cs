using System;
using System.IO;

namespace InterviewCodeReview.Interfaces
{
    public interface IFileFactory
    {
        string FileName { get; }
        StreamWriter GetOuputFile();
    }
}
