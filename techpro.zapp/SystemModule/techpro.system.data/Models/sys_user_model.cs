using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;

namespace techpro.system.data.Models
{
    public class sys_user_model
    {
        public sys_user_model()
        {
            db = new User();
        }
        public User db { get; set; }

        public string password { get; set; }

        public string job_title_name { get; set; }

        public string department_name { get; set; }

    }
}
