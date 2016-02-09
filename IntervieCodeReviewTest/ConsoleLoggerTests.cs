using InterviewCodeReview;
using InterviewCodeReview.Loggers;
using InterviewCodeReview.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IntervieCodeReviewTest
{
    [TestClass]
    public class ConsoleLoggerTests
    {

        [TestMethod]
        public void ConsoleLoggerCanBeCreated()
        {
            ConsoleLogger cl = new ConsoleLogger();

            Assert.IsNotNull(cl);
        }

        [TestMethod]
        public void ConsoleLoggerCanCreateWithColors()
        {
            Dictionary<Type, ConsoleColor> colors = new Dictionary<Type, ConsoleColor>
            {
                {typeof(GeneralMessage), ConsoleColor.Blue},
                {typeof(WarningMessage), ConsoleColor.Yellow},
                {typeof(ErrorMessage), ConsoleColor.Red}
            };

            ConsoleLogger cl = new ConsoleLogger(colors);

            IDictionary<Type, ConsoleColor> instanceColors = cl.MessageColors;

            Assert.AreEqual(instanceColors.Count, 3);
            Assert.AreEqual(instanceColors[typeof(GeneralMessage)], ConsoleColor.Blue);
            Assert.AreEqual(instanceColors[typeof(WarningMessage)], ConsoleColor.Yellow);
            Assert.AreEqual(instanceColors[typeof(ErrorMessage)], ConsoleColor.Red);
        }

        [TestMethod]
        public void ConsoleLoggerCanSetColors()
        {
            Dictionary<Type, ConsoleColor> colors = new Dictionary<Type, ConsoleColor>
            {
                {typeof(GeneralMessage), ConsoleColor.Blue},
                {typeof(WarningMessage), ConsoleColor.Yellow},
                {typeof(ErrorMessage), ConsoleColor.Red}
            };

            ConsoleLogger cl = new ConsoleLogger();

            Assert.IsNull(cl.MessageColors);

            cl.MessageColors = colors;

            Assert.IsNotNull(cl.MessageColors);

            IDictionary<Type, ConsoleColor> instanceColors = cl.MessageColors;

            Assert.AreEqual(instanceColors.Count, 3);
            Assert.AreEqual(instanceColors[typeof(GeneralMessage)], ConsoleColor.Blue);
            Assert.AreEqual(instanceColors[typeof(WarningMessage)], ConsoleColor.Yellow);
            Assert.AreEqual(instanceColors[typeof(ErrorMessage)], ConsoleColor.Red);
        }

        [TestMethod]
        public void ConsoleLoggerCanLogMessagesToConsole()
        {
            string msg_text = "This is a test.";
            string actual = String.Empty;

            ConsoleLogger cl = new ConsoleLogger();

            TextWriter old_out = Console.Out;
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(ms, Encoding.UTF8, 512, true))
                {
                    Console.SetOut(sw);

                    cl.Log(new GeneralMessage(msg_text));
                }

                ms.Position = 0;
                using (StreamReader reader = new StreamReader(ms, Encoding.UTF8, true))
                {
                    actual = reader.ReadToEnd();
                }
            }

            Console.SetOut(old_out);

            Assert.AreEqual(msg_text + Environment.NewLine, actual);
        }

        // Pending: Check that console foreground color changes according to message type if there are setting defined.
    }
}
