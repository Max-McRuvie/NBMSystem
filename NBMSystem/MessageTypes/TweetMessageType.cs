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
            set { if(!value.StartsWith("@") || value.Length < 1 || value.Length > 15 )
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
            set { if(value.Length < 1 || value.Length > 140)
                {
                    throw new ArgumentException("Error: The text length must be between 1 and 140 chars");
                }
                else
                {
                    tText = value;
                }

            }
        }
    }
}
