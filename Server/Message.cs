using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Message
    {
        public string SenderName { get; set; }
        public string Content { get; set; }
        public string TimeStamp { get; set; }

        public Message(string name, string content)
        {
            SenderName = name;
            Content = content;
            TimeStamp = DateTime.Now.ToLongTimeString();
        }
    }
}
