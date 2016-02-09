using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InterviewCodeReview;
using InterviewCodeReview.Interfaces;
using System.Collections.Generic;
using IntervieCodeReviewTest.Stubs;
using InterviewCodeReview.Messages;
using InterviewCodeReview.Exceptions;

namespace IntervieCodeReviewTest
{
    [TestClass]
    public class JobLoggerTest
    {
        [TestMethod]
        public void JobLoggerIsSingleton()
        {
            JobLogger jl1 = JobLogger.Instance;
            JobLogger jl2 = JobLogger.Instance;

            Assert.AreSame(jl1, jl2);
        }

        [TestMethod]
        public void JobLoggerCanSetLoggersFromCollection()
        {
            JobLogger jl = JobLogger.Instance;

            List<ILogger> loggers = new List<ILogger>
            {
                new StubConsoleLogger(),
                new StubTextFileLogger(),
                new StubDBLogger()
            };

            jl.SetLoggers(loggers);

            Assert.AreEqual(loggers.Count, jl.Loggers.Count);

            foreach (ILogger t in jl.Loggers)
            {
                Assert.IsTrue(loggers.Contains(t));
            }
        }

        [TestMethod]
        public void JobLoggerCanSetAcceptedMessageTypesFromCollection()
        {
            JobLogger jl = JobLogger.Instance;

            List<Type> acceptedMessages = new List<Type>
            {
                typeof(GeneralMessage),
                typeof(ErrorMessage),
                typeof(WarningMessage)
            };

            jl.SetAcceptedMessages(acceptedMessages);

            Assert.AreEqual(acceptedMessages.Count, jl.AcceptedMessageTypes.Count);

            foreach (Type t in jl.AcceptedMessageTypes)
            {
                Assert.IsTrue(acceptedMessages.Contains(t));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(JobLoggerConfigurationException), JobLogger.ERR_EMPTY_LOGGERS)]
        public void JobLoggerCannotSetLoggersFromEmptyCollection()
        {
            JobLogger jl = JobLogger.Instance;

            List<ILogger> loggers = new List<ILogger>
            {
            };

            jl.SetLoggers(loggers);
        }

        [TestMethod]
        [ExpectedException(typeof(JobLoggerConfigurationException), JobLogger.ERR_EMPTY_LOGGERS)]
        public void JobLoggerCannotSetLoggersFromNullCollection()
        {
            JobLogger jl = JobLogger.Instance;

            jl.SetLoggers(null);
        }

        [TestMethod]
        [ExpectedException(typeof(JobLoggerConfigurationException), JobLogger.ERR_EMPTY_ACCEPTED_MESSAGES)]
        public void JobLoggerCannotAcceptMessageTypesFromEmptyCollection()
        {
            JobLogger jl = JobLogger.Instance;

            List<Type> acceptedMessages = new List<Type>
            {
            };

            jl.SetAcceptedMessages(acceptedMessages);
        }

        [TestMethod]
        [ExpectedException(typeof(JobLoggerConfigurationException), JobLogger.ERR_EMPTY_ACCEPTED_MESSAGES)]
        public void JobLoggerCannotAcceptMessageTypesFromNullCollection()
        {
            JobLogger jl = JobLogger.Instance;

            jl.SetAcceptedMessages((ICollection<Type>)null);
        }

        [TestMethod]
        public void JobLoggerCanBeReseted()
        {
            JobLogger jl = JobLogger.Instance;

            List<ILogger> loggers = new List<ILogger>
            {
                new StubConsoleLogger(),
                new StubTextFileLogger(),
                new StubDBLogger()
            };

            List<Type> acceptedMessages = new List<Type>
            {
                typeof(GeneralMessage),
                typeof(ErrorMessage),
                typeof(WarningMessage)
            };

            jl.SetLoggers(loggers);
            jl.SetAcceptedMessages(acceptedMessages);

            Assert.AreEqual(loggers.Count, jl.Loggers.Count);
            Assert.AreEqual(acceptedMessages.Count, jl.Loggers.Count);

            jl.Reset();

            Assert.AreEqual(0, jl.Loggers.Count);
            Assert.AreEqual(0, jl.Loggers.Count);

        }

