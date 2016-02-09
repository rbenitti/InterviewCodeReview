using System;
using System.Collections.Generic;
using System.Linq;
using InterviewCodeReview.Interfaces;
using InterviewCodeReview.Exceptions;
using InterviewCodeReview.Messages;

namespace InterviewCodeReview
{
    /// <summary>
    /// JobLogger (singleton)
    /// Acts as a Facade to multiple loggers     
    /// </summary>
    public class JobLogger
    {
        public const string ERR_EMPTY_LOGGERS = "JobLogger must have at least one logger.";
        public const string ERR_EMPTY_ACCEPTED_MESSAGES = "JobLogger must be specified at least one message type to log.";
        public const string ERR_INVALID_MESSAGE_TYPE = "Message types must implement IMessage.";
        public const string ERR_LOGGER_NOT_CONFIGURED = "JobLogger has not been configured correctly.";

        static readonly private JobLogger _instance = new JobLogger();
        private List<ILogger> _loggers = new List<ILogger> { };
        private HashSet<Type> _acceptedMessageTypes = new HashSet<Type> { };

        /// <summary>
        /// JobLogger Singleton instance
        /// </summary>
        public static JobLogger Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Property. Returns a collection with the type of messages that are currently being logged 
        /// </summary>
        public ICollection<Type> AcceptedMessageTypes
        {
            // generate a copy
            get { return _acceptedMessageTypes.ToList(); }
        }

        /// <summary>
        /// Property. Returns a collection with the current loggers.
        /// </summary>
        public ICollection<ILogger> Loggers
        {
            // generate a copy
            get { return _loggers.ToList(); }
        }

        // Prevent instantiation of class from the outside
        private JobLogger() { }

        /// <summary>
        /// Set the JobLogger loggers from the input collection. 
        /// (Removes previous configuration)
        /// </summary>
        /// <param name="loggers"></param>
        /// <returns>Singleton instance</returns>
        public JobLogger SetLoggers(ICollection<ILogger> loggers)
        {
            if (loggers == null || loggers.Count <= 0)
            {
                throw new JobLoggerConfigurationException(ERR_EMPTY_LOGGERS);
            }

            _loggers = new List<ILogger> { };

            foreach (ILogger l in loggers)
            {
                _loggers.Add(l);
            }

            return Instance;
        }

        /// <summary>
        /// Set the message types that will be logged by the loggers. 
        /// (Removes previous configuration)
        /// </summary>
        /// <param name="messageTypes"></param>
        /// <returns></returns>
        public JobLogger SetAcceptedMessages(ICollection<Type> messageTypes)
        {
            if (messageTypes == null || messageTypes.Count <= 0)
            {
                throw new JobLoggerConfigurationException(ERR_EMPTY_ACCEPTED_MESSAGES);
            }

            foreach (Type t in messageTypes)
            {
                if (!typeof(IMessage).IsAssignableFrom(t))
                {
                    throw new JobLoggerConfigurationException(ERR_INVALID_MESSAGE_TYPE);
                }
            }

            _acceptedMessageTypes = new HashSet<Type>(messageTypes);

            return Instance;
        }

        /// <summary>
        /// Enables logging of messages of type T. 
        /// </summary>
        /// <typeparam name="T">T must implement IMessage</typeparam>
        /// <returns></returns>
        public JobLogger AcceptMessages<T>() where T : IMessage
        {
            _acceptedMessageTypes.Add(typeof(T));

            return Instance;
        }

        /// <summary>
        /// Disables logging of messages of type T. 
        /// </summary>
        /// <typeparam name="T">T must implement IMessage</typeparam>
        public void RejectMessages<T>() where T : IMessage
        {
            _acceptedMessageTypes.Remove(typeof(T));
        }

        /// <summary>
        /// Removes loggers and message type configuration.
        /// </summary>
        /// <returns></returns>
        public JobLogger Reset()
        {
            _acceptedMessageTypes = new HashSet<Type> { };
            _loggers = new List<ILogger> { };

            return Instance;
        }

        /// <summary>
        /// If the type of "message" is enabled for logging, logs "message" using all configured loggers.
        /// </summary>
        /// <param name="message"></param>
        public void LogMessage(IMessage message)
        {
            List<Exception> exceptions;

            if (_acceptedMessageTypes.Count == 0 || _loggers.Count == 0)
            {
                throw new JobLoggerConfigurationException(ERR_LOGGER_NOT_CONFIGURED);
            }

            if (_acceptedMessageTypes.Contains(message.GetType()))
            {
                exceptions = new List<Exception>();

                foreach (ILogger logger in _loggers)
                {
                    try
                    {
                        logger.Log(message);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }
                if (exceptions.Count > 0)
                {
                    throw new AggregateException(exceptions);
                }
            }

        }

        /// <summary>
        /// Configuration example 
        /// </summary>
        static public void Configure()
        {
            string connectionString;
            string fullFileName;

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

            _instance
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
        }
    }
}