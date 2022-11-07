using System.Collections.Generic;
using System.Linq;
using techpro.common.BaseClass;
using techpro.common.Models;
using techpro.system.data.Models;

namespace techpro.system.web.Controller
{
    partial class sys_warehouseController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_warehouse",
            // icon = "house",
            module = "system",
            id = "sys_warehouse",
            url = "/sys_warehouse_index",
            title = "sys_warehouse",
            translate = "NAV.sys_warehouse",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_warehouse;getListUse",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_warehouse;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_warehouse;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_warehouse;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_warehouse;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_warehouse;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_warehouse;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_warehouse;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_warehouse;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_warehouse_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_warehouse_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_warehouse_model item)
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
