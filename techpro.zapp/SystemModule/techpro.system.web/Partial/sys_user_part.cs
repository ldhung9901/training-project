using System;
using System.ComponentModel.DataAnnotations;
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
    partial class sys_userController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_user",
            // icon = "manage_accounts",
            module = "system",
            id = "sys_user",
            url = "/sys_user_index",
            title = "sys_user",
            translate = "NAV.sys_user",
            type = "item",
            list_controller_action_public = new List<string>(){
             "sys_user;getListUse",
                "sys_user;getListUseLeader",
                "sys_user;getProfileCurrentUser",
                 "sys_user;uploadAvatar",
                 "sys_user;avatarImage"
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_user;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_user;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_user;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_user;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_user;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_user;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_user;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_user;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_user_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_user_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_user_model item)
        {
            if (string.IsNullOrWhiteSpace(item.db.Username))
            {
                ModelState.AddModelError("db.Username", "required");
            } 
            if (string.IsNullOrWhiteSpace(item.password) && action == ActionEnumForm.create)
            {
                ModelState.AddModelError("password", "required");
            }
            var search = repo.FindAll().Where(d => d.db.Username == item.db.Username && d.db.Id != item.db.Id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.Username", "existed");
            }


            return ModelState.IsValid;
        }

    }
}