        [TestMethod]
        public void JobLoggerCanAcceptMessageTypes()
        {
            JobLogger jl = JobLogger.Instance;

            jl.Reset();

            jl.SetAcceptedMessages(new List<Type> { typeof(ErrorMessage) });

            jl.AcceptMessages<ErrorMessage>();
            jl.AcceptMessages<WarningMessage>();

            Assert.AreEqual(2, jl.AcceptedMessageTypes.Count);

            Assert.IsTrue(jl.AcceptedMessageTypes.Contains(typeof(ErrorMessage)));
            Assert.IsTrue(jl.AcceptedMessageTypes.Contains(typeof(WarningMessage)));
            Assert.IsFalse(jl.AcceptedMessageTypes.Contains(typeof(GeneralMessage)));
        }


        [TestMethod]
        public void JobLoggerCanRejectMessageTypes()
        {
            JobLogger jl = JobLogger.Instance;

            jl.Reset();

            jl.AcceptMessages<ErrorMessage>();
            jl.AcceptMessages<WarningMessage>();

            Assert.AreEqual(2, jl.AcceptedMessageTypes.Count);
            Assert.IsTrue(jl.AcceptedMessageTypes.Contains(typeof(ErrorMessage)));
            Assert.IsTrue(jl.AcceptedMessageTypes.Contains(typeof(WarningMessage)));

            jl.RejectMessages<WarningMessage>();

            Assert.AreEqual(1, jl.AcceptedMessageTypes.Count);
            Assert.IsTrue(jl.AcceptedMessageTypes.Contains(typeof(ErrorMessage)));
            Assert.IsFalse(jl.AcceptedMessageTypes.Contains(typeof(WarningMessage)));

        }

        [TestMethod]
        public void JobLoggerCanLogAcceptedMessageTypes()
        {
            JobLogger jl = JobLogger.Instance;

            jl.Reset();

            StubLogger consoleLogger = new StubConsoleLogger();

            List<ILogger> loggers = new List<ILogger>
            {
                consoleLogger
            };

            jl.SetLoggers(loggers);
            jl.AcceptMessages<ErrorMessage>();

            Assert.AreEqual(0, consoleLogger.Output.Count);

            jl.LogMessage(new GeneralMessage("This general message should not be logged."));

            Assert.AreEqual(0, consoleLogger.Output.Count);

            jl.LogMessage(new ErrorMessage("This error message should be logged."));

            Assert.AreEqual(1, consoleLogger.Output.Count);
            Assert.AreEqual("This error message should be logged.", consoleLogger.Output[0]);
        }

        [TestMethod]
        public void JobLoggerCannotLogRejectMessageTypes()
        {
            JobLogger jl = JobLogger.Instance;

            jl.Reset();

            StubLogger consoleLogger = new StubConsoleLogger();

            List<ILogger> loggers = new List<ILogger>
            {
                consoleLogger
            };

            jl.SetLoggers(loggers);
            jl.AcceptMessages<GeneralMessage>();
            jl.RejectMessages<ErrorMessage>();

            Assert.AreEqual(0, consoleLogger.Output.Count);

            jl.LogMessage(new ErrorMessage("This error message should not be logged."));

            Assert.AreEqual(0, consoleLogger.Output.Count);
        }

        [TestMethod]
        public void JobLoggerCannotLogUnspecifiedMessageTypes()
        {
            JobLogger jl = JobLogger.Instance;

            jl.Reset();

            StubLogger consoleLogger = new StubConsoleLogger();

            List<ILogger> loggers = new List<ILogger>
            {
                consoleLogger
            };

            jl.SetLoggers(loggers);
            jl.AcceptMessages<GeneralMessage>();

            Assert.AreEqual(0, consoleLogger.Output.Count);

            jl.LogMessage(new ErrorMessage("This error message should not be logged."));

            Assert.AreEqual(0, consoleLogger.Output.Count);
        }

