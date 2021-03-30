using System;
using System.Text;
using MimeKit;

namespace EmailSender.Web.Utilities
{
    public static class EmailMessageCreator
    {
        /// <returns>Returns Base64</returns>
        public static string Message(string to, string toEmailAddress, string subject, string body)
        {
            var message = new MimeMessage();
            message.To.Add(new MailboxAddress(to, toEmailAddress));
            message.Subject = subject;
            message.Body = new TextPart
            {
                Text = body
            };

            byte[] bytes = Encoding.UTF8.GetBytes(message.ToString());
            return Convert.ToBase64String(bytes);
        }
    }
}