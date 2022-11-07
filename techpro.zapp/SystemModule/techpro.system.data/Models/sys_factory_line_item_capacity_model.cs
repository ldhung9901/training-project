using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;
namespace techpro.system.data.Models
{
    public class sys_factory_line_item_capacity_model
    {
        public sys_factory_line_item_capacity_model()
        {
            db = new sys_factory_line_item_capacity_db();
        }

        public string factory_name { get; set; }

        public string factory_line_name { get; set; }


        public string sys_item_specification_name { get; set; }

        public string sys_unit_main_name { get; set; }

        public string sys_unit_name { get; set; }

        public string sys_item_name { get; set; }

        public string createby_name { get; set; }
        public sys_factory_line_item_capacity_db db { get; set; }
    }
}
