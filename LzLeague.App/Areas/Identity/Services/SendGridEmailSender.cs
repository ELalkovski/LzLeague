namespace LzLeague.App.Areas.Identity.Services
{
    using System;
    using System.Threading.Tasks;
    using LzLeague.App.Helpers;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Options;
    using SendGrid;
    using SendGrid.Helpers.Mail;

    public class SendGridEmailSender : IEmailSender
    {
        private SendGridOptions options;

        public SendGridEmailSender(IOptions<SendGridOptions> options)
        {
            this.options = options.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SendGridClient(this.options.SendGridKey);
            var from = new EmailAddress("emil27778@gmail.com", "LzLeague");
            var to = new EmailAddress(email, email);
            var mail = MailHelper.CreateSingleEmail(from, to, subject, message, message);
            var response = await client.SendEmailAsync(mail);
        }
    }
}
