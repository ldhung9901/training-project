using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;

namespace techpro.system.data.Models
{
    public class sys_company_model
    {
        public sys_company_model()
        {
            db = new sys_company_db();
        }

        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public sys_company_db db { get; set; }
    }
}
