using InterviewCodeReview;
using InterviewCodeReview.Interfaces;
using InterviewCodeReview.Messages;
using IntervieCodeReviewTest.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace IntervieCodeReviewTest
{
    [TestClass]
    public class DBLoggerTest
    {
        private const string gmsg_text = "Genera message text";
        private const string wmsg_text = "Warning message text";
        private const string emsg_text = "Error message text";

        [TestMethod]
        public void SQLDBLoggerCanBeCreated()
        {
            IDatabase database = new StubDatabase();
            Dictionary<Type, int> messageCodesMap = new Dictionary<Type, int> {
                    {typeof(GeneralMessage), 1 },
                    {typeof(WarningMessage), 2 },
                    {typeof(ErrorMessage), 3 },
                };
            SQLDBLogger dblogger = new SQLDBLogger(database, messageCodesMap);

            Assert.IsNotNull(dblogger);
            Assert.IsInstanceOfType(dblogger, typeof(ILogger));
            Assert.AreEqual(messageCodesMap, dblogger.MessageCodesMap);

        }

        [TestMethod]
        public void SQLDBLoggerCanLogToDatabase()
        {
            StubDatabase database = new StubDatabase();
            Dictionary<Type, int> messageCodesMap = new Dictionary<Type, int> {
                    {typeof(GeneralMessage), 1 },
                    {typeof(WarningMessage), 2 },
                    {typeof(ErrorMessage), 3 },
                };

            SQLDBLogger dblogger = new SQLDBLogger(database, messageCodesMap);

            dblogger.Log(new ErrorMessage(emsg_text));

            Assert.AreEqual(emsg_text, database.Parameters["?message"]);
            Assert.AreEqual(messageCodesMap[typeof(ErrorMessage)].ToString(), database.Parameters["?messageCode"]);
        }
    }
}
