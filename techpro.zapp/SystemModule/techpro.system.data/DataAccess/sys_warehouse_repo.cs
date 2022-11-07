using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using techpro.system.data.Models;
using techpro.DataBase.Provider;

namespace techpro.system.data.DataAccess
{
    public class sys_warehouse_repo 
    {
        public techproDefautContext _context;

        public sys_warehouse_repo(techproDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_warehouse_model> getElementById(string id)
        {
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        
        public async Task<int> insert(sys_warehouse_model model)
        {
            await _context.sys_warehouses.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public int update(sys_warehouse_model model)
        {
           var db= _context.sys_warehouses.Where(d=>d.id ==  model.db.id).FirstOrDefault();
            db.name = model.db.name;
            db.warehouse_code = model.db.warehouse_code;
            db.status_del = model.db.status_del;
            db.note = model.db.note;
            db.location = model.db.location;
            db.is_QC = model.db.is_QC;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_warehouse_model> FindAll()
        {
            var result = _context.sys_warehouses.Select(d=> new sys_warehouse_model()
            {
                createby_name =  _context.users.Where(t=>t.Id ==  d.create_by).Select(d=>d.FirstName+ " "+d.LastName).SingleOrDefault(),
                db = d,
            });
         
            return result;
        }
        public int delete(string id,string userid)
        {
            var itemToRemove = _context.sys_warehouses.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            itemToRemove.status_del = 2;
            _context.SaveChanges();
            return 1;
        }

    }
}
