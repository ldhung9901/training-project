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
    public partial class sys_group_userController : BaseAuthenticationController
    {
        public sys_group_user_repo repo;

        public sys_group_userController(IUserService UserService, techproDefautContext context) : base(UserService)
        {
            repo = new sys_group_user_repo(context);
        }

        public IActionResult getListUse()
        {
            var result = repo._context.sys_group_users
                .Where(d => d.status_del == 1).
                 Select(d => new
                 {
                     id = d.id,
                     name = d.name,
                 }).ToList();
            return Json(result);
        }
        public JsonResult getListItem([FromBody] JObject json)
        {
            var model = repo.FindAllItem(json.GetValue("id").ToString()).ToList();
            return Json(model);
        }
        public JsonResult getListRole([FromBody] JObject json)
        {
            var model = repo.FindAllRole(json.GetValue("id").ToString()).ToList();
            return Json(model);
        }

        [HttpPost]
        public async Task<JsonResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_group_user_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.create_by = getUserId();
            model.db.status_del = 1;
            model.db.id = Guid.NewGuid().ToString();
            model.db.create_date = DateTime.Now;
            await repo.insert(model);
            return Json(model);
        }

        [HttpPost]
        public JsonResult edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_group_user_model>(json.GetValue("data").ToString());
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

        public IActionResult getListUser([FromBody] JObject json)
        {
            var search = json["search"].ToString();
            var result = repo._context.users
            .Where(u => u.FirstName.Contains(search)||u.LastName.Contains(search)).
                 Select(d => new
                 {
                     id = d.Id,
                     name = d.FirstName + " " + d.LastName,
                     avatar_path=d.avatar_path,
                     id_department=d.id_department,
                     id_job_title=d.id_job_title,
                     department_name = repo._context.sys_departments.Where(t => t.id == d.id_department).Select(t => t.name).FirstOrDefault(),
                     job_title_name = repo._context.sys_job_titles.Where(t=>t.id==d.id_job_title).Select(t=>t.name).FirstOrDefault(),
                 }).ToList();
            return Json(result);
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
                     //.Where(d=>d.db.status_del==1)
                     .Where(d => d.db.name.Contains(search) || d.db.note.Contains(search))
                     ;
                var status_del = int.Parse(dictionary["status_del"]);
                query = query.Where(d => d.db.status_del == status_del);

                var count = query.Count();
                var dataList = await Task.Run(() => query.Skip((param.pageIndex - 1) * param.pageSize).Take(param.pageSize)
                .OrderByDescending(d => d.db.create_date).ToList());
                DTResult<sys_group_user_model> result = new DTResult<sys_group_user_model>
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
