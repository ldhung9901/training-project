using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;

namespace techpro.system.data.Models
{
    public class sys_approval_config_model
    {
        public sys_approval_config_model()
        {
            db = new sys_approval_config_db();
            list_item = new List<sys_approval_config_detail_model>();
        }

        public string createby_name { get; set; }
        public sys_approval_config_db db { get; set; }
        public List<sys_approval_config_detail_model> list_item { get; set; }
    }
    public class sys_approval_config_detail_model
    {
        public sys_approval_config_detail_model()
        {
            db = new sys_approval_config_detail_db();
        }

        public string user_name { get; set; }
        public sys_approval_config_detail_db db { get; set; }
    }
}
