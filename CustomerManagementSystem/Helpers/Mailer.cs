using System;
using System.IO;
using System.Threading.Tasks;
using CustomerManagementSystem.Models.ViewModels;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using RazorLight;

namespace CustomerManagementSystem.Helpers
{
    public static class Mailer
    {
        public static async Task SendCommentToCustomer(CustomerCommentPostModel model)
        {
            try
            {
                var engine = new RazorLightEngineBuilder()
                    .UseFileSystemProject($"{Directory.GetCurrentDirectory()}/wwwroot/emails/")
                    .UseMemoryCachingProvider()
                    .Build();
                var body = await engine.CompileRenderAsync("general.cshtml", model);
                var mail = new MimeMessage();
                mail.From.Add(new MailboxAddress(new AppConfig().SystemName, 
                    new AppConfig().EmailSender
                    ));
                mail.To.Add(new MailboxAddress(model.Name, model.EmailAddress));
                mail.Subject = model.Subject;
                mail.Body = new TextPart(TextFormat.Html) {Text = body};
                mail.Priority = MessagePriority.Urgent;
                mail.XPriority = XMessagePriority.Highest;
                using var client = new SmtpClient
                {
                    ServerCertificateValidationCallback = (s, c, 
                        h, e) => true
                };
                await client.ConnectAsync(new AppConfig().EmailServer, new AppConfig().EmailPort, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(new AppConfig().EmailUsername, 
                    new AppConfig().EmailPassword
                    );
                await client.SendAsync(mail);
                await client.DisconnectAsync(true);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}