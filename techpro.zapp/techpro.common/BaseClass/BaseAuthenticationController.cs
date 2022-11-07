using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using techpro.common.Common;
using techpro.DataBase.System;
using techpro.common.Services;
using techpro.DataBase.Provider;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace techpro.common.BaseClass
{
    public enum ActionEnumForm
    {
        create = 1,
        edit = 2,
        detail = 3,
        revise = 4
    }

    [ServiceFilter(typeof(techProAuthorize))]
    [ApiController]
    [Route("[controller].ctr/[action]")]
    public abstract class BaseAuthenticationController : Controller
    {
       private readonly  IUserService _userService;
       public  BaseAuthenticationController(IUserService UserService)
        {
            _userService = UserService;
        }


        public string getUserId()
        {
            return User.Identity.Name;

        }
        public User getUser()
        {
            var username = User.Identity.Name;
            var user = _userService.GetById(username);
            return user;

        }
        public JsonResult generateError()
        {
            Response.StatusCode = 400;
            var errorList = ModelState
               .Where(x => x.Value != null)
               .ToList().
               Where(d => d.Value.Errors.Count > 0)
               .Select(kvp =>
                   new
                   {
                       key = kvp.Key,
                       value = kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                   }
               ).ToList();
            return Json(errorList);
        }
        public JsonResult generateSuscess()
        {
            Response.StatusCode = 200;
            return Json(new { result=""});
        }

        public void remove_approval(techproDefautContext context,string id)
        {
           
            var remove  =  context.sys_approval_details.Where(d=>d.id_approval ==id)   ;
            context.sys_approval_details.RemoveRange(remove);
            var remove1 = context.sys_approvals.Where(d => d.id == id);
            context.sys_approvals.RemoveRange(remove1);
            context.SaveChanges();
          
        }

        [HttpPost]
        public virtual JsonResult sync_cancel_approval([FromBody] JObject json)
        {
            return generateSuscess();
        }
        [HttpPost]
        public virtual JsonResult sync_open_approval([FromBody] JObject json)
        {
            return generateSuscess();
        }
    }
}
