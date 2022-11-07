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
    public partial class sys_customerController : BaseAuthenticationController
    {
        private sys_customer_repo repo;

        public sys_customerController(IUserService UserService,techproDefautContext context) : base(UserService)
        {
            repo = new sys_customer_repo(context);
        }

        public IActionResult getListUse([FromBody] JObject json)
        {
            var search = json.GetValue("search").ToString().ToLower();
            var ignoreIds = new List<string>();
            // if (string.IsNullOrWhiteSpace(search)) return Json(new List<string>());
            var result = repo._context.sys_customers
                 .Where(d => d.status_del == 1)
                     .Where(t => t.name.ToLower().Contains(search))
                //.Where(t => !ignoreIds.Contains(t.id))
                 .Select(d => new
                 {
                     id = d.id,
                     name = d.name,
                     db=d
                 }).ToList();
            return Json(result);
        }
        public IActionResult getListUseCustomner([FromBody] JObject json)
        {
            var search = json.GetValue("search").ToString().ToLower();
            var result = repo._context.sys_customers
                 .Where(d => d.is_customer == true)
                 .Where(d => d.status_del == 1)
                     .Where(t => t.name.ToLower().Contains(search))
                 .Select(d => new
                 {
                     id = d.id,
                     name = d.name,
                     db = d
                 }).ToList();
            return Json(result);
        }
        public IActionResult getListUseSupplier([FromBody] JObject json)
        {
            var search = json.GetValue("search").ToString().ToLower();
            var result = repo._context.sys_customers
                .Where(d=>d.is_supplier == true)
                 .Where(d => d.status_del == 1)
                     .Where(t => t.name.ToLower().Contains(search))
                 .Select(d => new
                 {
                     id = d.id,
                     name = d.name,
                     db = d
                 }).ToList();
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {
          
            var model = JsonConvert.DeserializeObject<sys_customer_model>(json.GetValue("data").ToString());
            var check= checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.status_del = 1;
            model.db.create_by = getUserId();
            model.db.id = Guid.NewGuid().ToString();
            model.db.create_date = DateTime.Now;
            await repo.insert(model);
            return Json(model);
        }

        [HttpPost]
        public JsonResult edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_customer_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
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


        public async Task<IActionResult> getElementById([FromBody] JObject json)
        {
            var model = await repo.getElementById(json.GetValue("id").ToString());
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
               
                var search = dictionary["search"];
                var customer_type = dictionary["customer_type"];
                var query = repo.FindAll()
                    //.Where(d=>d.db.status_del ==1)
                     .Where(d => d.db.name.Contains(search) || d.db.tax_number.Contains(search))
                     ;
                var status_del = int.Parse(dictionary["status_del"]);
                query = query.Where(d => d.db.status_del == status_del);

                if (customer_type == "1")
                {
                    query = query.Where(d => d.db.is_customer == true);
                }  
                if (customer_type == "2")
                {
                    query = query.Where(d => d.db.is_supplier == true);
                }
                var count = query.Count();
                var dataList = await Task.Run(() => query.Skip((param.pageIndex - 1) * param.pageSize).Take(param.pageSize)
                .OrderByDescending(d=>d.db.create_date).ToList());
                DTResult<sys_customer_model> result = new DTResult<sys_customer_model>
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
