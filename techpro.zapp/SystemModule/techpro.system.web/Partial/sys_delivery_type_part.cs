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
    partial class sys_delivery_typeController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_delivery_type",
            // icon = "menu",
            module = "system",
            id = "sys_delivery_type",
            url = "/sys_delivery_type_index",
            title = "sys_delivery_type",
            translate = "NAV.sys_delivery_type",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_delivery_type;getListUse",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_delivery_type;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_delivery_type;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_delivery_type;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_delivery_type;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_delivery_type;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_delivery_type;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_delivery_type;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_delivery_type;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_delivery_type_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_delivery_type_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_delivery_type_model item)
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
