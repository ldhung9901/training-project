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
    public class sys_user_repo
    {
        public techproDefautContext _context;

        public sys_user_repo(techproDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_user_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.Id == id);
            return obj;
        }

        public async Task<int> insert(sys_user_model model)
        {
            await _context.users.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public int update(sys_user_model model)
        {
            var db = _context.users.Where(d => d.Id == model.db.Id).FirstOrDefault();
            db.id_department = model.db.id_department;
            db.id_job_title = model.db.id_job_title;
            db.phone_number = model.db.phone_number;
            db.email = model.db.email;
            db.FirstName = model.db.FirstName;
            db.LastName = model.db.LastName;
            db.Username = model.db.Username;
            if (!string.IsNullOrWhiteSpace(model.password))
            {
                db.PasswordHash = model.db.PasswordHash;
                db.PasswordSalt = model.db.PasswordSalt;
            }
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_user_model> FindAll()
        {
            var result = _context.users.Select(d => new sys_user_model()
            {
                db = d,
                department_name = _context.sys_departments.Where(t => t.id == d.id_department).Select(d => d.name).SingleOrDefault(),
                job_title_name = _context.sys_job_titles.Where(t => t.id == d.id_job_title).Select(d => d.name).SingleOrDefault(),


            });

            return result;
        }
        public int delete(string id, string userid)
        {
            var itemToRemove = _context.users.Where(x => x.Id == id).FirstOrDefault();
            _context.users.Remove(itemToRemove);
            _context.SaveChanges();
            return 1;
        }

    }
}
