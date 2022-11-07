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
    public partial class sys_configurationController : BaseAuthenticationController
    {
        public sys_configuration_repo repo;

        public sys_configurationController(IUserService UserService, techproDefautContext context) : base(UserService)
        {
            repo = new sys_configuration_repo(context);
        }

        public async Task<IActionResult> getElementById(string id)
        {
            var model = await repo.getElementById(id);
            return Json(model);
        }
        public IActionResult get_List_license()
        {
            var result = repo.Find_license().Where(d => d.status_del ==1).ToList();
            return Json(result);
        }
        
        [HttpPost]
        public JsonResult edit_company([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_configuration_model>(json.GetValue("data").ToString());
            var check = checkModelStateEdit(model);
            if (!check)
            {
                return generateError();
            }
            model.db_company.update_by = getUserId();
            model.db_company.update_date = DateTime.Now;
            repo.update(model);
            return Json(model);
        }

       [HttpPost]
        public async Task<IActionResult> create_license([FromBody] JObject json)
        {
             var model = JsonConvert.DeserializeObject<sys_configuration_model_v2>(json.GetValue("data").ToString());
           
            await repo.insert_license(model,getUserId());
            return Json(model);
           
        }

        [HttpPost]
        public async Task<JsonResult> create_working_time([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_working_time_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.status_del = 1;
            model.db.id = Guid.NewGuid().ToString();
            model.db.create_by = getUserId();
            model.db.create_date = DateTime.Now;
            await repo.insert_time(model);
    
            return Json(model);
        }

        [HttpPost]
        public async Task<JsonResult> edit_working_time([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_working_time_model>(json.GetValue("data").ToString());
            var check = checkModelStateEdit(model);
            if (!check)
            {
                return generateError();
            }

            await repo.update_working_time(model);

            return Json(model);
        }

        public IActionResult get_list_working_time()
        {
            var result = repo.find_working_time().Where(d => d.db.status_del == 1);
            
            return Json(result);
        }

        public async Task<IActionResult> getListItem([FromBody] JObject json){
            var model = repo.FindAllItem(json.GetValue("id").ToString()).ToList();
            return Json(model);
        }

        public async Task<IActionResult> delete_working_time([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.delete_working_time(id, getUserId());
            var data = repo._context.sys_working_time_configs.Where(x => x.id == id).FirstOrDefault();
            return Json("");
        }
    }
}
