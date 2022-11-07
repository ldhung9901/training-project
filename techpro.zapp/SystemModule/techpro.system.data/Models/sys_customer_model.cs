using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;

namespace techpro.system.data.Models
{
    public class sys_customer_model
    {
        public sys_customer_model()
        {
            db = new sys_customer_db();
        }

        public string createby_name { get; set; }
        public sys_customer_db db { get; set; }
    }
}
