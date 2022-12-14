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
    public partial class sys_item_specificationController : BaseAuthenticationController
    {
        private sys_item_specification_repo repo;

        public sys_item_specificationController(IUserService UserService, techproDefautContext context) : base(UserService)
        {
            repo = new sys_item_specification_repo(context);
        }

        [HttpPost]
        public IActionResult get_item_specification([FromBody] JObject json)
        {
            var list_item = JsonConvert.DeserializeObject<List<string>>(json.GetValue("list_item").ToString());

            var check = repo._context.sys_item_specifications.Where(d => d.status_del == 1 && list_item.ToList().Contains(d.id_item)).Select(t=>new
            {
                id=t.id,
                name=t.name,
                code_item= repo._context.sys_items.Where(t1=>t1.id==t.id_item).FirstOrDefault().code_item,
            });

            return Json(check);
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_item_specification_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.create_by = getUserId();
            model.db.create_date = DateTime.Now;
            model.db.status_del = 1;
            await repo.insert(model);
            return Json(model);
        }

        [HttpPost]
        public JsonResult edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_item_specification_model>(json.GetValue("data").ToString());
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
            var id = json.GetValue("id").ToString();
            repo.delete(id, getUserId());
            return Json("");
        }


        public async Task<IActionResult> getElementById(string id)
        {
            var model = await repo.getElementById(id);
            return Json(model);
        }
        public IActionResult getListUse([FromBody] JObject json)
        {
            var search = json.GetValue("search").ToString().ToLower();
           
            var ignoreIds = new List<long>();
            var id_item = "";
            if (string.IsNullOrWhiteSpace(search)) return Json(new List<string>());
            try
            {
                id_item = json["data"]["id_item"].ToString();
                //ignoreIds = json["data"]["ignoreIds"].ToString().Split(";")
                //    .Where(d=>!string.IsNullOrWhiteSpace(d))
                //    .Select(d=>long.Parse(d)).ToList();
            }
            catch { }
            var result = repo._context.sys_item_specifications
                 .Where(t => t.status_del == 1)
                .Where(t=>t.id_item == id_item)
                .Where(t => t.name.ToLower().Contains(search))
                .Where(t => !ignoreIds.Contains(t.id))
                 .Select(d => new
                 {
                     id = d.id,
                     name = d.name,
                     conversion_factor = d.conversion_factor,
                     id_unit =  d.id_unit,
                     sys_unit_name= repo._context.sys_units.Where(t => t.id == d.id_unit).Select(t => t.name).SingleOrDefault(),
                 }).Take(10).ToList();
            return Json(result);
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
                var id_item = dictionary["id_item"];
                var search = dictionary["search"];
                var query = repo.FindAll()
                    .Where(d=>d.db.id_item == id_item)
                     .Where(d => d.db.name.Contains(search))
                     ;
                var status_del = int.Parse(dictionary["status_del"]);
                query = query.Where(d => d.db.status_del == status_del);

                var count = query.Count();
                var dataList = await Task.Run(() => query.Skip((param.pageIndex - 1) * param.pageSize).Take(param.pageSize)
                .OrderByDescending(d => d.db.create_date).ToList());
                DTResult<sys_item_specification_model> result = new DTResult<sys_item_specification_model>
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
