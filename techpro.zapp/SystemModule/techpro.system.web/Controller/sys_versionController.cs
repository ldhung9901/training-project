using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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
    public partial class sys_versionController : BaseAuthenticationController
    {
        private sys_version_repo repo;

        public sys_versionController(IUserService UserService, techproDefautContext context) : base(UserService)
        {
            repo = new sys_version_repo(context);
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_version_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
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
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_version_model>(json.GetValue("data").ToString());
            var check = checkModelStateEdit(model);
            if (!check)
            {
                return generateError();
            }
            model.db.update_by = getUserId();
            model.db.update_date = DateTime.Now;
            await repo.update(model);
            return Json(model);
        }

        [HttpGet]
        public IActionResult getAll()
        {
            var result = repo.FindAll();
            return Json(result);
        }
        public async Task<IActionResult> delete([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.delete(id, getUserId());
            return Json("");
        }
        public async Task<IActionResult> revert([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.revert(id, getUserId());
            return Json("");
        }


        public async Task<IActionResult> getElementById(string id)
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
                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json.GetValue("data").ToString());
              
                string search = dictionary["search"];
                var query = repo.FindAllRange()
                .Where(d => d.db.version.Contains(search));

                // var status_del = int.Parse(dictionary["status_del"]);
                // query = query.Where(d => d.db.status_del == status_del);
                if (dictionary.ContainsKey("range"))
                {
                    DateTime startDate = dictionary["range"]["start"];
                    DateTime endDate = dictionary["range"]["end"];
                    query = query.Where(d => d.db.release_day.Value.Date >= startDate.Date && d.db.release_day.Value.Date <= endDate.Date);
                }
                var count = query.Count();
                var dataList = await Task.Run(() => query.Skip((param.pageIndex - 1) * param.pageSize).Take(param.pageSize)
                .OrderByDescending(d => d.db.create_date).ToList());
                DTResult<sys_version_model> result = new DTResult<sys_version_model>
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
