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
using Microsoft.Extensions.Options;
using techpro.common.Helpers;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using techpro.common.Common;

namespace techpro.system.web.Controller
{
    public partial class sys_userController : BaseAuthenticationController
    {
        private sys_user_repo repo;
        private readonly AppSettings _appSettings;

        public sys_userController(IUserService UserService, techproDefautContext context, IOptions<AppSettings> appSettings) : base(UserService)
        {
            repo = new sys_user_repo(context);
            _appSettings = appSettings.Value;
        }
        [HttpGet]
        public IActionResult avatarImage(string id, string id_user)
        {
            try
            {
                var path = Path.Combine(_appSettings.folder_path, "file_upload", "avatar", id_user, id);
                var bytes = System.IO.File.ReadAllBytes(path);
                return new FileContentResult(bytes, File_content_type.GetContentType(id));
            }
            catch (Exception) { return StatusCode(404); }

        }
        [HttpGet]
        public JsonResult getProfileCurrentUser()
        {
            string id_user = getUserId();
            var model = repo.FindAll().Where(d => d.db.Id == id_user).SingleOrDefault();
            return Json(model);
        }
        public IActionResult getListUse()
        {
            var result = repo._context.users
                 .Select(d => new
                 {
                     id = d.Id,
                     name = d.FirstName + " " + d.LastName
                 }).ToList();
            return Json(result);
        }
        public async Task<IActionResult> uploadAvatar(string id_user)
        {
            var model = repo.FindAll().Where(d => d.db.Id == id_user).SingleOrDefault();

            var avatar = Request.Form.Files;
            var formFile = avatar[0];
            var tick = Guid.NewGuid().ToString();
            var path = Path.Combine(_appSettings.folder_path, "file_upload", "avatar", model.db.Id);
            Thread.Sleep(1);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var filename = formFile.FileName.Trim('"') + "";
            filename = StringFunctions.NonUnicode(filename);
            filename = Regex.Replace(filename, "[^A-Za-z0-9.]", "");
            var pathsave = Path.Combine(path, tick + filename);
            using (System.IO.Stream stream = new FileStream(pathsave, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
            var user = repo._context.users.Where(d => d.Id == id_user).SingleOrDefault();
            user.avatar_path = "/sys_user.ctr/avatarImage?id=" + tick + filename + "&id_user=" + user.Id;
            repo._context.SaveChanges();
            return Ok(new
            {
                id = user.Id,
                avatar_path = user.avatar_path,
                username = user.Username,
                firstName = user.FirstName,
                lastName = user.LastName,
            });
        }
        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_user_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.Id = Guid.NewGuid().ToString();
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(model.password, out passwordHash, out passwordSalt);
            model.db.Id = Guid.NewGuid().ToString();
            model.db.PasswordHash = passwordHash;
            model.db.PasswordSalt = passwordSalt;
            await repo.insert(model);


            return Json(model);
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        [HttpPost]
        public JsonResult edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_user_model>(json.GetValue("data").ToString());
            var check = checkModelStateEdit(model);
            if (!check)
            {
                return generateError();
            }
            if (!string.IsNullOrWhiteSpace(model.password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(model.password, out passwordHash, out passwordSalt);
                model.db.PasswordHash = passwordHash;
                model.db.PasswordSalt = passwordSalt;
            }
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
                var query = repo.FindAll()
                     .Where(d => d.db.LastName.Contains(search)
                     || d.db.FirstName.Contains(search)
                     || d.db.Username.Contains(search)
                     )
                     ;
                var count = query.Count();
                var dataList = await Task.Run(() => query.Skip((param.pageIndex - 1) * param.pageSize).Take(param.pageSize)
                .ToList());
                DTResult<sys_user_model> result = new DTResult<sys_user_model>
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
