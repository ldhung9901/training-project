using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;

namespace techpro.system.data.Models
{
    public class sys_workstation_template_opc_model
    {
        public sys_workstation_template_opc_model()
        {
            db = new sys_workstation_template_opc_db();
        }

        public string createby_name { get; set; }

        public string workstation_name { get; set; }

        public string template_opc_name { get; set; }
        public sys_workstation_template_opc_db db { get; set; }
     
    }
}
