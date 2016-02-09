using InterviewCodeReview.Interfaces;
using System;
using System.Collections.Generic;

namespace InterviewCodeReview.Loggers
{
    public class ConsoleLogger : ILogger
    {
        private IDictionary<Type, ConsoleColor> _colors;

        public IDictionary<Type, ConsoleColor> MessageColors
        {
            get
            {
                return _colors;
            }
            set
            {
                _colors = value;
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ConsoleLogger()
        {
        }

        /// <summary>
        /// Creates a ConsoleLogger with a default message color mapping.
        /// </summary>
        /// <param name="colors">Dictionary mapping messages types to colors</param>
        public ConsoleLogger(IDictionary<Type, ConsoleColor> colors)
        {
            _colors = colors;
        }

        /// <summary>
        /// Sends "message" text to Console using the color mapped for "message" type, or default Console color if no color has been assigned. 
        /// </summary>
        /// <param name="message"></param>
        public void Log(IMessage message)
        {
            Type messageType = message.GetType();
            ConsoleColor previousForegroundColor = Console.ForegroundColor;
            ConsoleColor messageForegroundColor;

            messageForegroundColor = previousForegroundColor;
            if (_colors != null && _colors.ContainsKey(messageType))
            {
                messageForegroundColor = _colors[messageType];
            }

            Console.ForegroundColor = messageForegroundColor;

            Console.WriteLine(message.Text);

            Console.ForegroundColor = previousForegroundColor;
        }
    }
}