        [TestMethod]
        public void JobLoggerCanLogToAllLoggers()
        {
            string msg_text1 = "This is a general message 1";
            string msg_text2 = "This is a general message 2";
            string msg_text3 = "This is a general message 3";
            string msg_text4 = "This is a general message 4";

            JobLogger jl = JobLogger.Instance;

            StubLogger consoleLogger = new StubConsoleLogger();
            StubLogger textFileLogger = new StubTextFileLogger();
            StubLogger dbLogger = new StubDBLogger();

            //Log two time to console logger
            List<ILogger> loggers = new List<ILogger>
            {
                consoleLogger,
                consoleLogger,
                textFileLogger,
                dbLogger
            };

            List<Type> acceptedMessages = new List<Type>
            {
                typeof(GeneralMessage),
            };

            jl.SetLoggers(loggers);
            jl.AcceptMessages<GeneralMessage>();

            Assert.AreEqual(0, consoleLogger.Output.Count);
            Assert.AreEqual(0, textFileLogger.Output.Count);
            Assert.AreEqual(0, dbLogger.Output.Count);

            jl.LogMessage(new GeneralMessage(msg_text1));
            jl.LogMessage(new GeneralMessage(msg_text2));
            jl.LogMessage(new GeneralMessage(msg_text3));
            jl.LogMessage(new GeneralMessage(msg_text4));

            Assert.AreEqual(8, consoleLogger.Output.Count);
            Assert.AreEqual(4, textFileLogger.Output.Count);
            Assert.AreEqual(4, dbLogger.Output.Count);

            CollectionAssert.AreEqual(new List<string> { msg_text1, msg_text1, msg_text2, msg_text2, msg_text3, msg_text3, msg_text4, msg_text4 }, consoleLogger.Output);
            CollectionAssert.AreEqual(new List<string> { msg_text1, msg_text2, msg_text3, msg_text4 }, textFileLogger.Output);
            CollectionAssert.AreEqual(new List<string> { msg_text1, msg_text2, msg_text3, msg_text4 }, dbLogger.Output);
        }

        [TestMethod]
        [ExpectedException(typeof(JobLoggerConfigurationException), JobLogger.ERR_LOGGER_NOT_CONFIGURED)]
        public void JobLoggerCannotLogIfHasNotLoggers()
        {
            JobLogger jl = JobLogger.Instance;
            jl.Reset();

            jl.AcceptMessages<ErrorMessage>();

            jl.LogMessage(new ErrorMessage("Cannot log this message."));
        }

        [TestMethod]
        [ExpectedException(typeof(JobLoggerConfigurationException), JobLogger.ERR_LOGGER_NOT_CONFIGURED)]
        public void JobLoggerCannotLogIfHasNotAcceptedMessages()
        {
            JobLogger jl = JobLogger.Instance;

            jl.Reset();

            StubLogger consoleLogger = new StubConsoleLogger();

            //Log two time to console logger
            List<ILogger> loggers = new List<ILogger>
            {
                consoleLogger,
            };

            jl.SetLoggers(loggers);

            jl.LogMessage(new ErrorMessage("Cannot log this message."));
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void JobLoggerCanAggregateLoggersExceptions()
        {
            JobLogger jl = JobLogger.Instance;

            jl.Reset();

            StubLogger consoleLogger1 = new StubConsoleLogger();
            StubLogger consoleLogger2 = new StubConsoleLogger();
            StubLogger exceptionLogger1 = new StubExceptionThrowingLogger();
            StubLogger exceptionLogger2 = new StubExceptionThrowingLogger();
            StubLogger exceptionLogger3 = new StubExceptionThrowingLogger();

            List<ILogger> loggers = new List<ILogger>
            {
                exceptionLogger1,
                consoleLogger1,
                exceptionLogger2,
                consoleLogger2,
                exceptionLogger3
            };

            jl.SetLoggers(loggers);
            jl.AcceptMessages<ErrorMessage>();

            jl.LogMessage(new ErrorMessage("Error message."));
        }


        [TestMethod]
        public void JobLoggerCanContinueIfLoggersThrowExceptions()
        {
            int numberOfException = 0;
            string msg_text = "Error message.";

            JobLogger jl = JobLogger.Instance;

            jl.Reset();

            StubLogger consoleLogger1 = new StubConsoleLogger();
            StubLogger consoleLogger2 = new StubConsoleLogger();
            StubLogger exceptionLogger1 = new StubExceptionThrowingLogger();
            StubLogger exceptionLogger2 = new StubExceptionThrowingLogger();
            StubLogger exceptionLogger3 = new StubExceptionThrowingLogger();

            List<ILogger> loggers = new List<ILogger>
            {
                exceptionLogger1,
                consoleLogger1,
                exceptionLogger2,
                consoleLogger2,
                exceptionLogger3
            };

            jl.SetLoggers(loggers);
            jl.AcceptMessages<ErrorMessage>();

            try
            {
                jl.LogMessage(new ErrorMessage(msg_text));
            }
            catch (AggregateException agg)
            {
                numberOfException = agg.InnerExceptions.Count();
            }

            Assert.AreEqual(3, numberOfException);

            Assert.AreEqual(1, consoleLogger1.Output.Count);
            Assert.AreEqual(1, consoleLogger2.Output.Count);

            CollectionAssert.AreEqual(new List<string> { msg_text }, consoleLogger1.Output);
            CollectionAssert.AreEqual(new List<string> { msg_text }, consoleLogger2.Output);
        }
    }
}
