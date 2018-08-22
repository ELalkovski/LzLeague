namespace LzLeague.App.Helpers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Options;
    using SendGrid;
    using SendGrid.Helpers.Mail;

    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SendGridClient("key");
            //var from = new EmailAddress("emil27778@gmail.com", "Emil Lalkovski");
            //var to = new EmailAddress(email, email);
            //var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);


            //var response = await client.SendEmailAsync(msg);
            //var body = await response.Body.ReadAsStringAsync();
            //var status = response.StatusCode;
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("emil27778@gmail.com", "Emil Lalkovski"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.TrackingSettings = new TrackingSettings
            {
                ClickTracking = new ClickTracking { Enable = false }
            };

            await client.SendEmailAsync(msg);
            
        }

    }
}
