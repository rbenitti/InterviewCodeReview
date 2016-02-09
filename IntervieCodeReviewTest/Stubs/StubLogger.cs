using InterviewCodeReview.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntervieCodeReviewTest.Stubs
{
    public abstract class StubLogger : ILogger
    {
        public List<string> Output = new List<string> { };

        public virtual void Log(IMessage message)
        {
            Output.Add(message.Text);
        }
    }
}
