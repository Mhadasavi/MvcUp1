using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcUp1.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public MailJetSetting _mailJetSetting { get; set; }
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        Task IEmailSender.SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }
        public async Task<MailjetResponse> Execute(string email, string subject, string body)
        {
            _mailJetSetting = _configuration.GetSection("MailJet").Get<MailJetSetting>();
            MailjetClient client = new MailjetClient((_mailJetSetting.ApiKey), (_mailJetSetting.SecretKey))
            {
                //Version = ApiVersion.V3_1,
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = SendV31.Resource,
            }
             .Property(Send.Messages, new JArray {
     new JObject {
      {
       "From",
       new JObject {
        {"Email", "uchaudhary57@gmail.com"},
        {"Name", "Marcus"}
       }
      }, {
       "To",
       new JArray {
        new JObject {
         {
          "Email",
          email
         }, {
          "Name",
          "Marcus"
         }
        }
       }
      }, {
       "Subject",
       "Greetings from Mailjet."
      }, {
       "TextPart",
       "My first Mailjet email"
      }, {
       "HTMLPart",
       "<h3>Dear passenger 1, welcome to <a href='https://www.mailjet.com/'>Mailjet</a>!</h3><br />May the delivery force be with you!"
      }, {
       "CustomID",
       "AppGettingStartedTest"
      }
     }
             });
           MailjetResponse response= await client.PostAsync(request);
            Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));
            return response;
        }
    }
}
