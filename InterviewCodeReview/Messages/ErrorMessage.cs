using InterviewCodeReview.Interfaces;

namespace InterviewCodeReview.Messages
{
    public class ErrorMessage : Message, IMessage
    {
        public ErrorMessage(string text) : base(text) { }
    }
}
