using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using techpro.system.data.Models;
using techpro.DataBase.Provider;

namespace techpro.system.data.DataAccess
{
    public class sys_version_repo 
    {
        public techproDefautContext _context;

        public sys_version_repo(techproDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_version_model> getElementById(string id)
        {
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        
        public async Task<int> insert(sys_version_model model)
        {
            await _context.sys_versions.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> update(sys_version_model model)
        {
           var db= await _context.sys_versions.Where(d=>d.id ==  model.db.id).FirstOrDefaultAsync();
            db.version = model.db.version;
            db.release_day = model.db.release_day;
            db.note = model.db.note;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_version_model> FindAll()
        {
            var result = _context.sys_versions.Select(d=> new sys_version_model()
            {
                
                createby_name =  _context.users.Where(t=>t.Id ==  d.create_by).Select(d=>d.FirstName+ " "+d.LastName).SingleOrDefault(),
                db = d,
            }).OrderByDescending(d => d.db.create_date);
         
            return result;
        }

        public IQueryable<sys_version_model> FindAllRange()
        {
            var result = _context.sys_versions.Select(d=> new sys_version_model()
            {
                
                createby_name =  _context.users.Where(t=>t.Id ==  d.create_by).Select(d=>d.FirstName+ " "+d.LastName).SingleOrDefault(),
                db = d,
            });
         
            return result;
        }
        
        public int delete(string id,string userid)
        {
            var itemToRemove =  _context.sys_versions.Where(x => x.id ==id).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }
        public int revert(string id, string userid)
        {
            var itemToRemove = _context.sys_versions.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 1;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }
        
    }
}
