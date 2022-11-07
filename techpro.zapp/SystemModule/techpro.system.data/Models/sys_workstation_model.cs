using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;

namespace techpro.system.data.Models
{
    public class sys_workstation_model
    {
        public sys_workstation_model()
        {
            db = new sys_workstation_db();
        }

        public string factory_name { get; set; }
        public string phase_name { get; set; }

        public string factory_line_name { get; set; }

        public string createby_name { get; set; }
        public sys_workstation_db db { get; set; }
     
    }
}
