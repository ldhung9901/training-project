using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using techpro.system.data.Models;
using techpro.DataBase.Provider;
using techpro.DataBase.System;
using techpro.common.Models;
using techpro.common.Services;
using Microsoft.AspNetCore.Hosting;
using techpro.common.Common;
using System.IO;
using HandlebarsDotNet;
using System.Web;
using Microsoft.Extensions.Options;
using techpro.common.Helpers;

namespace techpro.system.data.DataAccess
{
    public class sys_approval_repo
    {
        public techproDefautContext _context;
        private IMailService _mailService;
        private readonly AppSettings _appSettings;
        private IWebHostEnvironment Environment;

        public sys_approval_repo(techproDefautContext context, IMailService mailService, IWebHostEnvironment _environment,IOptions<AppSettings> appSettings)
        {
            Environment = _environment;
             _appSettings = appSettings.Value;
            _context = context;
            _mailService = mailService;
        }

        public async Task<sys_approval_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            obj.list_step = FindAllStep(id).ToList();
            obj.list_item = FindAllItem(id).ToList();
            return obj;
        }

        public async Task<int> insert(sys_approval_model model)
        {
            saveStep(model);
            var config = _context.sys_approval_steps
              .Where(d => d.id_approval_config == model.db.id_sys_approval_config)
              .Where(d => d.step_num == 1)
              .FirstOrDefault();
            var next_deadline = DateTime.Now.AddHours(Convert.ToDouble(config.duration_hours ?? 0));
            model.db.deadline = next_deadline;
            model.db.step_name = config.name;
            model.db.total_step = _context.sys_approval_steps
              .Where(d => d.id_approval_config == model.db.id_sys_approval_config).Count();
            await _context.sys_approvals.AddAsync(model.db);
            _context.SaveChanges();

            saveDetail(model);



            return 1;
        }

        public int send_mail_await_approval(string id_user, sys_approval_model model)
        {
            var result_user = _context.Fn_get_sys_approval(model.db.id).FirstOrDefault();
            var data_name = FindVoucherInfo(model.db.menu, model);
            var CurrentDirectory = Path.Combine(this.Environment.WebRootPath, "MailTemplate\\sendWaitApprovalTemplate.html");
            var subject = $"[MES] Gửi phiếu duyệt ({data_name.voucher_detail})";
            string source = File.ReadAllText(CurrentDirectory);
            var template = Handlebars.Compile(source);
            var data = new Dictionary<string, dynamic>(){
                {"menu", model.db.menu},
                {"name", data_name.voucher_detail},
                {"from_user_name", result_user.from_user_name},
                {"to_user_name", result_user.to_user_name},
                {"date", DateTime.Now.ToString("dd/MM/yyyy HH:mm")},
                {"step_name", model.db.step_name},
                {"step_num ", model.db.step_num},
                {"url", $"{_appSettings.host_domain}/{model.db.menu}_index?name={HttpUtility.UrlEncode(data_name.voucher_data.name)}"},
            };
            var result = template(data);
            _mailService.send_mail_user(new MailRequest
            {
                body = result,
                subject = subject,
                id_user = id_user,
            });
            return 1;
        }
        public int send_mail_approval(string id_user, sys_approval_model model)
        {
            var result_user = _context.Fn_get_sys_approval(model.db.id).FirstOrDefault();
            var data_name = FindVoucherInfo(model.db.menu, model);
            var CurrentDirectory = Path.Combine(this.Environment.WebRootPath, "MailTemplate\\sendAppovalTemplate.html");
            var subject = $"[MES] Đã duyệt phiếu ({data_name.voucher_detail})";
            string source = File.ReadAllText(CurrentDirectory);
            var template = Handlebars.Compile(source);
            var data = new Dictionary<string, dynamic>(){
                    {"menu", model.db.menu},
                    {"name", data_name.voucher_detail},
                    {"from_user_name", result_user.from_user_name},
                    {"to_user_name", result_user.to_user_name},
                    {"date", DateTime.Now.ToString("dd/MM/yyyy HH:mm")},
                    {"step_name", model.db.step_name},
                    {"step_num ", model.db.step_num},
                    {"url", $"{_appSettings.host_domain}/{model.db.menu}_index?name={HttpUtility.UrlEncode(data_name.voucher_data.name)}"},
                };
            var result = template(data);
            _mailService.send_mail_user(new MailRequest
            {
                body = result,
                subject = subject,
                id_user = id_user,
            });
            return 1;
        }
        public int send_mail_cancel_approval(List<string> list_id_user, sys_approval_model model)
        {
            // var list_user = FindAllStep(model.db.id).Select(d => d.db.user_id).ToList();
            var result_user = _context.Fn_get_sys_approval(model.db.id).FirstOrDefault();
            var data_name = FindVoucherInfo(model.db.menu, model);
            var CurrentDirectory = Path.Combine(this.Environment.WebRootPath, "MailTemplate\\sendCancelAppovalTemplate.html");
            var subject = $"[MES] Hủy phiếu ({data_name.voucher_detail})";
            string source = File.ReadAllText(CurrentDirectory);
            var template = Handlebars.Compile(source);
            var data = new Dictionary<string, dynamic>(){
                    {"menu", model.db.menu},
                    {"name", data_name.voucher_detail},
                    {"from_user_name", result_user.from_user_name},
                    {"to_user_name", result_user.to_user_name},
                    {"date", DateTime.Now.ToString("dd/MM/yyyy HH:mm")},
                    {"step_name", model.db.step_name},
                    {"step_num ", model.db.step_num},
                    {"last_note ",result_user.last_note},
                    {"url", $"{_appSettings.host_domain}/{model.db.menu}_index?name={HttpUtility.UrlEncode(data_name.voucher_data.name)}"},
                };
            var result = template(data);
            _mailService.send_mail_mutiple_user(new MailRequest
            {
                body = result,
                subject = subject,
                list_id_user = list_id_user,
            });
            return 1;
        }

