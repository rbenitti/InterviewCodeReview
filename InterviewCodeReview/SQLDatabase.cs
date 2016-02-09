using InterviewCodeReview.Exceptions;
using InterviewCodeReview.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace InterviewCodeReview
{
    /// <summary>
    /// Implementation of IDatabase for execution of commands in a SQL database.
    /// </summary>
    class SQLDatabase : IDatabase
    {
        public const string ERR_EMPTY_CONNECTIONSTRING = "ConnectionString is not defined.";
        public const string ERR_CANNOT_OPEN_CONNECTION = "Connection cannot be opened.";

        private string _connectionString;

        public string ConnectionString { get { return _connectionString; } }

        /// <summary>
        /// Prepares an instace of SQLDatabase to receive sql commands. The connection string must be valid, or an Exception will be thrown.
        /// </summary>
        /// <param name="connectionString">Must be a valid SQLClient connection string</param>
        public SQLDatabase(string connectionString)
        {
            if (connectionString == null || connectionString.Trim() == String.Empty)
            {
                throw new DatabaseConnectionException(ERR_EMPTY_CONNECTIONSTRING);
            }

            //Check that connection string is valid
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open(); // throws if invalid
            }

            _connectionString = connectionString;
        }

        /// <summary>
        /// Executes an SQL command. Encapsulates the connection to the database.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters">Can be null. Holds parameters/values for the command to execute.</param>
        /// <returns>Rows affected count.</returns>
        public int Execute(string command, IDictionary<string, string> parameters)
        {
            int rowsAffected = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString)) 
            {
                conn.Open();

                if (conn.State == ConnectionState.Open)
                {
                    using (SqlCommand comm = new SqlCommand(command, conn))
                    {
                        if (parameters != null)
                        {
                            foreach (KeyValuePair<string, string> kv in parameters)
                            {
                                comm.Parameters.AddWithValue(kv.Key, kv.Value);
                            }
                        }

                        rowsAffected = comm.ExecuteNonQuery();
                    }
                }
                else
                {
                    throw new DatabaseConnectionException(ERR_CANNOT_OPEN_CONNECTION);
                }
            }

            return rowsAffected;
        }
    }
}
