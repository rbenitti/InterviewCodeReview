using Microsoft.VisualStudio.TestTools.UnitTesting;
using InterviewCodeReview.Messages;

namespace IntervieCodeReviewTest
{
    [TestClass]
    public class MessageTests
    {
        [TestMethod]
        public void MessageGeneralMessageCanBeCreated()
        {
            string gm_text = "This is an general message.";
            Message gm = new GeneralMessage(gm_text);
            Assert.AreEqual(gm_text, gm.Text);
        }

        [TestMethod]
        public void MessageWarningMessageCanBeCreated()
        {
            string wm_text = "This is an warning message.";
            Message wm = new WarningMessage(wm_text);
            Assert.AreEqual(wm_text, wm.Text);
        }

        [TestMethod]
        public void MessageErrorMessageCanBeCreated()
        {
            string em_text = "This is an error message.";
            Message em = new ErrorMessage(em_text);
            Assert.AreEqual(em_text, em.Text);
        }


        [TestMethod]
        public void MessageGeneralMessageEmpty()
        {
            Message m_null = new GeneralMessage(null);
            Assert.AreEqual(Message.MSG_NOT_DEFINED, m_null.Text);

            Message m_empty = new GeneralMessage("");
            Assert.AreEqual(Message.MSG_NOT_DEFINED, m_empty.Text);
        }

        [TestMethod]
        public void MessageWarningMessageEmpty()
        {
            Message m_null = new WarningMessage(null);
            Assert.AreEqual(Message.MSG_NOT_DEFINED, m_null.Text);

            Message m_empty = new WarningMessage("");
            Assert.AreEqual(Message.MSG_NOT_DEFINED, m_empty.Text);
        }

        [TestMethod]
        public void MessageErrorMessageEmpty()
        {
            Message m_null = new ErrorMessage(null);
            Assert.AreEqual(Message.MSG_NOT_DEFINED, m_null.Text);

            Message m_empty = new ErrorMessage("");
            Assert.AreEqual(Message.MSG_NOT_DEFINED, m_empty.Text);
        }
    }
}
