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
    public class sys_cost_type_repo 
    {
        public techproDefautContext _context;

        public sys_cost_type_repo(techproDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_cost_type_model> getElementById(string id)
        {
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        
        public async Task<int> insert(sys_cost_type_model model)
        {
            await _context.sys_cost_types.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public int update(sys_cost_type_model model)
        {
           var db= _context.sys_cost_types.Where(d=>d.id ==  model.db.id).FirstOrDefault();
            db.name = model.db.name;
            db.note = model.db.note;
            db.cost_type_code=model.db.cost_type_code;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_cost_type_model> FindAll()
        {
            var result = _context.sys_cost_types.Select(d=> new sys_cost_type_model()
            {
                createby_name =  _context.users.Where(t=>t.Id ==  d.create_by).Select(d=>d.FirstName+ " "+d.LastName).SingleOrDefault(),
                db = d,
            });
         
            return result;
        }
        public int delete(string id,string userid)
        {
            var itemToRemove =  _context.sys_cost_types.Where(x => x.id ==id).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }

    }
}
