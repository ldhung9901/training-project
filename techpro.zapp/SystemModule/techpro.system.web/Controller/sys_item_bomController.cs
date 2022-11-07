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
using Microsoft.EntityFrameworkCore;

namespace techpro.system.web.Controller
{
    public partial class sys_item_bomController : BaseAuthenticationController
    {
        private sys_item_bom_repo repo;

        public sys_item_bomController(IUserService UserService, techproDefautContext context) : base(UserService)
        {
            repo = new sys_item_bom_repo(context);
        }


        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_item_bom_model>(json.GetValue("data").ToString());

            model.db.create_by = getUserId();
            model.db.create_date = DateTime.Now;
            model.db.status_del = 1;
            await repo.insert(model);
            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> create_config([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_item_bom_config_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreateItemBomConfig(model);
            if (!check)
            {
                return generateError();
            }

            model.db.create_by = getUserId();
            model.db.create_date = DateTime.Now;
            model.db.status_del = 1;
            await repo.insert_config(model);
            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> create_quality([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_item_bom_quality_model>(json.GetValue("data").ToString());


            model.db.create_by = getUserId();
            model.db.create_date = DateTime.Now;
            model.db.status_del = 1;
            await repo.insert_quality(model);
            return Json(model);
        }


        [HttpPost]
        public JsonResult edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_item_bom_model>(json.GetValue("data").ToString());
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
        [HttpPost]
        public JsonResult edit_config([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_item_bom_config_model>(json.GetValue("data").ToString());
            var check = checkModelStateEditItemBomConfig(model);
            if (!check)
            {
                return generateError();
            }
            model.db.update_by = getUserId();
            model.db.update_date = DateTime.Now;
            repo.update_config(model);
            return Json(model);
        }


        public JsonResult delete_quality([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.delete_quality(id);
            return Json("");
        }

        public JsonResult delete_bom_config([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.delete_bom_config(id, getUserId());
            return Json("");
        }

        public JsonResult delete([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.delete(id);
            return Json("");
        }

        public IActionResult get_bom_config(string id_item)
        {
           
            var result = repo._context.sys_item_bom_configs
                 .Where(d => d.id_item == id_item)
                 .Where(d => d.status_del != 2)
                 .Select(d => new
                 {
                     id = d.id,
                     name = d.name,
                     unit_name = repo._context.sys_units.Where(t => t.id == d.id_unit)
                     .Select(t => t.name).SingleOrDefault(),
                     specification_name = repo._context.sys_item_specifications
                     .Where(t => t.id == d.id_specification).Select(t => t.name).SingleOrDefault(),
                 }).ToList();
            return Json(result);
        }
        [HttpGet]
        public IActionResult get_list_quality(string id_item, long id_item_bom_config)
        {
            var result = repo.FindAllQuality().Where(d => d.db.id_item == id_item && d.db.id_item_bom_config == id_item_bom_config).ToList();
            return Json(result);
        }
        [HttpGet]
        public IActionResult get_item_quota(string id_item, long id_item_bom_config)
        {
            var result = repo.FindAll().Where(d => d.db.id_item == id_item && d.db.id_item_bom_config == id_item_bom_config).ToList();
            return Json(result);
        }
        public IActionResult getListUse([FromBody] JObject json)
        {
            var id_item = "";
            long? id_specification = 0;
            id_item = json["id_item"].ToString();
            try
            {
                id_specification = long.Parse(json["id_specification"].ToString());
            }
            catch { }
            var query = repo._context.sys_item_bom_configs.Where(d => d.id_item == id_item)
                  .Where(d => d.status_del != 2);
            if (id_specification != 0)
            {
                query = query.Where(d => d.id_specification == id_specification);
            }
            var result = query.Select(d => new
            {
                id = d.id,
                name = d.name,
                unit_name = repo._context.sys_units.Where(t => t.id == d.id_unit)
                     .Select(t => t.name).SingleOrDefault(),
            }).ToList();
            return Json(result);
        }

        

        public JsonResult get_bom_tree([FromBody] JObject json)
        {
            var id_item_parent = (json.GetValue("id_item_parent").ToString());
            var result = new List<sys_item_bom_model>();
            repo.get_bom_tree(result, id_item_parent, "1", 1);
            return Json(result);
        }



        public async Task<IActionResult> getElementById(string id)
        {
            var model = await repo.getElementById(id);
            return Json(model);
        }

        [HttpGet]
        public  async Task<IActionResult> getAll()
        {
            var result = await repo.FindAllConfig().Where(d => d.db.status_del == 1).ToListAsync();
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
                var id_item_bom_config = long.Parse(dictionary["id_item_bom_config"]);
                var search = dictionary["search"];
                var query = repo.FindAll()
                       .Where(d => d.db.id_item == id_item)
                         .Where(d => d.db.id_item_bom_config == id_item_bom_config)
                     .Where(d => d.sys_item_bom_name.Contains(search))
                     .Where(d => d.db.status_del == 1)

                     ;
                var count = query.Count();
                var dataList = await Task.Run(() => query.Skip((param.pageIndex - 1) * param.pageSize).Take(param.pageSize)
                .OrderByDescending(d => d.db.create_date).ToList());
                DTResult<sys_item_bom_model> result = new DTResult<sys_item_bom_model>
                {
                    start = (param.pageIndex - 1) * param.pageSize,
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
