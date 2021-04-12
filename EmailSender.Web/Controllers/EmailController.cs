using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailSender.Web.Dtos;
using EmailSender.Web.Services;
using EmailSender.Web.Utilities;
using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmailSender.Web.Controllers
{
    [Route("/send")]
    [GoogleScopedAuthorize(GmailService.ScopeConstants.GmailCompose)]
    public class EmailController : Controller
    {
        private readonly EmailsService _emailsService;

        public EmailController(EmailsService emailsService)
        {
            _emailsService = emailsService;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            return View("Index", new List<Destination>(_emailsService.Get()));
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromForm] SendEmailRequest request, [FromServices] IGoogleAuthProvider auth)
        {
            if (!ModelState.IsValid) return View("Index", new List<Destination>(_emailsService.Get()));
            
            var credentials = await auth.GetCredentialAsync().ConfigureAwait(false);

            var gmailService = new GmailService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credentials,
                ApplicationName = "EmailSender"
            });

            var destinations = _emailsService.Get();
            int sections = (destinations.Count / 5) + 1;
            var tasks = new List<Task>(5);
            for (int i = 0; i < sections; i++)
            {
                var currFive = destinations.Skip(i * 5).Take(5);
                tasks.Clear();

                foreach (var destination in currFive)
                {
                    string body = request.Template.Replace("{0}", destination.Name);
                    var message = new Message
                    {
                        Raw = EmailMessageCreator.Message(destination.Name, destination.Email, request.Subject, body)
                    };
            
                    var compose = new UsersResource.MessagesResource.SendRequest(gmailService, message, "me");
                    tasks.Add(compose.ExecuteAsync());
                }

                await Task.WhenAny(tasks).ConfigureAwait(false);
                
                await Task.Delay(1000).ConfigureAwait(false);
            }

            return Redirect("/");
        }

        [HttpPost("Add")]
        public IActionResult AddEmail([FromForm] Destination destination)
        {
            if (!ModelState.IsValid) return View("Index", new List<Destination>(_emailsService.Get()));
            
            _emailsService.Add(destination);
            
            return RedirectToAction("Index");
        }
    }
}