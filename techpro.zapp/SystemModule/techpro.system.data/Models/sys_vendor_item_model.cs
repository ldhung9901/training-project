using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;
namespace techpro.system.data.Models
{
    public class sys_vendor_item_model
    {
        public sys_vendor_item_model()
        {
            db = new sys_vendor_item_db();
        }


        public string sys_item_specification_name { get; set; }

        public string sys_unit_main_name { get; set; }

        public string sys_unit_name { get; set; }

        public string sys_item_name { get; set; }

        public string createby_name { get; set; }

        public string supplier_name { get; set; }
        public sys_vendor_item_db db { get; set; }
    }
}
