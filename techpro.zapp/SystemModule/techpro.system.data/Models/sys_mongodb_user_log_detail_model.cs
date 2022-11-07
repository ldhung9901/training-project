using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;
using techpro.DataBase.mongodb.log;

namespace techpro.system.data.Models
{
    public class sys_mongodb_user_log_detail_model
    {
        public sys_mongodb_user_log_detail_model()
        {
            db = new mongodb_user_log_detail_db();
            user_auth_log_db = new mongodb_user_auth_log_db();
        }

        public mongodb_user_auth_log_db user_auth_log_db { get; set; }
        public mongodb_user_log_detail_db db { get; set; }
    }
}
