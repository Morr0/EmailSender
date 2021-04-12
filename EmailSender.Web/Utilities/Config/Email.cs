using System.Collections.Generic;

namespace EmailSender.Web.Utilities.Config
{
    public class Email
    {
        public string Subject { get; set; }
        public string Template { get; set; }
        public List<Destination> Destinations { get; set; }
    }
}