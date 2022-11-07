using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using techpro.common.BaseClass;
using techpro.common.common;
using techpro.common.Services;
using techpro.system.data.DataAccess;
using techpro.system.data.Models;
using techpro.DataBase.Provider;

namespace techpro.system.web.Controller
{
    public partial class sys_factory_line_item_capacityController : BaseAuthenticationController
    {
        private sys_factory_line_item_capacity_repo repo;

        public sys_factory_line_item_capacityController(IUserService UserService, techproDefautContext context) : base(UserService)
        {
            repo = new sys_factory_line_item_capacity_repo(context);
        }
       
        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            sys_factory_line_item_capacity_model model = JsonConvert.DeserializeObject<sys_factory_line_item_capacity_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.create_by = getUserId();
            model.db.id = 0;
            model.db.create_date = DateTime.Now;
            await repo.insert(model);
            return Json(model);
        }

      


        [HttpPost]
        public JsonResult edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_factory_line_item_capacity_model>(json.GetValue("data").ToString());
            var check = checkModelStateEdit(model);
            if (!check)
            {
                return generateError();
            }
            model.db.update_by = getUserId();
            model.db.update_date = DateTime.Now;
            repo.update(model);
            return Json(model);
        }


        public JsonResult delete([FromBody] JObject json)
        {
            var id = long.Parse(json.GetValue("id").ToString());
            repo.delete(id, getUserId());
            return Json("");
        }


        public async Task<IActionResult> getElementById(long id)
        {
            var model = await repo.getElementById(id);
            return Json(model);
        }
        public async Task<IActionResult> getElementByIdItem(string id_workstation, string id_item)
        {
            var id_sys_factory_line = repo._context.sys_workstations.Where(d => d.id == id_workstation).Select(d => d.id_sys_factory_line).First();
            var model = await repo.getElementByIdItem(id_sys_factory_line,id_item);
            return Json(model);
        }


        [HttpPost]

        public async Task<IActionResult> DataHandler([FromBody] JObject json)
        {
            try
            {
                var a = Request;
                var param = JsonConvert.DeserializeObject<AntTableParams>(json.GetValue("param1").ToString());
                var dictionary = new Dictionary<string, string>();
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.GetValue("data").ToString());
                var id_sys_factory = dictionary["id_sys_factory"];
                var id_sys_factory_line = dictionary["id_sys_factory_line"];
                var search = dictionary["search"];
                var query = repo.FindAll()
                    .Where(d => d.db.id_sys_factory == id_sys_factory)
                    .Where(d=>d.db.id_sys_factory_line == id_sys_factory_line)
                    .Where(d => d.sys_item_name.Contains(search) || d.sys_item_specification_name.Contains(search))
                     ;
                var count = query.Count();
                var dataList = await Task.Run(() => query.Skip((param.pageIndex - 1) * param.pageSize).Take(param.pageSize)
                .OrderByDescending(d => d.db.create_date).ToList());
                DTResult<sys_factory_line_item_capacity_model> result = new DTResult<sys_factory_line_item_capacity_model>
                {
                    start =(param.pageIndex - 1) * param.pageSize,
                    data = dataList,
                    recordsFiltered = count,
                    recordsTotal = count
                };
                return Json(result);
            }

            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }

        }

    }
}
