using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace techpro.DataBase.Function
{
    [NotMapped]
	public class Fn_get_sys_approval
    {
		public string id { get; set; }
        [Column(TypeName="DateTime")]
		public DateTime? deadline { get; set; }
		public int? status_action { get; set; }
        //1 new, 2 in process, 3 finish, 4 cancel,5 return, 6 close
		public int? status_finish { get; set; }
		public string to_user { get; set; }
		public string from_user { get; set; }
		public string to_user_name { get; set; }
		public string from_user_name { get; set; }
		public int? step_num { get; set; }
		public string step_name { get; set; }
		public string last_note { get; set; }
		public DateTime? last_date_action { get; set; }
	}

}
