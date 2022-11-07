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
    partial class sys_receiving_typeController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_receiving_type",
            // icon = "menu",
            module = "system",
            id = "sys_receiving_type",
            url = "/sys_receiving_type_index",
            title = "sys_receiving_type",
            translate = "NAV.sys_receiving_type",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_receiving_type;getListUse",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_receiving_type;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_receiving_type;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_receiving_type;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_receiving_type;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_receiving_type;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_receiving_type;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_receiving_type;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_receiving_type;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_receiving_type_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_receiving_type_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_receiving_type_model item)
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
        }

    }
}
