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
using techpro.common.Models;
using techpro.common.Services;
using techpro.DataBase.Provider;
using techpro.DataBase.System;
using techpro.system.data.DataAccess;
using techpro.system.data.Models;

namespace techpro.system.web.Controller
{
   public partial  class sys_work_schedule_factory_lineController : BaseAuthenticationController
    {
        public sys_work_schedule_factory_line_repo repo;

       

        public sys_work_schedule_factory_lineController(IUserService UserService, techproDefautContext context) : base(UserService)
        {
            repo = new sys_work_schedule_factory_line_repo(context);
        }

        public IActionResult getListUse()
        {


            var result = repo._context.sys_work_schedule_factory_lines
                 .Select(d => new
                 {
                     id = d.id,
                     name = d.daysOfWork
                 }).ToList();
            return Json(result);
        }


        public string getAttributeName(decimal? id)
        {

            var name = "";
            switch (id)
            {
                case 1:
                    name = "Thứ 2";
                    break;
                case 2:
                    name = "Thứ 3";
                    break;
                case 3:
                    name = "Thứ 4";
                    break;
                case 4:
                    name = "Thứ 5";
                    break;
                case 5:
                    name = "Thứ 6";
                    break;
                case 6:
                    name = "Thứ 7";
                    break;
                case 7:
                    name = "Chủ nhật";
                    break;
                default:
                    name = "";
                    break;
            }

            return name;
        }

        public JsonResult DataHandler([FromBody] JObject json)
        {
            try
            {
                var a = Request;
                var param = JsonConvert.DeserializeObject<AntTableParams>(json.GetValue("param1").ToString());
                var dictionary = new Dictionary<string, string>();
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.GetValue("data").ToString());
                var id_sys_factory = dictionary["id_sys_factory"];
                var id_sys_factory_line = dictionary["id_sys_factory_line"];
                var search = dictionary["search"];
                var query = repo.FindAll().Where(q => q.db.id_sys_factory == id_sys_factory && q.db.id_sys_factory_line == id_sys_factory_line);
                var count = query.Count();
                var dataList = query.ToList();

                if (dataList.Count() == 0)
                {
                    for (int i = 1; i <= 7; i++)
                    {
                        dataList.Add(new sys_work_schedule_factory_line_model()
                        {

                            day_name = getAttributeName(i),
                            db = new sys_work_schedule_factory_line_db()
                            {
                                daysOfWork = i,
                                timeStart_1 = null,
                                timeEnd_1= null,
                                note = ""
                            }

                        }) ;
                    }
                }
                else
                {
                    dataList.ForEach(
                             t =>
                             {
                                 t.day_name = getAttributeName(t.db.daysOfWork);
                             }
                         );


                }

                DTResult<sys_work_schedule_factory_line_model> result = new DTResult<sys_work_schedule_factory_line_model>
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

        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {
            var model = new sys_work_schedule_factory_line_model();
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.GetValue("id_sys").ToString());
            var lstmodel = JsonConvert.DeserializeObject<List<sys_work_schedule_factory_line_model>>(json.GetValue("data").ToString());
            var id_sys_factory = dictionary["id_sys_factory"];
            var id_sys_factory_line = dictionary["id_sys_factory_line"];  
            var check = checkModelStateCreate(lstmodel);
            if (!check)
            {
                return generateError();
            }
            for (int i = 0; i < lstmodel.Count(); i++)
            {
                model = lstmodel[i];
                if (model.db.id == 0)
                {
                 
                    model.db.create_date = DateTime.Now;
                    model.db.create_by = getUserId();
                    model.db.id_sys_factory = id_sys_factory;
                    model.db.id_sys_factory_line =id_sys_factory_line;
                    await repo.save(model, 1);



                }
                else
                {
                    model.db.update_date = DateTime.Now;
                    model.db.update_by = getUserId();

                    await repo.save(model, 2);
                }

            }

            return Json(lstmodel);
        }


    }
}
