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
    partial class sys_inventory_position_item_capacityController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_inventory_position_item_capacity",
            // icon = "cabin",
            module = "system",
            id = "sys_inventory_position_item_capacity",
            url = "/sys_inventory_position_item_capacity_index",
            title = "sys_inventory_position_item_capacity",
            translate = "NAV.sys_inventory_position_item_capacity",
            type = "item",
            list_controller_action_public = new List<string>()
            {

            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_inventory_position_item_capacity;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_inventory_position_item_capacity;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_inventory_position_item_capacity;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_inventory_position_item_capacity;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_inventory_position_item_capacity;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_inventory_position_item_capacity;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_inventory_position_item_capacity;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_inventory_position_item_capacity;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_inventory_position_item_capacity_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_inventory_position_item_capacity_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_inventory_position_item_capacity_model item)
        {
            if (item.db.min_stock ==null || item.db.min_stock == 0)
            {
                ModelState.AddModelError("db.min_stock", "required");
            }
            if (item.db.max_stock == null || item.db.max_stock == 0)
            {
                ModelState.AddModelError("db.max_stock", "required");
            }
            if (string.IsNullOrWhiteSpace(item.db.id_item))
            {
                ModelState.AddModelError("db.id_item", "required");
            }
            var searchName = repo.FindAll().Where(d =>
            d.db.id_position == item.db.id_position &&
                d.db.id_warehouse == item.db.id_warehouse &&
            d.db.id_item == item.db.id_item &&
             d.db.id_specification == item.db.id_specification &&
            d.db.id != item.db.id).Count();
            if (searchName > 0)
            {
                ModelState.AddModelError("db.id_item", "existed");
                ModelState.AddModelError("db.id_specification", "existed");
            }

            return ModelState.IsValid;
        }

    }
}
