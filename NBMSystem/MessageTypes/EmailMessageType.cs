using NBMSystem.Input;
using System;
using System.Net.Mail;

namespace NBMSystem.MessageTypes
{
    public class EmailMessageType : MessageInput
    {
        private string e_sender;
        private string e_subject;
        private string e_text;

        EmailMessageType()
        {

        }

        public string EmailSender
        {
            get { return e_sender; }
            set
            {
                try
                {
                    MailAddress mail = new MailAddress(value);
                }
                catch
                {
                    throw new ArgumentException("Error: Email address is not valid");
                }
            }
        }

        public string EmailSubject
        {
            get { return e_subject; }
            set { if((value.Length > 20) || (value.Length < 1))
                {
                    throw new ArgumentException("Error: Subject must be between 1 and 20 char long");
                }
            }
        }

        public string EmailText
        {
            get { return e_text; }
            set { if ((value.Length > 1028) || (value.Length < 1))
                {
                    throw new ArgumentException("Error: Email must be between 1 and 1028 characters long");
                }

            }
        }

    }
}
