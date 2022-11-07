using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using techpro.common.BaseClass;
using techpro.DataBase.Provider;
using techpro.DataBase.mongodb.log;
using System.IO;
using techpro.common.Helpers;
using System.Security.Claims;
using System.Net;
using Audit.Core;
using Microsoft.Extensions.Options;

namespace techpro.common.Common
{
    public class techProAuthorize : ActionFilterAttribute
    {

        public string ResourceKey { get; set; }

        public string OperationKey { get; set; }

        private readonly techproDefautContext dbContext;
        public MongodbDefautContext _mongo;
        private IMemoryCache _cache;

        public techProAuthorize(techproDefautContext _dbContext, IMemoryCache memoryCache, MongodbDefautContext mongo, IOptions<MongoDBSettings> mongoDBSettings)
        {
            this.dbContext = _dbContext;
            _cache = memoryCache;
            _mongo = mongo;
            Audit.Core.Configuration.Setup().UseMongoDB(config => config.ConnectionString(mongoDBSettings.Value.ConnectionURI).Database(mongoDBSettings.Value.DatabaseName).Collection("audit_logs"));
            Audit.EntityFramework.Configuration.Setup().ForContext<techproDefautContext>(config => config.IncludeEntityObjects().AuditEventType("{context}:{database}"));
        }
        //Called when access is denied
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            string actionName = context.RouteData.Values["action"].ToString().ToLower();
            string controllerName = context.RouteData.Values["controller"].ToString().ToLower();
            string ipAddress = context.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            string method = request.Method;
            string route = request.Path;
            // Save log user
            bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            if (!isDevelopment)
            {
                _mongo.mongodb_user_logs.InsertOne(new mongodb_user_log_db
                {
                    action_name = actionName,
                    controller_name = controllerName,
                    create_date = DateTime.Now,
                    device_ip_address = ipAddress,
                    request_method = method,
                    request_route = string.Concat(request.Scheme, "://", request.Host.ToUriComponent(), request.PathBase.ToUriComponent(), request.Path.ToUriComponent(), request.QueryString.ToUriComponent()),
                    //request_body_data = body,
                    user_name = context.HttpContext.User.Claims.Where(d => d.Type == ClaimTypes.NameIdentifier).Select(d => d.Value).FirstOrDefault(),
                    user_id = context.HttpContext.User.Identity.Name,
                });
            }
            //
            if (actionName == "sync_cancel_approval" || actionName == "sync_open_approval" || actionName == "sync_close_approval" || actionName == "demo")
            {
                base.OnActionExecuting(context);
                return;
            }
            if (controllerName == "sys_user" && actionName == "avatarimage")
            {
                base.OnActionExecuting(context);
                return;
            }
            var reponse = context.HttpContext.Response.StatusCode;
            // var isValid = false;
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new ContentResult { Content = "401" };
                return;
            }
            if (controllerName == "sys_home" || controllerName == "sys_approval" || actionName == "sync_approval" || controllerName == "api-report")
            {
                base.OnActionExecuting(context);
                return;
            }
            if (ListControlller.listpublicactioncontroller.Contains(controllerName + ";" + actionName))
            {
                base.OnActionExecuting(context);
                return;
            }

            var _checkrole = checkrole(context.HttpContext, controllerName, actionName);
            if (_checkrole == false)
            {
                context.HttpContext.Response.StatusCode = 403;
                context.Result = new ContentResult { Content = "403" };
                return;
            }
            base.OnActionExecuting(context);
        }


        private bool checkrole(HttpContext httpContext, string controllerName, string actionName)
        {
            List<string> cacheActionControllerUser;
            var username = httpContext.User.Identity.Name;
            var cachekey = "techproAuthorizeListControllerAction" + username;
            if (!_cache.TryGetValue(cachekey, out cacheActionControllerUser))
            {

                var userid = username;

                var listgroupId = dbContext.sys_group_user_details

                     .Where(d => dbContext.sys_group_users.Where(t => t.id == d.id_group_user).Select(t => t.status_del).SingleOrDefault() == 1)
                    .Where(d => d.user_id == userid).Select(d => d.id_group_user).ToList();
                var listrole = dbContext.sys_group_user_roles.Where(d => listgroupId.Contains(d.id_group_user)).ToList();
                var listresult = new List<string>();
                var listcontrollerName = listrole.Select(d => d.controller_name).Distinct().ToList();
                var listroleId = listrole.Select(d => d.id_controller_role).Distinct().ToList();
                for (int i = 0; i < ListControlller.list.Count; i++)
                {
                    if (listcontrollerName.ToList().Contains(ListControlller.list[i].translate))
                    {
                        for (int j = 0; j < ListControlller.list[i].list_role.Count; j++)
                        {
                            if (listroleId.Contains(ListControlller.list[i].list_role[j].id))
                            {
                                listresult.AddRange(ListControlller.list[i].list_role[j].list_controller_action.Select(d => d.ToLower()));
                            }
                        }
                    }
                }
                // Key not in cache, so get data.
                cacheActionControllerUser = listresult;
                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromMinutes(1));

                // Save data in cache.
                _cache.Set(cachekey, cacheActionControllerUser, cacheEntryOptions);
            }
            if (!cacheActionControllerUser.Contains(controllerName + ";" + actionName)) return false;


            return true;
        }


    }
}
