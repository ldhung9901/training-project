using System;
using System.Collections.Generic;
using System.Text;
using techpro.common.Models;

namespace techpro.system.web.Controller
{
    partial class sys_mongodb_user_auth_logController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_mongodb_user_auth_log",
            icon = "contact_mail",
            module = "system",
            id = "sys_mongodb_user_auth_log",
            url = "/sys_mongodb_user_auth_log_index",
            title = "sys_mongodb_user_auth_log",
            translate = "NAV.sys_mongodb_user_auth_log",
            type = "item",
            list_controller_action_public = new List<string>(){
                //"sys_job_title;getListUse",
                  "sys_mongodb_user_auth_log;create_detail",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 /*new ControllerRoleModel()
                {
                    id="sys_mongodb_user_auth_log;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_mongodb_user_auth_log;create_detail",
                    }
                },*/
                /*new ControllerRoleModel()
                {
                    id="sys_job_title;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_job_title;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_job_title;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_job_title;delete",
                    }
                },*/
                  new ControllerRoleModel()
                {
                    id="sys_mongodb_user_auth_log;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_mongodb_user_auth_log;DataHandler",
                          "sys_mongodb_user_auth_log;DataHandlerDetail",
                    }
                }
            }
        };

       /* private bool checkModelStateCreate(sys_job_title_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_job_title_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_job_title_model item)
        {
            if (string.IsNullOrWhiteSpace(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
            var search = repo.FindAll().Where(d => d.db.name == item.db.name && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.name", "existed");
            }


            return ModelState.IsValid;
        }*/

    }
}
