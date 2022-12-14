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
    public partial class sys_vendor_itemController : BaseAuthenticationController
    {
        private sys_vendor_item_repo repo;

        public sys_vendor_itemController(IUserService UserService, techproDefautContext context) : base(UserService)
        {
            repo = new sys_vendor_item_repo(context);
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {
            var modelHistory = JsonConvert.DeserializeObject<sys_vendor_item_history_model>(json.GetValue("data").ToString());

            var model = JsonConvert.DeserializeObject<sys_vendor_item_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.create_by = getUserId();
            model.db.id = 0;
            model.db.create_date = DateTime.Now;
            model.db.update_date = DateTime.Now;
            modelHistory.db.create_by = getUserId();
          
            modelHistory.db.create_date = DateTime.Now;
            modelHistory.db.update_date = DateTime.Now;

            await repo.insert(model,modelHistory);
            return Json(model);
        }




        [HttpPost]
        public JsonResult edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_vendor_item_model>(json.GetValue("data").ToString());
            var modelHistory = JsonConvert.DeserializeObject<sys_vendor_item_history_model>(json.GetValue("data").ToString());
            modelHistory.db.id = 0;
            modelHistory.db.action = "edit";
            var check = checkModelStateEdit(model);
            if (!check)
            {
                return generateError();
            }
            model.db.update_by = getUserId();
            model.db.update_date = DateTime.Now;
            modelHistory.db.update_by = getUserId();
            modelHistory.db.update_date = DateTime.Now;
             repo.update(model, modelHistory);
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


        [HttpPost]

        public async Task<IActionResult> DataHandler([FromBody] JObject json)
        {
            try
            {
                var a = Request;
                var param = JsonConvert.DeserializeObject<AntTableParams>(json.GetValue("param1").ToString());
                var dictionary = new Dictionary<string, string>();
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.GetValue("data").ToString());
                var id_supplier = dictionary["id_supplier"];
               
                var search = dictionary["search"];
                var query = repo.FindAll()
                    .Where(d => d.sys_item_name.Contains(search) || d.sys_item_specification_name.Contains(search))
                     ;
                if (id_supplier != "-1")
                {
                    query = query.Where(d => d.db.id_supplier == id_supplier);
                }
                var count = query.Count();
                var dataList = await Task.Run(() => query.Skip((param.pageIndex - 1) * param.pageSize).Take(param.pageSize)
                .OrderByDescending(d => d.db.create_date).ToList());
                DTResult<sys_vendor_item_model> result = new DTResult<sys_vendor_item_model>
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
        [HttpPost]
        public async Task<IActionResult> DataHandlerHistory([FromBody] JObject json)
        {
            try
            {
                var a = Request;
                var param = JsonConvert.DeserializeObject<AntTableParams>(json.GetValue("param1").ToString());
                var dictionary = new Dictionary<string, string>();
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.GetValue("data").ToString());
                var id_supplier = dictionary["id_supplier"];
                //var id_position = dictionary["id_position"];

                var query = repo.FindAllHistory().Where(d=> d.db.id_supplier == id_supplier);
                var count = query.Count();
                var dataList = await Task.Run(() => query
                .OrderByDescending(d => d.db.update_date).ToList());
                DTResult<sys_vendor_item_history_model> result = new DTResult<sys_vendor_item_history_model>
                {
                    
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
