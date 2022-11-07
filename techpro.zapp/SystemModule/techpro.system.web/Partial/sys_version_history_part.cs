using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using techpro.common.BaseClass;
using techpro.common.Models;
using techpro.system.data.Models;

namespace techpro.system.web.Controller
{
    partial class sys_version_historyController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_version_history",
            // icon = "support",
            module = "system",
            id = "sys_version_history",
            url = "/sys_version_history_index",
            title = "sys_version_history",
            translate = "system.sys_version_history",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_version_history;getListUse",
                "sys_version_history;getElementById",
                "sys_version_history;getAll",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 
                  new ControllerRoleModel()
                {
                    id="sys_version_history;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_version_history;DataHandler",
                    }
                }
            },
            is_badge = true,
            

        };
        private bool checkModelStateCreate(sys_version_history_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_version_history_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_version_history_model item)
        {
           
            
            
            return ModelState.IsValid;
        }

    }
}
