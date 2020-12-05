using NBMSystem.Input;
using System;

namespace NBMSystem.MessageTypes
{
    public class SmsMessageType : MessageInput
    {
        private string sSender;
        private string sNumber;
        private string sText;

        public string SmsSender
        {
            get { return sSender; }
            set { sSender = value; }
        }

        public string SmsNumber
        {
            get { return sNumber; }
            set {
                if (!value.StartsWith("+"))
                {
                    throw new ArgumentException("Error: The inserted phone number must begin with a country code");
                }
                else
                {
                    sNumber = value;
                }
            }

        }

        public string SmsText
        {
            get { return sText; }
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
                    sText = value;
                }
                }
        }

    }
}
