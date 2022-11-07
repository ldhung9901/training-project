using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;
namespace techpro.system.data.Models
{
    public class sys_factory_line_list_maintenance_system_model
    {
        public sys_factory_line_list_maintenance_system_model()
        {
            db = new sys_factory_line_list_maintenance_system_db();
        }

        public string createby_name { get; set; }

        public string factory_name { get; set; }
        public string factory_line_name { get; set; }
        public string maintenance_system_name { get; set; }
        public sys_factory_line_list_maintenance_system_db db { get; set; }
    }
}
