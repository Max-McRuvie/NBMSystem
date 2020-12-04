using System;
using NBMSystem.Input;

namespace NBMSystem.MessageTypes
{
    public class TweetMessageType : MessageInput
    {
        private string t_sender;
        private string t_text;

        public string TweetSender
        {
            get { return t_sender; }
            set { if(!value.StartsWith("@") || value.Length > 15 || value.Length < 1)
                {
                    throw new ArgumentException("Error: The tweet sender must start with the @ symbol, and contain a maximum of 15 characters");
                }
            }
        }

        public string TweetText
        {
            get { return t_text; }
            set { if(value.Length > 140 || value.Length < 1)
                {
                    throw new ArgumentException("Error: The tweet sender must start with the @ symbol, and be a length of less than 15");
                } 

            }
        }
    }
}
