using InterviewCodeReview.Interfaces;

namespace InterviewCodeReview.Messages
{
    public class WarningMessage : Message, IMessage
    {
        public WarningMessage(string text) : base(text) { }
    }

}
