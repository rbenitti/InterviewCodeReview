using InterviewCodeReview.Interfaces;

namespace InterviewCodeReview.Messages
{
    // Implement Message as an abstract class sharing concrete Messages logic
    public abstract class Message : IMessage
    {
        public const string MSG_NOT_DEFINED = "Message text not defined.";

        protected string _text;

        public string Text
        {
            get
            {
                return _text != null ? _text : MSG_NOT_DEFINED;
            }
            protected set
            {
                _text = null;

                if (value != null)
                {
                    string temp = value.Trim();

                    if (temp != string.Empty)
                    {
                        _text = temp;
                    }
                }
            }
        }

        public Message(string text)
        {
            Text = text;
        }
    }
}
