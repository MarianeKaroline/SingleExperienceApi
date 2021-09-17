using SingleExperience.Repository.Services.EmailServices.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SingleExperience.Repository.Services.EmailServices
{

    public class EmailService
    {
        public async Task<bool> SendEmail(EmailMessageModel emailMessageModel)
        {
            using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("single.experience@gmail.com", "123456@Se");

                MailMessage msgObj = new MailMessage();
                msgObj.To.Add(emailMessageModel.Destination);
                msgObj.From = new MailAddress("single.experience@gmail.com");
                msgObj.Subject = emailMessageModel.MessageSubject;
                msgObj.IsBodyHtml = true;
                msgObj.Body = FormatHtml(emailMessageModel);

                client.Send(msgObj);
            }

            return true;
        }

        private string FormatHtml(EmailMessageModel emailMessageModel)
        {
            return "<!DOCTYPE html> " +
            "<html>" +
              "<body style=\"font-family:'Century Gothic'\">" +
                  "<h1 style=\"text-align:center;\"> " + emailMessageModel.MessageSubject + "</h1>" +
                  "<h2 style=\"font-size:14px;\">" +
                      "Name : " + emailMessageModel.Name + "<br />" +
                      "Email : " + emailMessageModel.From +
                  "</h2>" +
                  "<p>" + emailMessageModel.MessageBody + "</p>" +
              "</body>" +
            "</html>";
        }
    }
}
