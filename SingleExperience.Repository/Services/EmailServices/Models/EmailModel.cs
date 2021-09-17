using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Repository.Services.EmailServices.Models
{
    public class EmailModel
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool EmailIsSSL { get; set; }
    }
}
