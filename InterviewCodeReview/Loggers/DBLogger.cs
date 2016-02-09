using InterviewCodeReview.Interfaces;
using System;
using System.Collections.Generic;

namespace InterviewCodeReview
{
    /// <summary>
    /// SQLDBLogger saves messages into a table Log(message, messageCode) in a SQL database. 
    /// The connection to the database is provided by facade IDatabase object.
    /// </summary>
    public class SQLDBLogger : ILogger
    {
        public const string ERR_NO_DATABASE = "A database must be provided.";
        public const string ERR_EMPTY_MESSAGE_CODE_MAPPING = "A mapping between message types an message codes must be provided.";
        public const string ERR_NO_MESSAGE_CODE = "Message type has no message code defined.";
        public const string ERR_INVALID_MESSAGE_TYPE = "Type must be a Message subclass.";

        private IDatabase _database;
        private IDictionary<Type, int> _messageCodes;

        public IDictionary<Type, int> MessageCodesMap
        {
            get { return _messageCodes; }
        }
        /// <summary>
        /// A SQLDBLogger inserts messages into a table Log(message, messageCode) in the defined database.
        /// The table Log must exist.
        /// </summary>
        /// <param name="database">Database facade</param>
        /// <param name="messageCodes">Message code mapping</param>
        public SQLDBLogger(IDatabase database, IDictionary<Type, int> messageCodes)
        {
            if (database == null)
            {
                throw new Exception(ERR_NO_DATABASE);
            }

            if (messageCodes == null || messageCodes.Count == 0)
            {
                throw new Exception(ERR_EMPTY_MESSAGE_CODE_MAPPING);
            }

            // Check that all Type keys in the dictionary implement IMessage
            foreach (KeyValuePair<Type, int> kv in messageCodes)
            {
                if (!typeof(IMessage).IsAssignableFrom(kv.Key))
                {
                    throw new Exception(ERR_INVALID_MESSAGE_TYPE);
                }
            }

            _database = database;
            _messageCodes = messageCodes;
        }

        public void Log(IMessage message)
        {
            int rowsAffected;
            int messageCode;
            string sqlCommand = String.Empty;
            Dictionary<string, string> parameters = new Dictionary<string, string> { };
            Type messageType = message.GetType();

            if (!_messageCodes.ContainsKey(messageType))
            {
                throw new Exception(ERR_NO_MESSAGE_CODE);
            }

            messageCode = _messageCodes[messageType];

            sqlCommand = "Insert into Log Values(?message, ?messageCode)";
            parameters["?message"] = message.Text;
            parameters["?messageCode"] = messageCode.ToString();

            rowsAffected = _database.Execute(sqlCommand, parameters);

        }
    }
}
