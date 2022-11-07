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
using System.IO;

namespace techpro.system.web.Controller
{
    public partial class sys_workstationController : BaseAuthenticationController
    {
        private sys_workstation_repo repo;

        public sys_workstationController(IUserService UserService, techproDefautContext context) : base(UserService)
        {
            repo = new sys_workstation_repo(context);
        }


        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_workstation_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.id = Guid.NewGuid().ToString();
            model.db.create_by = getUserId();
            model.db.create_date = DateTime.Now;
            model.db.status_del = 1;
            await repo.insert(model);
            return Json(model);
        }



        [HttpPost]
        public JsonResult edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_workstation_model>(json.GetValue("data").ToString());
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

            var id_factory = "";
            var id_factory_line = "";
            try
            {
                id_factory = json["id_factory"].ToString();
                id_factory_line = json["id_factory_line"].ToString();
            }
            catch { }


            if (id_factory != "" && id_factory_line != "")
            {
                var result = repo._context.sys_workstations
              .Where(t => t.id_sys_factory == id_factory)
               .Where(t => t.id_sys_factory_line == id_factory_line)
               .Select(d => new
               {
                   id = d.id,
                   name = d.name,
               }).ToList();
                return Json(result);
            }
            else
            {
                var result = repo._context.sys_workstations
              .Select(d => new
              {
                  id = d.id,
                  name = d.name,
              }).ToList();
                return Json(result);
            }

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
                var id_sys_phase = dictionary["id_sys_phase"];
                var search = dictionary["search"];
                var query = repo.FindAll()
                     .Where(d => d.db.name.Contains(search))
                      .Where(d => d.db.id_sys_factory == id_sys_factory)
                       .Where(d => d.db.id_sys_factory_line == id_sys_factory_line)
                     ;
                    
                var status_del = int.Parse(dictionary["status_del"]);
                query = query.Where(d => d.db.status_del == status_del);
                if (id_sys_phase != "-1")
                {
                query = query.Where(d => d.db.id_sys_phase == id_sys_phase);
                }
                
                var count = query.Count();
                var dataList = await Task.Run(() => query.Skip((param.pageIndex - 1) * param.pageSize).Take(param.pageSize)
                .OrderByDescending(d => d.db.create_date).ToList());
                DTResult<sys_workstation_model> result = new DTResult<sys_workstation_model>
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
