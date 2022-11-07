using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace techpro.common.Models
{
    public class FileModel
    {
        public IFormFile file  { get; set; }
        public string folder { get; set; }
    }
}