        public int approval(sys_approval_model model)
        {
            var db = _context.sys_approvals.Where(d => d.id == model.db.id).FirstOrDefault();
            db.last_date_action = DateTime.Now;
            db.from_user = model.db.from_user;
            db.last_user_action = model.db.last_user_action;
            db.last_date_action = DateTime.Now;
            db.last_note = model.db.last_note;
            db.status_action = model.db.status_action;
            if (model.db.status_action == 1)
            {
                saveStep(model);
                var config = _context.sys_approval_steps
               .Where(d => d.id_approval_config == model.db.id_sys_approval_config)
               .Where(d => d.step_num == 1)
               .FirstOrDefault();
                var next_deadline = DateTime.Now.AddHours(Convert.ToDouble(config.duration_hours ?? 0));

                db.to_user = config.user_id;
                db.id_sys_approval_config = model.db.id_sys_approval_config;
                db.status_finish = 2;
                db.start_by = model.db.start_by;
                db.start_date = DateTime.Now;
                db.step_name = config.name;
                db.step_num = 1;
                db.deadline = next_deadline;
                db.last_note = "";
                db.total_step = _context.sys_approval_steps
              .Where(d => d.id_approval_config == model.db.id_sys_approval_config).Count();
                _context.SaveChanges();
                model.db = db;
                update_step(db.id, db.step_num, db.status_action ?? 4);
                saveDetail(model);


            }

            else if (model.db.status_action == 2)
            {

                var checkNextStep = _context.sys_approval_steps
               .Where(d => d.id_approval_config == (db.id_sys_approval_config))
               .Where(d => d.step_num == (db.step_num + 1))
               .FirstOrDefault();
                if (checkNextStep == null)
                {
                    db.status_action = 2;
                    _context.SaveChanges();
                    model.db = db;
                    update_step(db.id, db.step_num, db.status_action ?? 4);
                    saveDetail(model);

                    db.status_finish = 3;
                    db.to_user = "";
                    db.deadline = null;
                    _context.SaveChanges();
                }
                else
                {

                    db.status_action = 2;
                    _context.SaveChanges();
                    model.db = db;
                    update_step(db.id, db.step_num, db.status_action ?? 4);
                    saveDetail(model);

                    var next_deadline = DateTime.Now.AddHours(Convert.ToDouble(checkNextStep.duration_hours ?? 0));
                    db.to_user = checkNextStep.user_id;
                    db.deadline = next_deadline;
                    db.step_num++;
                    db.step_name = checkNextStep.name;
                    _context.SaveChanges();
                    model.db = db;
                    update_step(db.id, db.step_num, 1);
                }


            }
            else if (model.db.status_action == 3)
            {
                db.status_finish = 5;
                db.to_user = db.start_by;
                db.deadline = null;
                _context.SaveChanges();
                model.db = db;
                update_step(db.id, db.step_num, db.status_action ?? 4);
                saveDetail(model);
            }

            return 1;
        }

