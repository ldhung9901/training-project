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
   public partial class sys_work_schedule_factory_lineController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_work_schedule_factory_line",
            // icon = "menu",
            module = "system",
            id = "sys_work_schedule_factory_line",
            url = "/sys_work_schedule_factory_line_index",
            title = "sys_work_schedule_factory_line",
            translate = "NAV.sys_work_schedule_factory_line",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_work_schedule_factory_line;getListUse"

            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_work_schedule_factory_line;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_work_schedule_factory_line;create",
                    }
                },
                  new ControllerRoleModel()
                  {
                      id="sys_work_schedule_factory_line;edit",
                      name="edit",
                      list_controller_action = new List<string>()
                      {
                            "sys_work_schedule_factory_line;edit",
                      }
                  },
                //new ControllerRoleModel()
                //{
                //    id="sys_work_schedule_factory_line;delete",
                //    name="delete",
                //    list_controller_action = new List<string>()
                //    {
                //          "sys_work_schedule_factory_line;delete",
                //    }
                //},
                  new ControllerRoleModel()
                {
                    id="sys_work_schedule_factory_line;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_work_schedule_factory_line;DataHandler",
                    }
                }
            }
        };
        public bool checkModelStateCreate(List<sys_work_schedule_factory_line_model> lst_item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, lst_item);
        }

        public bool checkModelStateEdit(List<sys_work_schedule_factory_line_model> lst_item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, lst_item);
        }
        public bool checkModelStateCreateEdit(ActionEnumForm action, List<sys_work_schedule_factory_line_model> lst_item)
        {
            //for (int i = 0; i < lst_item.Count; i++)
            //{
            //    var itemNew = lst_item[i];
            //    if (itemNew.db.timeStart_1 == null)
            //    {
            //        ModelState.AddModelError("timeStart_1_" + i, "required");
            //    }

            //    if (itemNew.db.timeEnd_1 == null)
            //    {
            //        ModelState.AddModelError("timeEnd_1_" + i, "required");
            //    }

            //    if (itemNew.db.timeStart_1 != null && itemNew.db.timeEnd_1 != null)
            //    {
            //        if (itemNew.db.timeStart_1 > itemNew.db.timeEnd_1)
            //        {
            //            ModelState.AddModelError("db.timeStart_1_" + i, "msgdb.timeStart_1PhaiNhoHondb.timeEnd_1");
            //        }

            //    }
            //    if (itemNew.db.timeStart_2 != null && itemNew.db.timeEnd_2 != null)
            //    {
            //        if (itemNew.db.timeStart_1 > itemNew.db.timeEnd_1)
            //            ModelState.AddModelError("db.timeStart_2_" + i, "msgdb.timeStart_2PhaiNhoHondb.timeEnd_2");
            //    }
            //    if (itemNew.db.timeEnd_1 != null && itemNew.db.timeStart_2 != null)
            //    {
            //        if (itemNew.db.timeEnd_1 > itemNew.db.timeStart_2)
            //            ModelState.AddModelError("db.timeStart_2_" + i, "msgdb.timeStart_2PhaiLonHondb.timeEnd_1");
            //    }

            //}


            return ModelState.IsValid;
        }

    }
}
