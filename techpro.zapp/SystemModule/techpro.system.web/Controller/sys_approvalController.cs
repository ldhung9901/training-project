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
using techpro.DataBase.mongodb.log;
using System.Security.Claims;
using techpro.DataBase.mongodb.notification;
using System.Web;
using techpro.common.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using techpro.common.Helpers;
using Microsoft.EntityFrameworkCore;

namespace techpro.system.web.Controller
{
    public partial class sys_approvalController : BaseAuthenticationController
    {
        public sys_approval_repo repo;
        private sys_mongodb_user_auth_log_repo user_auth_log_repo;
        private INotificationService _notification;
        private IMailService _mailService;
        private IWebHostEnvironment Environment;


        public sys_approvalController(IUserService UserService, techproDefautContext context, MongodbDefautContext mongodb_context, INotificationService notification, IMailService mailService, IWebHostEnvironment _environment, IOptions<AppSettings> appSettings) : base(UserService)
        {

            repo = new sys_approval_repo(context, mailService, _environment, appSettings);
            _mailService = mailService;
            user_auth_log_repo = new sys_mongodb_user_auth_log_repo(mongodb_context);
            _notification = notification;
            Environment = _environment;
        }
        public JsonResult getListItem([FromBody] JObject json)
        {
            var model = repo.FindAllItem(json.GetValue("id").ToString()).ToList();
            return Json(model);
        }

        [HttpPost]
        public async Task<JsonResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_approval_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            var userid = getUserId();
            if (model.db.id == "0")
            {
                model.db.create_by = userid;
                model.db.create_date = DateTime.Now;
                model.db.start_by = getUserId();
                model.db.start_date = DateTime.Now;
                model.db.id = Guid.NewGuid().ToString();
                model.db.last_date_action = DateTime.Now;
                model.db.last_user_action = userid;
                model.db.from_user = userid;
                model.db.step_num = 1;
                model.db.status_action = 1;
                model.db.to_user = repo._context.sys_approval_config_details
                    .Where(d => d.id_approval_config == model.db.id_sys_approval_config)
                    .Where(d => d.step_num == 1)
                    .Select(d => d.user_id).FirstOrDefault();
                model.db.status_finish = 2;
                await repo.insert(model);
            }
            else
            {
                model.db.last_user_action = userid;
                model.db.from_user = userid;
                model.db.status_action = 1;
                model.db.start_by = userid;
                repo.approval(model);
            }
            var result = repo._context.Fn_get_sys_approval(model.db.id).FirstOrDefault();


            var data = repo.FindVoucherInfo(model.db.menu, model);
            string response_data = JsonConvert.SerializeObject(data);

            string voucher_number = data.voucher_data.name;
            string request_work = "Gửi duyệt phiếu: " + data.voucher_detail;


            Request.Headers.TryGetValue("id_user_auth_log", out var id_user_auth_log);

            await _notification.insert_notification(model.db.from_user, new mongodb_notification_db
            {
                id_user = model.db.from_user,
                color = "bg-yellow-50 text-yellow-600",
                content = $"Người dùng {result.from_user_name} đã gửi phiếu {data.voucher_detail} đến {result.to_user_name}",
                create_by = model.db.from_user,
                create_date = DateTime.Now,
                data = null,
                url = $"/{model.db.menu}_index?name={HttpUtility.UrlEncode(data.voucher_data.name)}",
                is_read = false,
                module = model.db.menu,
                status_del = 1,
                title = request_work,
                type = "warning"

            });
            repo.send_mail_await_approval(model.db.to_user, model);

            if (model.db.to_user != null && model.db.to_user != model.db.from_user)
            {
                await _notification.insert_notification(model.db.to_user, new mongodb_notification_db
                {
                    id_user = model.db.to_user,
                    color = "bg-yellow-50 text-yellow-600",
                    content = $"Người dùng {result.from_user_name} đã gửi phiếu {data.voucher_detail} đến {result.to_user_name}",
                    create_by = model.db.to_user,
                    create_date = DateTime.Now,
                    data = null,
                    url = $"/{model.db.menu}_index?name={HttpUtility.UrlEncode(data.voucher_data.name)}",
                    is_read = false,
                    module = model.db.menu,
                    status_del = 1,
                    title = request_work,
                    type = "warning"

                });
            }






