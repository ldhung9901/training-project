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
    public class sys_job_title_repo
    {
        public techproDefautContext _context;

        public sys_job_title_repo(techproDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_job_title_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }

        public async Task<int> insert(sys_job_title_model model)
        {
            await _context.sys_job_titles.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public int update(sys_job_title_model model)
        {
            var db = _context.sys_job_titles.Where(d => d.id == model.db.id).FirstOrDefault();
            db.status_del = model.db.status_del;
            db.job_title_code = model.db.job_title_code;
            db.name = model.db.name;
            db.note = model.db.note;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_job_title_model> FindAll()
        {
            var result = _context.sys_job_titles.Select(d => new sys_job_title_model()
            {
                createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.FirstName + " " + d.LastName).SingleOrDefault(),
                db = d,
            });

            return result;
        }
        public int delete(string id, string userid)
        {
            var itemToRemove = _context.sys_job_titles.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();

            return 1;
        }

    }
}
