using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewCodeReview.Exceptions
{
    
    public class JobLoggerConfigurationException : Exception
    {
        public JobLoggerConfigurationException() { }
        public JobLoggerConfigurationException(string message) : base(message) { }
        public JobLoggerConfigurationException(string message, Exception inner) : base(message, inner) { }
    }


    public class UndefinedFileNameException : Exception
    {
        public UndefinedFileNameException() { }
        public UndefinedFileNameException(string message) : base(message) { }
        public UndefinedFileNameException(string message, Exception inner) : base(message, inner) { }
    }

    public class DatabaseConnectionException : Exception
    {
        public DatabaseConnectionException() { }
        public DatabaseConnectionException(string message) : base(message) { }
        public DatabaseConnectionException(string message, Exception inner) : base(message, inner) { }
    }
    
}
