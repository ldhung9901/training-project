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
    partial class sys_factory_lineController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_factory_line",
            // icon = "menu",
            module = "system",
            id = "sys_factory_line",
            url = "/sys_factory_line_index",
            title = "sys_factory_line",
            translate = "NAV.sys_factory_line",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_factory_line;getListUse",
                "sys_factory_line;getListUses",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_factory_line;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_factory_line;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_factory_line;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_factory_line;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_factory_line;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_factory_line;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_factory_line;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_factory_line;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_factory_line_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_factory_line_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_factory_line_model item)
        {
            if (string.IsNullOrWhiteSpace(item.db.id_factory))
            {
                ModelState.AddModelError("db.id_factory", "required");
            }
            if (string.IsNullOrWhiteSpace(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }

            var searchName = repo.FindAll().Where(d => 
            d.db.id_factory == item.db.id_factory &&
            d.db.name == item.db.name && d.db.id != item.db.id).Count();
            if (searchName > 0)
            {
                ModelState.AddModelError("db.name", "existed");
            }

            return ModelState.IsValid;
        }

    }
}
