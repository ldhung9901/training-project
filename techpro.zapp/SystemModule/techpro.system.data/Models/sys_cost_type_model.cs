using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;

namespace techpro.system.data.Models
{
    public class sys_cost_type_model
    {
        public sys_cost_type_model()
        {
            db = new sys_cost_type_db();
        }

        public string createby_name { get; set; }
        public sys_cost_type_db db { get; set; }
    }
}
