using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;

namespace techpro.system.data.Models
{
    public class sys_item_bom_model
    {
        public sys_item_bom_model()
        {
            db = new sys_item_bom_db();
            
        }

        public string index_string { get; set; }

        public decimal? quota_cal { get; set; }

        public string createby_name { get; set; }

        public string sys_item_name { get; set; }
        public string sys_item_code { get; set; }
        public string sys_config_name { get; set; }

        public string sys_item_bom_unit_name { get; set; }

        public string sys_item_bom_name { get; set; }

        public string specification_name { get; set; }

        public string phase_name { get; set; }
        public sys_item_bom_db db { get; set; }

        
    }
}
