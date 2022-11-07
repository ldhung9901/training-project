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
    partial class sys_item_bomController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_item_bom",
            // icon = "menu",
            module = "system",
            id = "sys_item_bom",
            url = "/sys_item_bom_index",
            title = "sys_item_bom",
            translate = "NAV.sys_item_bom",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_item_bom;getListUse",
                "sys_item_bom;get_bom_config",
                   "sys_item_bom;getAll",
                   "sys_item_bom;getElementById",
                   "sys_item_bom;get_list_quality",
                   "sys_item_bom;get_item_quota",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_item_bom;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                        "sys_item_bom;create_config",
                        "sys_item_bom;create_quality",
                          "sys_item_bom;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_item_bom;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_item_bom;edit",
                          "sys_item_bom;edit_config",

                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_item_bom;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_item_bom;delete",
                          "sys_item_bom;delete_quality",
                        "sys_item_bom;delete_bom_config",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_item_bom;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_item_bom;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_item_bom_model item)
        {
            if (string.IsNullOrWhiteSpace(item.db.id_item_bom))
            {
                ModelState.AddModelError("db.id_item_bom", "required");
            }

            var search = repo.FindAll().Where(d =>
           d.db.id_item == item.db.id_item &&
           d.db.id_item_bom_config == item.db.id_item_bom_config &&
            d.db.id_specification == item.db.id_specification &&
           d.db.id_item_bom == item.db.id_item_bom && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.id_item_bom", "existed");
            }

            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }
        private bool checkModelStateCreateItemBomConfig(sys_item_bom_config_model item)
        {
            if (string.IsNullOrWhiteSpace(item.db.config_code))
            {
                ModelState.AddModelError("db.config_code", "required");
            }
            if (string.IsNullOrWhiteSpace(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
            if (string.IsNullOrWhiteSpace(item.db.id_item))
            {
                ModelState.AddModelError("db.id_item", "required");
            }


            var search = repo._context.sys_item_bom_configs.Where(d =>
           d.id_item == item.db.id_item &&
           d.name == item.db.name && d.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.name", "existed");
            }
            return ModelState.IsValid;
        }
        private bool checkModelStateEditItemBomConfig(sys_item_bom_config_model item)
        {

            if (string.IsNullOrWhiteSpace(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }

            return ModelState.IsValid;
        }

        private bool checkModelStateEdit(sys_item_bom_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_item_bom_model item)
        {

            if (item.db.quota <= 0)
            {
                ModelState.AddModelError("db.quota", "required");
            }

            return ModelState.IsValid;
        }

    }
}
