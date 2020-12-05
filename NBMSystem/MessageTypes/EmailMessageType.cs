using NBMSystem.Input;
using System;
using System.Net.Mail;

namespace NBMSystem.MessageTypes
{
    public class EmailMessageType : MessageInput
    {
        private string eSender;
        private string eSubject;
        private string eText;

        public string EmailSender
        {
            get { return eSender; }
            set
            {
                try
                {
                    eSender = value;
                    MailAddress mail = new MailAddress(eSender);
                }
                catch
                {
                    throw new ArgumentException("Error: Email address is not valid");
                }
            }
        }

        public string EmailSubject
        {
            get { return eSubject; }
            set { if((value.Length > 20) || (value.Length < 1))
                {
                    throw new ArgumentException("Error: Subject must be between 1 and 20 char long");
                }
                else
                {
                    eSubject = value;
                }
            }
        }

        public string EmailText
        {
            get { return eText; }
            set { if ((value.Length > 1028) || (value.Length < 1))
                {
                    throw new ArgumentException("Error: Email must be between 1 and 1028 characters long");
                }
                else
                {
                    eText = value;
                }

            }
        }

    }
}
