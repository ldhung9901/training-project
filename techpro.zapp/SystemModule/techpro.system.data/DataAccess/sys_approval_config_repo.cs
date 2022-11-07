using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using techpro.system.data.Models;
using techpro.DataBase.Provider;

namespace techpro.system.data.DataAccess
{
    public class sys_approval_config_repo
    {
        public techproDefautContext _context;

        public sys_approval_config_repo(techproDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_approval_config_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }

        public async Task<int> insert(sys_approval_config_model model)
        {
            model.db.step = model.list_item.Select(d => d.db.step_num).Max();
            await _context.sys_approval_configs.AddAsync(model.db);
            _context.SaveChanges();
            saveDetail(model);
            return 1;
        }

        public int update(sys_approval_config_model model)
        {
            var db = _context.sys_approval_configs.Where(d => d.id == model.db.id).FirstOrDefault();
            db.status_del = model.db.status_del;
            db.name = model.db.name;
            db.note = model.db.note;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.step = model.list_item.Select(d => d.db.step_num).Max();
            db.menu = model.db.menu;
            _context.SaveChanges();
            if (model.db.status_del == 1) { saveDetail(model); }
            return 1;
        }
        public void saveDetail(sys_approval_config_model model)
        {
            var delete = _context.sys_approval_config_details.Where(t => t.id_approval_config == model.db.id);
            _context.RemoveRange(delete);
            _context.SaveChanges();
            model.list_item.ForEach(t =>
            {
                t.db.id = 0;
                t.db.id_approval_config = model.db.id;
                t.db.duration_hours = t.db.duration_hours ?? 0;
            });
            var listInsert = model.list_item.Select(d => d.db).ToList();
            _context.sys_approval_config_details.AddRange(listInsert);
            _context.SaveChanges();


        }

        public IQueryable<sys_approval_config_model> FindAll()
        {
            var ListResult = _context.sys_approval_configs.ToList();


           var result = ListResult.Select(d => new sys_approval_config_model()
            {
                createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.FirstName + " " + d.LastName).SingleOrDefault(),
                db = d,
                list_item = _context.sys_approval_config_details
                .Where(t => t.id_approval_config == d.id)
                .Select(d => new sys_approval_config_detail_model()
                {
                    user_name = _context.users.Where(t => t.Id == d.user_id).Select(d => d.FirstName + " " + d.LastName).SingleOrDefault(),
                    db = d,
                }).ToList()
            }).AsQueryable();;
           
            return result;
        }
        public IQueryable<sys_approval_config_detail_model> FindAllItem(string id)
        {
            var result = _context.sys_approval_config_details
                .Where(t => t.id_approval_config == id)
                .Select(d => new sys_approval_config_detail_model()
                {
                    user_name = _context.users.Where(t => t.Id == d.user_id).Select(d => d.FirstName + " " + d.LastName).SingleOrDefault(),
                    db = d,
                });

            return result;
        }
        public int delete(string id, string userid)
        {
            var itemToRemove = _context.sys_approval_configs.Where(x => x.id == id).SingleOrDefault(); ;
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }

    }
}
