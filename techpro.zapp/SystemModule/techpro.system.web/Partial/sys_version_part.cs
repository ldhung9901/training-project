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
    partial class sys_versionController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_version",
            // icon = "support",
            module = "system",
            id = "sys_version",
            url = "/sys_version_index",
            title = "sys_version",
            translate = "NAV.sys_version",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_version;getListUse",
                "sys_version;getElementById",
                "sys_version;getAll",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_version;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_version;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_version;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_version;edit",
                           "sys_version;revert",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_version;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_version;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_version;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_version;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_version_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_version_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_version_model item)
        {
           
            if (string.IsNullOrWhiteSpace(item.db.version))
            {
                ModelState.AddModelError("db.version", "required");
            }
            if (item.db.release_day == null)
            {
                ModelState.AddModelError("db.release_day", "required");
            }
            if (string.IsNullOrWhiteSpace(item.db.note))
            {
                ModelState.AddModelError("db.note", "required");
            }
            var search = repo.FindAll().Where(d => d.db.version == item.db.version && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.version", "existed");
            }
            return ModelState.IsValid;
        }

    }
}
