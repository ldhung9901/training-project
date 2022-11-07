using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;

namespace techpro.system.data.Models
{
    public class sys_group_user_model
    {
        public sys_group_user_model()
        {
            db = new sys_group_user_db();
            list_item = new List<sys_group_user_detail_model>();
        }

        public string createby_name { get; set; }
        public sys_group_user_db db { get; set; }
        public List<sys_group_user_detail_model> list_item { get; set; }
        public List<sys_group_user_role_model> list_role { get; set; }
    }
    public class sys_group_user_detail_model
    {
        public sys_group_user_detail_model()
        {
            db = new sys_group_user_detail_db();
        }

        public string user_name { get; set; }
        public sys_group_user_detail_db db { get; set; }
    }
    public class sys_group_user_role_model
    {
        public sys_group_user_role_model()
        {
            db = new sys_group_user_role_db();
        }

        public string user_name { get; set; }
        public sys_group_user_role_db db { get; set; }

    }
}
