using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;

namespace techpro.common.Models
{
    public class ControllerAppModel
    {
        public int badge_return;
        public int badge_approval;

        public ControllerAppModel()
        {
            list_role = new List<ControllerRoleModel>();
            list_controller_action_public = new List<string>();
        }

        public string id { get; set; }

        public string title { get; set; }

        public string url { get; set; }

        public string icon { get; set; }

        public string translate { get; set; }

        public string type { get; set; }

        public string controller { get; set; }

        public string module { get; set; }
        public bool is_badge { get; set; }
        public bool is_approval { get; set; }
        public bool is_show_all_user { get; set; }
        public string badge_version { get; set; }


        public List<ControllerRoleModel> list_role { get; set; }
        public List<string> list_controller_action_public { get; set; }
    }
    public class ControllerRoleModel
    {
        public ControllerRoleModel()
        {
            list_controller_action = new List<string>();
        }

        public string id { get; set; }


        public string name { get; set; }

        public List<string> list_controller_action { get; set; }
    }
}
