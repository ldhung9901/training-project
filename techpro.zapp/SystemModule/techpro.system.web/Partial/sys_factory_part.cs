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
    partial class sys_factoryController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_factory",
            // icon = "menu",
            module = "system",
            id = "sys_factory",
            url = "/sys_factory_index",
            title = "sys_factory",
            translate = "NAV.sys_factory",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_factory;getListUse",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_factory;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_factory;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_factory;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_factory;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_factory;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_factory;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_factory;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_factory;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_factory_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_factory_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_factory_model item)
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
