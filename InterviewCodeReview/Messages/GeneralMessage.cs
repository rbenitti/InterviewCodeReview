using InterviewCodeReview.Interfaces;

namespace InterviewCodeReview.Messages
{
	public class GeneralMessage : Message, IMessage
    {
        public GeneralMessage(string text) : base(text) { }
    }
}
