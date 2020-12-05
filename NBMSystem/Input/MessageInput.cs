using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBMSystem.Input
{
    public class MessageInput
    {
        private string mHeader;
        private string mBody;

        public string Header
        {
            get { return mHeader; }
            set
            {
                if (value.Length == 10)
                {
                    mHeader = value;
                }
                else
                {
                    throw new ArgumentException("Error: Header must be length of 10");
                }

            }
        }

        public string Body
        {
            get { return mBody; }
            set { if (value.Length > 0)
                {
                    mBody = value;
                }
                else
                {
                    throw new ArgumentException("Error: Body must not be empty");
                }
            }  
        }
    }
}
