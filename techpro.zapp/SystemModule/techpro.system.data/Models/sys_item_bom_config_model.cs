using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;

namespace techpro.system.data.Models
{
    public class sys_item_bom_config_model
    {
        public sys_item_bom_config_model()
        {
            db = new sys_item_bom_config_db();
            list_item_quota = new List<sys_item_bom_model>();
            list_item_quality = new List<sys_item_bom_quality_model>();
        }

        public string sys_item_name { get; set; }
        public string sys_config_name { get; set; }
        

        public string sys_unit_name { get; set; }
        public sys_item_bom_config_db db { get; set; }
        public List<sys_item_bom_model> list_item_quota{get;set;}
        public List<sys_item_bom_quality_model> list_item_quality{get;set;}
    }
    public class sys_item_bom_quality_model
    {
        public sys_item_bom_quality_model()
        {
            db = new sys_item_bom_quality_db();
        }
        public sys_item_bom_quality_db db { get; set; }
        
    }
}
