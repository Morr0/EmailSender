using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailSender.Web.Utilities;
using EmailSender.Web.Utilities.Config;
using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EmailSender.Web.Controllers
{
    [Route("/send")]
    [GoogleScopedAuthorize(GmailService.ScopeConstants.GmailCompose)]
    public class EmailController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromServices] IGoogleAuthProvider auth, [FromServices] IOptions<Email> emailConfig)
        {
            var credentials = await auth.GetCredentialAsync().ConfigureAwait(false);

            var gmailService = new GmailService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credentials,
                ApplicationName = "EmailSender"
            });

            int sections = (emailConfig.Value.Destinations.Count / 5) + 1;
            var tasks = new List<Task>(5);
            for (int i = 0; i < sections; i++)
            {
                var currFive = emailConfig.Value.Destinations.Skip(i * 5).Take(5);
                tasks.Clear();

                foreach (var destination in currFive)
                {
                    string body = emailConfig.Value.Template.Replace("{0}", destination.Name);
                    var message = new Message
                    {
                        Raw = EmailMessageCreator.Message(destination.Name, destination.Email, emailConfig.Value.Subject,body)
                    };
            
                    var compose = new UsersResource.MessagesResource.SendRequest(gmailService, message, "me");
                    tasks.Add(compose.ExecuteAsync());
                }

                await Task.WhenAny(tasks).ConfigureAwait(false);
                
                await Task.Delay(1000).ConfigureAwait(false);
            }

            return Redirect("/");
        }
    }
}