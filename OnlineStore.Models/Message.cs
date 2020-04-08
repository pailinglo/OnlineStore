using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Models
{
    public class Message
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string[] To { get; set; }
        public string[] CC { get; set; }
        public string From { get; set; }
        public bool IsHtml { get; set; }
    }
}
