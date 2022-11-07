using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using techpro.common.BaseClass;
using techpro.common.Services;
using techpro.DataBase.Provider;
using techpro.common.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using techpro.DataBase.Function;
using Newtonsoft.Json.Linq;

namespace techpro.system.web.Controller
{
    public partial class sys_homeController : BaseAuthenticationController
    {
        private techproDefautContext context;
        IServiceScopeFactory _serviceFactory;

        public sys_homeController(IUserService UserService, techproDefautContext _context, IServiceScopeFactory serviceFactory) : base(UserService)
        {
            context = _context;
            _serviceFactory = serviceFactory;
        }
        public JsonResult checkLogin()
        {
            return generateSuscess();

        }

        public JsonResult getListRoleFull()
        {
            var UserId = getUserId();
            var model = ListControlller.list;
            var listdynamic = new List<dynamic>();
            for (int i = 0; i < model.Count; i++)
            {


                var listRole = model[i].list_role
                    .Select(d => new
                    {
                        role = d,
                        controller_name = model[i].translate
                    });
                listdynamic.AddRange(listRole);

            }
            return Json(listdynamic);

        }
        public async Task<ActionResult> getModule()
        {
            var UserId = getUserId();
            var model = JsonConvert.DeserializeObject<List<ControllerAppModel>>(JsonConvert.SerializeObject(ListControlller.list));
            var groupID =await context.sys_group_user_details.Where(d => d.user_id == UserId).Where(d => context.sys_group_users.Where(g => g.id == d.id_group_user).Select(g => g.status_del).FirstOrDefault() == 1).Select(d => d.id_group_user).ToListAsync();
            await Task.WhenAll(model.Select(async (menu) =>
            {
                using (var scope = _serviceFactory.CreateScope())
                {

                    var dbContext = scope.ServiceProvider.GetService<techproDefautContext>();
                    menu.list_role = (await dbContext.sys_group_user_roles.Where(d => groupID.Contains(d.id_group_user)).Where(d => d.id_controller_role.Contains(menu.controller)).ToListAsync()).Select(d => new ControllerRoleModel
                    {
                        id = d.id_controller_role,
                        name = d.role_name,
                        list_controller_action = new List<string>()
                        {
                            d.id_controller_role,
                        }
                    }).ToList();

                    await dbContext.DisposeAsync();
                }
            }));

            // model.ForEach((menu) =>
            // {
            //     menu.list_role = context.sys_group_user_roles.Where(d => groupID.Contains(d.id_group_user)).ToList().Where(d => d.id_controller_role.Split(";")[0] == menu.controller).Select(d => new ControllerRoleModel
            //     {
            //         id = d.id_controller_role,
            //         name = d.role_name,
            //         list_controller_action = new List<string>()
            //         {
            //     d.id_controller_role,
            //         }
            //     }).ToList();
            // });

            var controller_names =await  context.sys_group_user_roles.Where(d => groupID.Contains(d.id_group_user))
                .Select(d => d.controller_name).Distinct().ToListAsync();
            var listdynamic = new List<dynamic>();
            var modelfilerRole = model.Where(d => controller_names.Contains(d.translate) || d.is_show_all_user == true)
                .ToList();

            await Task.WhenAll(modelfilerRole.Select(async (role) =>
            {
                using (var scope = _serviceFactory.CreateScope())
                {

                    var dbContext = scope.ServiceProvider.GetService<techproDefautContext>();
                    var count = await dbContext.sys_approvals
                       .Where(d => d.menu == role.id)
                       .Where(t => t.status_finish == 2)
                       .Where(d => d.to_user == UserId).CountAsync();

                    var countreturn = await dbContext.sys_approvals
                           .Where(d => d.menu == role.id)
                           .Where(t => t.status_finish == 5)
                           .Where(d => d.to_user == UserId).CountAsync();
                    role.badge_approval = count;
                    role.badge_return = countreturn;
                    var item = new
                    {
                        menu = role,
                    };
                    if (item.menu.id == "sys_version_history")
                    {
                        item.menu.badge_version = await dbContext.sys_versions.OrderByDescending(t => t.release_day).Select(t => t.version).FirstOrDefaultAsync();
                    }
                    listdynamic.Add(item);

                    await dbContext.DisposeAsync();
                }
            }));
            // for (int i = 0; i < modelfilerRole.Count; i++)
            // {




            // }
            return Json(listdynamic);

        }

    }
}
