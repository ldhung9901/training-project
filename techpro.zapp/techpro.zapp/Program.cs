using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using techpro.common.BaseClass;
using techpro.system.web.MenuAndRole;

namespace techpro.zapp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ListControlller.list = new List<common.Models.ControllerAppModel>();
            ListControlller.list.AddRange(SystemListController.listController);

            ListControlller.listpublicactioncontroller = new List<string>();
            for (int i = 0; i < ListControlller.list.Count; i++)
            {
                ListControlller.listpublicactioncontroller.AddRange(ListControlller.list[i].list_controller_action_public.Select(d => d.ToLower()));
            }
            
            CreateHostBuilder(args).Build().Run(); 
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

            
    }
}
