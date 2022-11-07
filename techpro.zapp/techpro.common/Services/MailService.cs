
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using techpro.common.Models;
using techpro.DataBase.Provider;

namespace techpro.common.Services
{
    public interface IMailService
    {
        void send_mail_mutiple_user(MailRequest mailRequest);
        void send_mail_user(MailRequest mailRequest);
    }

    public class MailService : IMailService
    {
        private techproDefautContext _context;
        IServiceScopeFactory _serviceFactory;


        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings, techproDefautContext context, IServiceScopeFactory serviceFactory)
        {
            _mailSettings = mailSettings.Value;
            _serviceFactory = serviceFactory;
            _context = context;
        }
        [AutomaticRetry(Attempts = 20)]
        public void Email(string htmlString, string Subject, string id_user = null)
        {
            BackgroundJob.Enqueue(() =>

                send(htmlString, Subject, id_user)
            );
        }

        public void send(string htmlString, string Subject, string id_user)
        {
            using (var scope = _serviceFactory.CreateScope())
            {

                var dbContext = scope.ServiceProvider.GetService<techproDefautContext>();

                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(_mailSettings.Mail, _mailSettings.DisplayName);
                if (id_user != null)
                {
                    var user_mail = dbContext.users.Where(d => d.Id == id_user).Select(d => d.email).FirstOrDefault();
                    message.To.Add(new MailAddress(user_mail));
                    Console.WriteLine(user_mail);
                }
                //message.CC.Add(new MailAddress("hoang.vu@techpro.com.vn"));
                message.Subject = Subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = htmlString;
                smtp.Port = _mailSettings.Port;
                smtp.Host = _mailSettings.Host; //for gmail host  
                smtp.UseDefaultCredentials = false;

                smtp.Credentials = new NetworkCredential(_mailSettings.Mail, _mailSettings.Password);
                smtp.EnableSsl = true;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                        System.Security.Cryptography.X509Certificates.X509Chain chain,
                        System.Net.Security.SslPolicyErrors sslPolicyErrors) {
                            return true;
                        };
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            };
        }

        public void send_mail_mutiple_user(MailRequest mailRequest)
        {
            mailRequest.list_id_user.ForEach(e =>
            {
                Email(mailRequest.body, mailRequest.subject, e);
            });
        }
        public void send_mail_user(MailRequest mailRequest)
        {

            Email(mailRequest.body, mailRequest.subject, mailRequest.id_user);
        }

    }
}