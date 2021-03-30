using System;
using System.Buffers.Text;
using System.Text;
using System.Threading.Tasks;
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
        [HttpGet]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromServices] IGoogleAuthProvider auth)
        {
            var credentials = await auth.GetCredentialAsync().ConfigureAwait(false);

            var gmailService = new GmailService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credentials,
                ApplicationName = "EmailSender"
            });

            var message = new Message
            {
                Raw = EmailMessageCreator.Message("Some Name", "someEmailsdlfghldjfhgkfg@gmail.com", "Some s","Hello worldlll")
            };

            var compose = new UsersResource.MessagesResource.SendRequest(gmailService, message, "me");
            await compose.ExecuteAsync().ConfigureAwait(false);

            return Redirect("/");
        }
    }
}