using System.Collections.Generic;
using EmailSender.Web.Dtos;

namespace EmailSender.Web.Utilities.Config
{
    public class Email
    {
        public string Subject { get; set; }
        public string Template { get; set; }
        public List<Destination> Destinations { get; set; }
    }
}