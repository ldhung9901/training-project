using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;

namespace techpro.system.data.Models
{
    public class sys_item_specification_model
    {
        public sys_item_specification_model()
        {
            db = new sys_item_specification_db();
        }

        public string createby_name { get; set; }

        public string sys_item_unit_name { get; set; }

        public string sys_item_name { get; set; }

        public string sys_unit_name { get; set; }
        public sys_item_specification_db db { get; set; }

       
    }
  
}