        public int cancel(sys_approval_model model)
        {

            var db = _context.sys_approvals.Where(d => d.id == model.db.id).FirstOrDefault();
            db.last_date_action = DateTime.Now;
            db.from_user = model.db.from_user;
            db.last_user_action = model.db.last_user_action;
            db.last_date_action = DateTime.Now;
            db.last_note = model.db.last_note;
            db.status_action = 4;
            db.deadline = null;
            db.status_finish = 4;
            db.to_user = null;
            model.db = db;
            saveDetail(model);
            _context.SaveChanges();

            
            return 1;

        }
        public int close(sys_approval_model model)
        {
            var db = _context.sys_approvals.Where(d => d.id == model.db.id).FirstOrDefault();
            db.last_date_action = DateTime.Now;
            db.from_user = model.db.from_user;
            db.last_user_action = model.db.last_user_action;
            db.last_date_action = DateTime.Now;
            db.last_note = model.db.last_note;
            db.status_action = 6;
            db.deadline = null;
            db.to_user = null;
            db.status_finish = 6;
            model.db = db;
            saveDetail(model);
            _context.SaveChanges();
            return 1;
        }
        public int open(sys_approval_model model)
        {
            var db = _context.sys_approvals.Where(d => d.id == model.db.id).FirstOrDefault();
            db.last_date_action = DateTime.Now;
            db.from_user = model.db.from_user;
            db.last_user_action = model.db.last_user_action;
            db.last_date_action = DateTime.Now;
            db.last_note = model.db.last_note;
            db.status_action = 5;
            db.deadline = null;
            db.to_user = null;
            db.status_finish = 3;
            model.db = db;
            saveDetail(model);
            _context.SaveChanges();
            return 1;
        }
        public void update_step(string id_approval, int? step_num, int status)
        {
            var step = _context.sys_approval_steps.Where(t => t.id_approval == id_approval && t.step_num == step_num).FirstOrDefault();
            step.status = status;
            _context.SaveChanges();
        }
        public void saveDetail(sys_approval_model model)
        {




            var db = new sys_approval_detail_db()
            {
                date_action = DateTime.Now,
                user_action = model.db.last_user_action,
                deadline = model.db.deadline,
                id = 0,
                step_name = model.db.step_name,
                id_approval = model.db.id,
                from_user = model.db.from_user,
                to_user = model.db.to_user,
                step_num = model.db.step_num,
                status_action = model.db.status_action,
                id_approval_config = model.db.id_sys_approval_config,
                note = model.db.last_note,
                status_finish = model.db.status_finish
            };
            _context.sys_approval_details.Add(db);


            _context.SaveChanges();

        }
        public void saveStep(sys_approval_model model)
        {
            var delete = _context.sys_approval_steps.Where(t => t.id_approval == model.db.id);
            _context.RemoveRange(delete);
            _context.SaveChanges();
            model.list_step.ForEach(t => {
                t.db.id = 0;
                t.db.status = 4;
                if (t.db.step_num == 1)
                {
                    t.db.status = 1;
                }
                t.db.id_approval_config = model.db.id_sys_approval_config;
                t.db.id_approval = model.db.id;
            });
            var listInsert = model.list_step.Select(d => d.db).ToList();
            _context.sys_approval_steps.AddRange(listInsert);
            _context.SaveChanges();


        }

        public IQueryable<sys_approval_model> FindAll()
        {
            var result = _context.sys_approvals.Select(d => new sys_approval_model()
            {
                createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.FirstName + " " + d.LastName).SingleOrDefault(),
                db = d,
            });

            return result;
        }
        public IQueryable<sys_approval_detail_model> FindAllItem(string id)
        {
            var result = _context.sys_approval_details
                .Where(t => t.id_approval == id)
                .Select(d => new sys_approval_detail_model()
                {
                    from_user_name = _context.users.Where(t => t.Id == d.from_user).Select(d => d.FirstName + " " + d.LastName).SingleOrDefault(),
                    to_user_name = _context.users.Where(t => t.Id == d.to_user).Select(d => d.FirstName + " " + d.LastName).SingleOrDefault(),
                    db = d,
                });

            return result;
        }
        public IQueryable<sys_approval_step_model> FindAllStep(string id)
        {
            var result = _context.sys_approval_steps
                .Where(t => t.id_approval == id)
                .Select(d => new sys_approval_step_model()
                {
                    user_name = _context.users.Where(t => t.Id == d.user_id).Select(d => d.FirstName + " " + d.LastName).SingleOrDefault(),
                    db = d,
                });

            return result;
        }
        public ApprovalVoucher FindVoucherInfo(string menu, sys_approval_model model)
        {
            dynamic voucher_data = null;
            string voucher_detail = "";
            var menu_name = "";
            switch (menu)
            {

           

            }
            return new ApprovalVoucher()
            {
                voucher_data = voucher_data,
                voucher_detail = voucher_detail,
                menu_name = menu_name
            };

        }
        public int delete(string id)
        {
            var itemToRemove = _context.sys_approvals.Where(x => x.id == id).SingleOrDefault();
            itemToRemove.status_finish = 4;
            _context.SaveChanges();
            return 1;
        }


    }


}
