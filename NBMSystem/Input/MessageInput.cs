using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBMSystem.Input
{
    public class MessageInput
    {
        private string m_header;
        private string m_body;

        public MessageInput()
        {

        }

        public string Header
        {
            get { return m_header; }
            set
            {
                if (value.Length == 10)
                {
                    m_header = value;
                }
                else
                {
                    throw new ArgumentException("Error: Header must be length of 10");
                }

            }
        }

        public string Body
        {
            get { return m_body; }
            set { if (value.Length > 0)
                {
                    m_body = value;
                }
                else
                {
                    throw new ArgumentException("Error: Body must not be empty");
                }
            }  
        }
    }
}
