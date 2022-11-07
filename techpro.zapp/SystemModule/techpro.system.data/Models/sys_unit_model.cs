using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;

namespace techpro.system.data.Models
{
    public class sys_unit_model
    {
        public sys_unit_model()
        {
            db = new sys_unit_db();
        }

        public string createby_name { get; set; }
        public sys_unit_db db { get; set; }
    }
}
