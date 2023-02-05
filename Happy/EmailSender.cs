using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace Happy.RegisterConfirmation
{
    public class EmailSender : IEmailSender
    {

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get;  } 

        public Task SendEmailAsync(string toEmail, string subject, string message)
        {
          return  Execute(Options.SendGridKey, subject, message, toEmail);
        }

        private Task Execute(string apiKey, string subject, string message, string toEmail)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("ivan402000@abv.bg", "Happy"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(toEmail));

           
            msg.SetClickTracking(false, false);
            return client.SendEmailAsync(msg);
        }
    }
}
