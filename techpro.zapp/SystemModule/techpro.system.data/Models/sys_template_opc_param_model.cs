using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;

namespace techpro.system.data.Models
{
    public class sys_template_opc_param_model
    {
        public sys_template_opc_param_model()
        {
            db = new sys_template_opc_param_db();
        }

        public string createby_name { get; set; }

        public string opcnode_name { get; set; }

        public string template_opc_name { get; set; }


        public string unit_name{ get; set; }
        public sys_template_opc_param_db db { get; set; }
     
    }
}
