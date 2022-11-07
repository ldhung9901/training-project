using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using techpro.common.Helpers;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using techpro.common.Services;
using techpro.DataBase.System;
using techpro.common.Models.Users;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using techpro.common.common;
using techpro.DataBase.Provider;
using techpro.DataBase.mongodb.log;
using MongoDB.Driver;
using System.Net;
using System.Net.Sockets;

namespace techpro.common.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        public MongodbDefautContext _mongo;
        public UsersController(
            MongodbDefautContext mongo,
            IUserService UserService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = UserService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _mongo = mongo;
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            
            string device_ip_address = HttpContext.Connection.RemoteIpAddress.ToString();
               

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.FirstName + " "+ user.LastName),
                    new Claim(ClaimTypes.WindowsAccountName, user.Username),
                    new Claim(ClaimTypes.WindowsDeviceClaim, device_ip_address),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var log_id = "";

            // Save log user
            bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            if (!isDevelopment)
            {

                mongodb_user_auth_log_db data = new mongodb_user_auth_log_db()
                {
                    account = user.Username,
                    login_time = DateTime.Now,
                    logout_time = null,
                    user_name = user.FirstName + " " + user.LastName,
                    user_id = user.Id,
                    // device_ip_address = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(),
                    device_ip_address = device_ip_address,
                    create_date = DateTime.Now,
                };
                _mongo.mongodb_user_auth_logs.InsertOne(data);
                log_id = data.id;
            }
            //
            var rs = new
            {
                id = user.Id,
                avatar_path = user.avatar_path,
                username = user.Username,
                firstName = user.FirstName,
                lastName = user.LastName,
                token = tokenString,
                log_id = log_id
            };

            return Ok(rs);

        }
        [HttpGet("logout")]
        [AllowAnonymous]
        public IActionResult Logout(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
                if (!isDevelopment)
                {

                    // mongodb_user_auth_log_db myObject; // passed in 
                    var filter = Builders<mongodb_user_auth_log_db>.Filter.Eq(s => s.id, id);

                    var data = _mongo.mongodb_user_auth_logs.Find(filter).FirstOrDefault();

                    data.logout_time = DateTime.Now;

                    var result = _mongo.mongodb_user_auth_logs.ReplaceOne(filter, data);

                    _mongo.mongodb_user_auth_logs.FindOneAndReplace(t => t.id == id, data);

                }
            }

            return NoContent();
        }




        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            // map model to entity
            var user = _mapper.Map<User>(model);

            try
            {

                // create user
                _userService.Create(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            var model = _mapper.Map<IList<UserModel>>(users);
            return Ok(model);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var user = _userService.GetById(id);
            var model = _mapper.Map<UserModel>(user);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] UpdateModel model)
        {
            // map model to entity and set id
            var user = _mapper.Map<User>(model);
            user.Id = id;

            try
            {
                // update user 
                _userService.Update(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok();
        }
        [HttpPost("changePassword")]
        public IActionResult ChangePassword([FromBody] JObject json)
        {
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.GetValue("data").ToString());
            var UserId = User.Identity.Name;

            var oldPassword = dictionary["oldPassword"];
            var password = dictionary["password"];

            var user = _userService.Authenticate(null, oldPassword, UserId);

            if (user == null)
                return BadRequest(new { message = "Old Password is incorrect" });
            _userService.Update(user, password);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return Ok(new
            {
                id = user.Id,
                username = user.Username,
                firstName = user.FirstName,
                lastName = user.LastName,
                token = tokenString
            });
        }
    }
}
