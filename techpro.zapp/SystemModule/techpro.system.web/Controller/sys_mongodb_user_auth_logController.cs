using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using techpro.common.BaseClass;
using techpro.common.common;
using techpro.common.Services;
using techpro.DataBase.mongodb.log;
using techpro.DataBase.Provider;
using techpro.system.data.DataAccess;
using techpro.system.data.Models;

namespace techpro.system.web.Controller
{
    public partial class sys_mongodb_user_auth_logController : BaseAuthenticationController
    {
        public sys_mongodb_user_auth_log_repo repo;

        public sys_mongodb_user_auth_logController(IUserService UserService, MongodbDefautContext context) : base(UserService)
        {
            repo = new sys_mongodb_user_auth_log_repo(context);
        }


        [HttpPost]
        public async Task<JsonResult> DataHandler([FromBody] JObject json)
        {


            try
            {
                var a = Request;
                var param = JsonConvert.DeserializeObject<AntTableParams>(json.GetValue("param1").ToString());
                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json.GetValue("data").ToString());

                string search = dictionary["search"];
               
                // var to_date = DateTime.Parse(dictionary["to_date"].ToString());
                //var end_date = DateTime.Parse(dictionary["end_date"].ToString());
                var query = repo.FindAll().Where(t => t.account.Contains(search) || t.user_name.Contains(search));
                //query = query.Where(t => to_date.Date <= t.login_time && t.login_time  <= end_date.Date).OrderByDescending(d => d.login_time);
                var dataList = await Task.Run(() => query.Skip((param.pageIndex - 1) * param.pageSize).Take(param.pageSize).ToList());
                if (dictionary.ContainsKey("range"))
                {
                    DateTime startDate = dictionary["range"]["start"];
                    DateTime endDate = dictionary["range"]["end"];
                    query = query.Where(d => d.login_time >= startDate.Date && d.login_time <= endDate.Date);
                }
                var count = query.Count();
                DTResult<mongodb_user_auth_log_db> result = new DTResult<mongodb_user_auth_log_db>
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

        [HttpPost]
        public async Task<JsonResult> DataHandlerDetail([FromBody] JObject json)
        {


            try
            {
                var a = Request;
                var param = JsonConvert.DeserializeObject<AntTableParams>(json.GetValue("param1").ToString());
                var dictionary = new Dictionary<string, string>();
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.GetValue("data").ToString());

                var search = dictionary["search"];
                var id = dictionary["id_user_auth_log"];
                var controller_name = dictionary["controller_name"];

                var query = repo.FindAllDetail();
                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(t => t.request_work.ToUpper().Contains(search.ToUpper()));

                }
                if (dictionary.ContainsKey("to_date") && dictionary.ContainsKey("end_date"))
                {
                    var to_date = DateTime.Parse(dictionary["to_date"].ToString()).Date;
                    var end_date = DateTime.Parse(dictionary["end_date"].ToString()).AddDays(1).Date;
                    query = query.Where(t => to_date <= t.request_time.Value && t.request_time.Value <= end_date);
                }

                if (!string.IsNullOrWhiteSpace(id))
                {
                    query = query.Where(t => t.id_user_auth_log == id);

                }
                if (!string.IsNullOrWhiteSpace(controller_name))
                {
                    query = query.Where(t => t.controller_name == controller_name);
                }

                query = query.OrderByDescending(t => t.request_time);

                var count = query.Count();
                var dataList = await Task.Run(() => query.Skip((param.pageIndex - 1) * param.pageSize).Take(param.pageSize).ToList());

                DTResult<mongodb_user_log_detail_db> result = new DTResult<mongodb_user_log_detail_db>
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

        [HttpPost]
        public async Task<JsonResult> create_detail([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<mongodb_user_log_detail_db>(json.GetValue("data").ToString());


            Request.Headers.TryGetValue("id_user_auth_log", out var id_user_auth_log);
            model.id_user_auth_log = id_user_auth_log;
            model.request_time = DateTime.Now;
            model.create_date = DateTime.Now;
            model.account = User.Claims.Where(d => d.Type == ClaimTypes.WindowsAccountName).Select(d => d.Value).FirstOrDefault();
            model.user_name = User.Claims.Where(d => d.Type == ClaimTypes.NameIdentifier).Select(d => d.Value).FirstOrDefault();
            model.user_id = User.Identity.Name;
            model.device_ip_address = User.Claims.Where(d => d.Type == ClaimTypes.WindowsDeviceClaim).Select(d => d.Value).FirstOrDefault();
            model.device_name = User.Claims.Where(d => d.Type == ClaimTypes.WindowsDeviceGroup).Select(d => d.Value).FirstOrDefault();


            await repo.insert(model);
            return Json(model);
        }

    }
}
