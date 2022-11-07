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
    public partial class sys_unitController : BaseAuthenticationController
    {
        private sys_unit_repo repo;

        public sys_unitController(IUserService UserService,techproDefautContext context) : base(UserService)
        {
            repo = new sys_unit_repo(context);
        }

        public IActionResult getListUse()
        {
            var result = repo._context.sys_units
                    .Where(d => d.status_del == 1)
                 .Select(d => new
                 {
                     id = d.id,
                     name = d.name
                 }).ToList();
            return Json(result);
        }
        [HttpPost]
        public async Task<JsonResult> create([FromBody] JObject json)
        {
          
            var model = JsonConvert.DeserializeObject<sys_unit_model>(json.GetValue("data").ToString());
            var check= checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.create_by = getUserId();
            model.db.id = Guid.NewGuid().ToString();
            model.db.create_date = DateTime.Now;
            model.db.status_del = 1;
            await repo.insert(model);
            return Json(model);
        }

        [HttpPost]
        public JsonResult edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_unit_model>(json.GetValue("data").ToString());
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


        public async Task<JsonResult> getElementById(string id)
        {
            var model = await repo.getElementById(id);
            return Json(model);
        }

       
        [HttpPost]

        public async Task<JsonResult> DataHandler([FromBody] JObject json)
        {
            try
            {
                var a = Request;
                var param = JsonConvert.DeserializeObject<AntTableParams>(json.GetValue("param1").ToString());
                var dictionary = new Dictionary<string, string>();
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.GetValue("data").ToString());
               
                var search = dictionary["search"];
                var query = repo.FindAll()
                      //.Where(d => d.db.status_del == 1)
                     .Where(d => d.db.name.Contains(search))
                     ;
                var status_del = int.Parse(dictionary["status_del"]);
                query = query.Where(d => d.db.status_del == status_del);

                var count = query.Count();
                var dataList = await Task.Run(() => query.Skip((param.pageIndex - 1) * param.pageSize).Take(param.pageSize)
                .OrderByDescending(d=>d.db.create_date).ToList());
                DTResult<sys_unit_model> result = new DTResult<sys_unit_model>
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
