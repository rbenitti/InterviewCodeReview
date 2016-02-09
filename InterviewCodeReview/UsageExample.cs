using System;
using System.Collections.Generic;
using InterviewCodeReview.Messages;
using InterviewCodeReview.Loggers;
using InterviewCodeReview.Interfaces;

namespace InterviewCodeReview
{
    class UsageExample
    {
        /// <summary>
        /// Configuration example 
        /// </summary>
        static public void Configure()
        {
            string connectionString;
            string fullFileName;
            JobLogger jl = JobLogger.Instance;

            connectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];

            fullFileName = System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"]
                + "LogFile"
                + DateTime.Now.ToShortDateString()
                + ".txt";

            IDatabase database = new SQLDatabase(connectionString);

            IFileFactory fileFactory = new AppendFileFactory(fullFileName);

            Dictionary<Type, int> messageCodes = new Dictionary<Type, int> {
                { typeof(GeneralMessage), 1},
                { typeof(WarningMessage), 2},
                { typeof(ErrorMessage), 3}
            };

            Dictionary<Type, ConsoleColor> messageColors = new Dictionary<Type, ConsoleColor>
            {
                {typeof(GeneralMessage), ConsoleColor.Blue},
                {typeof(WarningMessage), ConsoleColor.Yellow},
                {typeof(ErrorMessage), ConsoleColor.Red}
            };

            jl
               .Reset()
               .SetLoggers(new List<ILogger>
                {
                    new ConsoleLogger(messageColors),
                    new TextFileLogger(fileFactory),
                    new SQLDBLogger(database, messageCodes)
                })
               .AcceptMessages<GeneralMessage>()
               .AcceptMessages<WarningMessage>()
               .AcceptMessages<ErrorMessage>();

            jl.LogMessage(new ErrorMessage("This is an error message"));
        }
    }
}
