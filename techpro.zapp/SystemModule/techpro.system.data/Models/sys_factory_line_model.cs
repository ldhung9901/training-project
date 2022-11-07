using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;
namespace techpro.system.data.Models
{
    public class sys_factory_line_model
    {
        public sys_factory_line_model()
        {
            db = new sys_factory_line_db();
            list_maintenance_system = new List<sys_factory_line_list_maintenance_system_model>();
        }

        public string createby_name { get; set; }

        public string factory_name { get; set; }
        public sys_factory_line_db db { get; set; }
        public List<sys_factory_line_list_maintenance_system_model> list_maintenance_system { get; set; }
    }
}
