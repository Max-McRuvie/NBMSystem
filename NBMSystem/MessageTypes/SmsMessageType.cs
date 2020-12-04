using NBMSystem.Input;
using System;

namespace NBMSystem.MessageTypes
{
    public class SmsMessageType : MessageInput
    {
        private string s_sender;
        private string s_number;
        private string s_text;

        public SmsMessageType()
        {

        }

        public string SmsSender
        {
            get { return s_sender; }
            set { s_sender = value; }
        }

        public string SmsNumber
        {
            get { return s_number; }
            set {
                if (!value.StartsWith("+"))
                {
                    throw new ArgumentException("Error: The inserted phone number must begin with a country code");
                }
                else
                {
                    s_number = value;
                }
            }

        }

        public string SmsText
        {
            get { return s_text; }
            set { 
                if ((value.Length < 0) || (value.Length > 141))
                {
                    throw new ArgumentException("Error: Length must be between 0 and 141");
                }
                else if (value == " ")
                {
                    throw new ArgumentException("Text must not be empty");
                }
                else
                {
                    s_text = value;
                }
                }
        }

    }
}
