using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace techpro.common.Models
{
    public class MailRequest
    {
        public List<string> list_id_user { get; set; }
        public string id_user { get; set; }
        // public string CCEmail { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public List<IFormFile> attachments { get; set; }
    }
}
