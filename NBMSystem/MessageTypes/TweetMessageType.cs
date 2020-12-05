using System;
using NBMSystem.Input;

namespace NBMSystem.MessageTypes
{
    public class TweetMessageType : MessageInput
    {
        private string tSender;
        private string tText;

        public string TweetSender
        {
            get { return tSender; }
            set { if(!value.StartsWith("@") || value.Length > 15 || value.Length < 1)
                {
                    throw new ArgumentException("Error: The tweet sender must start with the @ symbol, and contain a maximum of 15 characters");
                }
                else
                {
                    tSender = value;
                }
            }
        }

        public string TweetText
        {
            get { return tText; }
            set { if(value.Length > 140 || value.Length < 1)
                {
                    throw new ArgumentException("Error: The tweet sender must start with the @ symbol, and be a length of less than 15");
                }
                else
                {
                    tText = value;
                }

            }
        }
    }
}
