using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;

namespace techpro.system.data.Models
{
    public class sys_receiving_type_model
    {
        public sys_receiving_type_model()
        {
            db = new sys_receiving_type_db();
        }

        public string createby_name { get; set; }
        public sys_receiving_type_db db { get; set; }
    }
}
