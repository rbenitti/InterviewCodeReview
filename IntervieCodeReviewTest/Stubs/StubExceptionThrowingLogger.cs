using InterviewCodeReview.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntervieCodeReviewTest.Stubs
{
    public class StubExceptionThrowingLogger : StubLogger, ILogger
    {
        public override void Log(IMessage message)
        {
            throw new Exception("Message cannot be logged");
        }
    }
}