            await user_auth_log_repo.insert(new mongodb_user_log_detail_db
            {
                id_user_auth_log = id_user_auth_log,
                controller_name = model.db.menu,
                action_name = "approval",
                request_time = DateTime.Now,
                create_date = DateTime.Now,
                request_work = request_work,
                response_data = response_data,
                voucher_number = voucher_number,
                account = User.Claims.Where(d => d.Type == ClaimTypes.WindowsAccountName).Select(d => d.Value).FirstOrDefault(),
                user_name = User.Claims.Where(d => d.Type == ClaimTypes.NameIdentifier).Select(d => d.Value).FirstOrDefault(),
                user_id = User.Identity.Name,
                device_ip_address = User.Claims.Where(d => d.Type == ClaimTypes.WindowsDeviceClaim).Select(d => d.Value).FirstOrDefault(),
                device_name = User.Claims.Where(d => d.Type == ClaimTypes.WindowsDeviceGroup).Select(d => d.Value).FirstOrDefault(),

            });

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> approval([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_approval_model>(json.ToString());
            var check = checkModelStateApproval(model);
            if (!check)
            {
                return generateError();
            }
            var userid = getUserId();
            model.db.last_user_action = userid;
            model.db.from_user = userid;
            repo.approval(model);
            var result = repo._context.Fn_get_sys_approval(model.db.id).FirstOrDefault();



            string request_work = "";
            string response_data = "";

            var data = repo.FindVoucherInfo(model.db.menu, model); ;


            if (result.status_action == 2)
            {
                request_work = $"Đã duyệt phiếu {data.voucher_detail}";
                await _notification.insert_notification(model.db.from_user, new mongodb_notification_db
                {
                    id_user = model.db.from_user,
                    color = "bg-green-50 text-green-600",
                    content = $"Người dùng {result.from_user_name} đã duyệt phiếu tại bước ( {result.step_name} )( Bước {result.step_num} )",
                    create_by = model.db.from_user,
                    create_date = DateTime.Now,
                    data = null,
                    url = $"/{model.db.menu}_index?name={HttpUtility.UrlEncode(data.voucher_data.name)}",
                    is_read = false,
                    module = model.db.menu,
                    status_del = 1,
                    title = request_work,
                    type = "success"
                });


            }

            if (result.status_action == 3)
            {
                request_work = "Trả lại phiếu: " + data.voucher_detail;
                await _notification.insert_mutiple_notification(new List<string>() { model.db.to_user, model.db.to_user }, new mongodb_notification_db
                {
                    id_user = model.db.from_user,
                    color = "bg-red-50 text-red-600",
                    content = $"Người dùng {result.from_user_name} trả lại phiếu {data.voucher_detail} (Lý do: {result.last_note}) ( {result.step_name} )( Bước {result.step_num} )",
                    create_by = model.db.from_user,
                    create_date = DateTime.Now,
                    data = null,
                    url = $"/{model.db.menu}_index?name={HttpUtility.UrlEncode(data.voucher_data.name)}",
                    is_read = false,
                    module = model.db.menu,
                    status_del = 1,
                    title = request_work,
                    type = "error"
                });
            }


            Request.Headers.TryGetValue("id_user_auth_log", out var id_user_auth_log);


            if (model.db.to_user != null && model.db.to_user != model.db.from_user && result.status_action != 3)
            {
                await _notification.insert_notification(model.db.to_user, new mongodb_notification_db
                {
                    id_user = model.db.to_user,
                    color = "bg-yellow-50 text-yellow-600",
                    content = $"Phiếu {data.voucher_detail} từ người dùng {result.from_user_name} đang chờ {result.to_user_name} duyệt ( {result.step_name} )( Bước {result.step_num} )",
                    create_by = model.db.to_user,
                    create_date = DateTime.Now,
                    data = null,
                    url = $"/{model.db.menu}_index?name={HttpUtility.UrlEncode(data.voucher_data.name)}",
                    is_read = false,
                    module = model.db.menu,
                    status_del = 1,
                    title = request_work,
                    type = "warning"
                });
            }
            if (model.db.status_finish == 3)
            {

                var list_user = repo.FindAllStep(model.db.id).Select(d => d.db.user_id).ToList();
                var begin_user = await repo._context.sys_approval_details.Where(d=> d.id_approval == model.db.id).OrderBy(d => d.date_action).Select(d => d.from_user).FirstOrDefaultAsync();
                if(begin_user !=null ){
                    list_user.Add(begin_user);
                }
                await _notification.insert_mutiple_notification(list_user, new mongodb_notification_db
                {
                   
                    color = "bg-green-50 text-green-600",
                    content = $"Phiếu {data.voucher_detail} từ người dùng {result.from_user_name} đang chờ {result.to_user_name} duyệt ( {result.step_name} )( Bước {result.step_num} )",
                    create_by = model.db.to_user,
                    create_date = DateTime.Now,
                    data = null,
                    url = $"/{model.db.menu}_index?name={HttpUtility.UrlEncode(data.voucher_data.name)}",
                    is_read = false,
                    module = model.db.menu,
                    status_del = 1,
                    title = request_work,
                    type = "success"

                });
                repo.send_mail_approval(begin_user, model);
            }

            await user_auth_log_repo.insert(new mongodb_user_log_detail_db
            {
                id_user_auth_log = id_user_auth_log,
                controller_name = model.db.menu,
                action_name = "approval",
                request_time = DateTime.Now,
                create_date = DateTime.Now,
                request_work = request_work,
                response_data = response_data,
                voucher_number = data.voucher_detail,
                account = User.Claims.Where(d => d.Type == ClaimTypes.WindowsAccountName).Select(d => d.Value).FirstOrDefault(),
                user_name = User.Claims.Where(d => d.Type == ClaimTypes.NameIdentifier).Select(d => d.Value).FirstOrDefault(),
                user_id = User.Identity.Name,
                device_ip_address = User.Claims.Where(d => d.Type == ClaimTypes.WindowsDeviceClaim).Select(d => d.Value).FirstOrDefault(),
                device_name = User.Claims.Where(d => d.Type == ClaimTypes.WindowsDeviceGroup).Select(d => d.Value).FirstOrDefault(),

            });


            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> cancel([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_approval_model>(json.ToString());
            var check = checkModelStateCancel(model);
            if (!check)
            {
                return generateError();
            }
            var userid = getUserId();
            model.db.last_user_action = userid;
            model.db.from_user = userid;
            repo.cancel(model);
            var result = repo._context.Fn_get_sys_approval(model.db.id).FirstOrDefault();


            var data = repo.FindVoucherInfo(model.db.menu, model);
            string response_data = JsonConvert.SerializeObject(data.voucher_data);
            string request_work = "Hủy phiếu: " + data.voucher_detail;


            await _notification.insert_notification(model.db.from_user, new mongodb_notification_db
            {
                id_user = model.db.from_user,
                color = "bg-red-50 text-red-600",
                content = $"Phiếu {data.voucher_detail} đã bị hủy (Lý do: {result.last_note})",
                create_by = model.db.from_user,
                create_date = DateTime.Now,
                data = null,
                url = $"/{model.db.menu}_index?name={HttpUtility.UrlEncode(data.voucher_data.name)}",
                is_read = false,
                module = model.db.menu,
                status_del = 1,
                title = "Hủy phiếu " + data.voucher_detail,
                type = "error"

            });

            if (model.db.status_finish == 4)
            {
                var list_user = repo.FindAllStep(model.db.id).Where(d => d.db.user_id != model.db.from_user).Select(d => d.db.user_id).ToList();
                await _notification.insert_mutiple_notification(list_user, new mongodb_notification_db
                {
                    id_user = model.db.to_user,
                    color = "bg-red-50 text-red-600",
                    content = $"Phiếu {data.voucher_detail} đã bị hủy bởi {result.from_user_name} (Lý do: {result.last_note})",
                    create_by = model.db.to_user,
                    create_date = DateTime.Now,
                    data = null,
                    url = $"/{model.db.menu}_index?name={HttpUtility.UrlEncode(data.voucher_data.name)}",
                    is_read = false,
                    module = model.db.menu,
                    status_del = 1,
                    title = request_work,
                    type = "error"

                });
                repo.send_mail_cancel_approval(list_user, model);
            }

            Request.Headers.TryGetValue("id_user_auth_log", out var id_user_auth_log);
            await user_auth_log_repo.insert(new mongodb_user_log_detail_db
            {
                id_user_auth_log = id_user_auth_log,
                controller_name = model.db.menu,
                action_name = "approval",
                request_time = DateTime.Now,
                create_date = DateTime.Now,
                request_work = request_work,
                response_data = response_data,
                voucher_number = data.voucher_detail,
                account = User.Claims.Where(d => d.Type == ClaimTypes.WindowsAccountName).Select(d => d.Value).FirstOrDefault(),
                user_name = User.Claims.Where(d => d.Type == ClaimTypes.NameIdentifier).Select(d => d.Value).FirstOrDefault(),
                user_id = User.Identity.Name,
                device_ip_address = User.Claims.Where(d => d.Type == ClaimTypes.WindowsDeviceClaim).Select(d => d.Value).FirstOrDefault(),
                device_name = User.Claims.Where(d => d.Type == ClaimTypes.WindowsDeviceGroup).Select(d => d.Value).FirstOrDefault(),
            });



            return Json(result);
        }
        [HttpPost]
        public async Task<JsonResult> close([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_approval_model>(json.ToString());
            var check = checkModelStateClose(model);
            if (!check)
            {
                return generateError();
            }
            var userid = getUserId();
            model.db.last_user_action = userid;
            model.db.from_user = userid;
            repo.close(model);
            var result = repo._context.Fn_get_sys_approval(model.db.id).FirstOrDefault();


            dynamic data = repo.FindVoucherInfo(model.db.menu, model);
            string response_data = JsonConvert.SerializeObject(data);
            string voucher_number = data.name;
            string request_work = "Đóng phiếu: " + voucher_number;

            await _notification.insert_notification(model.db.from_user, new mongodb_notification_db
            {
                id_user = model.db.from_user,
                color = "bg-red-50 text-red-600",
                content = $"Phiếu {voucher_number} đã bị đóng (Lý do: {result.last_note})",
                create_by = model.db.from_user,
                create_date = DateTime.Now,
                data = null,
                url = $"/{model.db.menu}_index?name={HttpUtility.UrlEncode(data.voucher_data.name)}",
                is_read = false,
                module = model.db.menu,
                status_del = 1,
                title = "Đóng phiếu " + voucher_number,
                type = "error"

            });

            if (model.db.status_finish == 6)
            {
                var list_user = repo.FindAllStep(model.db.id).Where(d => d.db.user_id != model.db.from_user).Select(d => d.db.user_id).ToList();
                await _notification.insert_mutiple_notification(list_user, new mongodb_notification_db
                {
                    color = "bg-red-50 text-red-600",
                    content = $"Phiếu {voucher_number} đã bị đóng bởi {result.from_user_name} (Lý do: {result.last_note})",
                    create_by = model.db.to_user,
                    create_date = DateTime.Now,
                    data = null,
                    url = $"/{model.db.menu}_index?name={HttpUtility.UrlEncode(data.voucher_data.name)}",
                    is_read = false,
                    module = model.db.menu,
                    status_del = 1,
                    title = request_work,
                    type = "error"

                });
            }
            Request.Headers.TryGetValue("id_user_auth_log", out var id_user_auth_log);
            await user_auth_log_repo.insert(new mongodb_user_log_detail_db
            {
                id_user_auth_log = id_user_auth_log,
                controller_name = model.db.menu,
                action_name = "approval",
                request_time = DateTime.Now,
                create_date = DateTime.Now,
                request_work = request_work,
                response_data = response_data,
                voucher_number = voucher_number,
                account = User.Claims.Where(d => d.Type == ClaimTypes.WindowsAccountName).Select(d => d.Value).FirstOrDefault(),
                user_name = User.Claims.Where(d => d.Type == ClaimTypes.NameIdentifier).Select(d => d.Value).FirstOrDefault(),
                user_id = User.Identity.Name,
                device_ip_address = User.Claims.Where(d => d.Type == ClaimTypes.WindowsDeviceClaim).Select(d => d.Value).FirstOrDefault(),
                device_name = User.Claims.Where(d => d.Type == ClaimTypes.WindowsDeviceGroup).Select(d => d.Value).FirstOrDefault(),
            });


            return Json(result);
        }
        [HttpPost]
        public async Task<JsonResult> open([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_approval_model>(json.ToString());
            var check = checkModelStateOpen(model);
            if (!check)
            {
                return generateError();
            }
            var userid = getUserId();
            model.db.last_user_action = userid;
            model.db.from_user = userid;
            repo.open(model);
            var result = repo._context.Fn_get_sys_approval(model.db.id).FirstOrDefault();


            var data = repo.FindVoucherInfo(model.db.menu, model);
            string response_data = JsonConvert.SerializeObject(data);

            string voucher_number = data.voucher_data.name;

            string request_work = "Mở phiếu: " + voucher_number;



            var list_user = repo.FindAllStep(model.db.id).Select(d => d.db.user_id).ToList();
            await _notification.insert_mutiple_notification(list_user, new mongodb_notification_db
            {
                id_user = model.db.from_user,
                color = "bg-blue-50 text-blue-600",
                content = $"Phiếu {data.voucher_detail} đã được mở lại (Lý do: {result.last_note})",
                create_by = model.db.from_user,
                create_date = DateTime.Now,
                data = null,
                url = $"/{model.db.menu}_index?name={HttpUtility.UrlEncode(data.voucher_data.name)}",
                is_read = false,
                module = model.db.menu,
                status_del = 1,
                title = "Mở phiếu " + voucher_number,
                type = "info"

            });

            Request.Headers.TryGetValue("id_user_auth_log", out var id_user_auth_log);
            await user_auth_log_repo.insert(new mongodb_user_log_detail_db
            {
                id_user_auth_log = id_user_auth_log,
                controller_name = model.db.menu,
                action_name = "approval",
                request_time = DateTime.Now,
                create_date = DateTime.Now,
                request_work = $"Phiếu {voucher_number} đã được mở lại (Lý do: {result.last_note})",
                response_data = response_data,
                voucher_number = voucher_number,
                account = User.Claims.Where(d => d.Type == ClaimTypes.WindowsAccountName).Select(d => d.Value).FirstOrDefault(),
                user_name = User.Claims.Where(d => d.Type == ClaimTypes.NameIdentifier).Select(d => d.Value).FirstOrDefault(),
                user_id = User.Identity.Name,
                device_ip_address = User.Claims.Where(d => d.Type == ClaimTypes.WindowsDeviceClaim).Select(d => d.Value).FirstOrDefault(),
                device_name = User.Claims.Where(d => d.Type == ClaimTypes.WindowsDeviceGroup).Select(d => d.Value).FirstOrDefault(),
            });



            return Json(result);
        }

        public async Task<JsonResult> getElementById([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var model = await repo.getElementById(id);
            return Json(model);
        }

        public IActionResult getListUser([FromBody] JObject json)
        {
            var search = json["search"].ToString();
            var result = repo._context.users
            .Where(u => u.FirstName.Contains(search) || u.LastName.Contains(search)).
                 Select(d => new
                 {
                     id = d.Id,
                     name = d.FirstName + " " + d.LastName
                 }).ToList();
            return Json(result);
        }


    }
}
