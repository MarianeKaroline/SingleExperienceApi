using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Repository.Services.EmailServices.Models
{
    public class EmailMessageModel
    {
        public string From { get; set; }
        public string Name { get; set; }
        public string Destination { get; set; }
        public string MessageSubject { get; set; }
        public string MessageBody { get; set; }
    }
}
