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
using techpro.common.Common;
using Microsoft.Extensions.DependencyInjection;

namespace techpro.system.web.Controller
{
    public partial class sys_itemController : BaseAuthenticationController
    {
        private sys_item_repo repo;

        public sys_itemController(IUserService UserService, techproDefautContext context, IServiceScopeFactory serviceFactory) : base(UserService)
        {
            repo = new sys_item_repo(context, serviceFactory);
        }


        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_item_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.create_by = getUserId();
            model.db.id = Guid.NewGuid().ToString();
            model.db.has_bom = 0;
            model.db.has_specification = 0;
            model.db.create_date = DateTime.Now;
            await repo.insert(model);
            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> create_item([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_item_model>(json.GetValue("data").ToString());

            model.db.create_by = getUserId();
            model.db.id = Guid.NewGuid().ToString();
            model.db.has_bom = 0;
            model.db.has_specification = 0;
            model.db.create_date = DateTime.Now;
            await repo.insert(model);
            return Json(model);
        }

        public IActionResult getListUse([FromBody] JObject json)
        {
            var search = json.GetValue("search").ToString().ToLower();
            var ignoreIds = new List<string>();
            var result = repo._context.sys_items
                  .Where(d => d.status_del == 1)
                .Where(t => t.name.ToLower().Contains(search))
                 //.Where(t=>!ignoreIds.Contains(t.id))
                 .Select(d => new
                 {
                     id = d.id,
                     name = d.name,
                     id_unit = d.id_unit,
                     unit_name = repo._context.sys_units.Where(t => t.id == d.id_unit).Select(t => t.name).SingleOrDefault(),
                     code_item = d.code_item
                 }).Take(20).ToList();
            return Json(result);
        }
        [HttpPost]
        public IActionResult get_item_exist([FromBody] JObject json)
        {

            var list_item = JsonConvert.DeserializeObject<List<string>>(json.GetValue("list_item").ToString());

            var rs = repo._context.sys_items.Where(d => d.status_del == 1 && list_item.Contains(d.code_item)).Select(t => new
            {
                id = t.id,
                name = t.name,
                code_item = t.code_item,
                id_unit = t.id_unit,
                unit_name = repo._context.sys_units.Where(d => d.status_del == 1 && t.id_unit == d.id).FirstOrDefault().name
            }).ToList();

            return Json(rs);
        }

        public IActionResult getListUseOtherUnit([FromBody] JObject json)
        {
            var id_item = "";
            try
            {
                id_item = json["id_item"].ToString();
            }
            catch { }
            var result = repo._context.sys_item_unit_others
                .Where(t => t.id_item == id_item)
                 .Select(d => new
                 {
                     id = d.id_unit,
                     name = repo._context.sys_units.Where(t => t.id == d.id_unit).Select(t => t.name).SingleOrDefault(),
                     conversion_factor = d.conversion_factor
                 }).ToList();
            return Json(result);
        }
        
        
        [HttpPost]
        public JsonResult edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_item_model>(json.GetValue("data").ToString());
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
        public JsonResult delete_item([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.delete_item(id, getUserId());
            return Json("");
        }


        public async Task<IActionResult> getElementById(string id)
        {
            var model = await repo.getElementById(id);
            return Json(model);
        }
        public JsonResult getListUnitOther([FromBody] JObject json)
        {
            var model = repo.FindAllUnitOther(json.GetValue("id").ToString()).ToList();
            return Json(model);
        }
        public JsonResult getListMinMaxStock([FromBody] JObject json)
        {
            var model = repo.FindAllMinMaxStock(json.GetValue("id").ToString()).ToList();
            return Json(model);
        }
        public JsonResult getListQuanlity([FromBody] JObject json)
        {
            var model = repo.FindAllQuanlity(json.GetValue("id").ToString()).ToList();
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

                var status_del = int.Parse(dictionary["status_del"]);
                var id_item_type = dictionary["id_item_type"];
                var search = dictionary["search"];
                var query = repo.FindAll()
                     //.Where(d=>d.db.status_del==1)
                     .Where(d => d.db.name.Contains(search) || d.sys_cost_type_name.Contains(search) || d.sys_item_type_name.Contains(search))
                     ;

                query = query.Where(d => d.db.status_del == status_del);

                if (!string.IsNullOrWhiteSpace(id_item_type) && id_item_type != "-1")
                {
                    query = query.Where(d => d.db.id_item_type == id_item_type);

                }

                var count = query.Count();
                var dataList = await Task.Run(() => query.Skip((param.pageIndex - 1) * param.pageSize).Take(param.pageSize)
                .OrderByDescending(d => d.db.create_date).ToList());
                DTResult<sys_item_model> result = new DTResult<sys_item_model>
                {
                    data = dataList,
                    recordsFiltered = count,
                    recordsTotal = count,
                    start = (param.pageIndex - 1) * param.pageSize,
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
