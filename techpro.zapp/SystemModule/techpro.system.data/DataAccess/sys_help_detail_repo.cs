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
    public class sys_help_detail_repo 
    {
        public techproDefautContext _context;

        public sys_help_detail_repo(techproDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_help_detail_model> getElementById(string id)
        {
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        
        public async Task<int> insert(sys_help_detail_model model)
        {
            await _context.sys_help_details.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> update(sys_help_detail_model model)
        {
           var db= await _context.sys_help_details.Where(d=>d.id ==  model.db.id).FirstOrDefaultAsync();
            db.title = model.db.title;
            db.note = model.db.note;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_help_detail_model> FindAll()
        {
            var result = _context.sys_help_details.Select(d=> new sys_help_detail_model()
            {
                help_name =   _context.sys_helps.Where(t => t.id == d.id_help).Select(d => d.name).SingleOrDefault(),
                createby_name =  _context.users.Where(t=>t.Id ==  d.create_by).Select(d=>d.FirstName+ " "+d.LastName).SingleOrDefault(),
                db = d,
            });
         
            return result;
        }
        public int delete(string id,string userid)
        {
            var itemToRemove =  _context.sys_help_details.Where(x => x.id ==id).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }
        public int revert(string id, string userid)
        {
            var itemToRemove = _context.sys_help_details.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 1;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }
        
    }
}
