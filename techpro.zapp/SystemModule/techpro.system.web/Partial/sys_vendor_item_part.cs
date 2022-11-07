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
    partial class sys_vendor_itemController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_vendor_item",
            // icon = "menu",
            module = "system",
            id = "sys_vendor_item",
            url = "/sys_vendor_item_index",
            title = "sys_vendor_item",
            translate = "NAV.sys_vendor_item",
            type = "item",
            list_controller_action_public = new List<string>()
            {

            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_vendor_item;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_vendor_item;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_vendor_item;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_vendor_item;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_vendor_item;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_vendor_item;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_vendor_item;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_vendor_item;DataHandler",
                    }
                },
                    new ControllerRoleModel()
                {
                    id="sys_vendor_item;listHistory",
                    name="listHistory",
                    list_controller_action = new List<string>()
                    {
                          "sys_vendor_item;DataHandlerHistory",
                    }
                },
            }
        };
        private bool checkModelStateCreate(sys_vendor_item_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_vendor_item_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_vendor_item_model item)
        {
            if (item.db.min_stock_order.ToString() == null || item.db.min_stock_order == 0)
            {
                ModelState.AddModelError("db.min_stock_order", "required");
            }
            if (item.db.price_item.ToString() == null || item.db.price_item == 0)
            {
                ModelState.AddModelError("db.price_item", "required");
            }
            if (item.db.delivery_time.ToString() == null || item.db.delivery_time == 0)
            {
                ModelState.AddModelError("db.delivery_time", "required");
            }

            if (string.IsNullOrWhiteSpace(item.db.id_item))
            {
                ModelState.AddModelError("db.id_item", "required");
            }
            //Phúc.Trần sửa 4/8/2022
            int searchName = repo.FindAll().Where(d => d.db.id_supplier == item.db.id_supplier &&d.db.id_item == item.db.id_item && d.db.id_specification == item.db.id_specification &&
                d.db.id != item.db.id).Count();
            
            if (searchName > 0)
            {
                ModelState.AddModelError("db.id_item", "existed");
                ModelState.AddModelError("db.id_specification", "existed");
            }
            //Phúc.Trần thêm 4/8/2022
            if (string.IsNullOrWhiteSpace(item.db.id_supplier) || item.db.id_supplier == "-1")
            {
                ModelState.AddModelError("db.id_supplier", "required");
            }
            return ModelState.IsValid;
        }

    }
}
