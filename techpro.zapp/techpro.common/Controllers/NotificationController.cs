
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using techpro.common.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using techpro.common.Services;
using techpro.common.Models.Users;
using techpro.DataBase.Provider;
using System.Threading.Tasks;
using techpro.DataBase.mongodb.notification;

namespace techpro.common.Controllers
{
    [Authorize]
    [ApiController]

    public class NotificationController : Controller
    {
        private INotificationService _notificationService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        public MongodbDefautContext _mongo;
        public NotificationController(
            MongodbDefautContext mongo,
            INotificationService NotificationService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _notificationService = NotificationService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _mongo = mongo;
        }


        [HttpGet]
        [Route("notification.ctr/get_all")]
        public async Task<IActionResult> get_all()
        {
            var id_user = User.Identity.Name;
            var resp = await _notificationService.get_all_notification(id_user);

            return Json(resp);
        }

        [HttpPost]
        [Route("notification.ctr/update")]
        public async Task<IActionResult> get_all([FromBody] mongodb_notification_db db)
        {
            var id_user = User.Identity.Name;
            var resp = await _notificationService.update_notification(id_user, db);
            return Json(resp);
        }

        [HttpDelete]
        [Route("notification.ctr/delete")]
        public async Task<IActionResult> delete([FromQuery] string id)
        {
            var id_user = User.Identity.Name;
            var resp = await _notificationService.remove_notification(id,id_user);
            return Ok();
        }

    }
}
