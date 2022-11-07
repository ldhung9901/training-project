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
    public class sys_customer_repo
    {
        public techproDefautContext _context;

        public sys_customer_repo(techproDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_customer_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }

        public async Task<int> insert(sys_customer_model model)
        {
            await _context.sys_customers.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public int update(sys_customer_model model)
        {
            var db = _context.sys_customers.Where(d => d.id == model.db.id).FirstOrDefault();
            db.name = model.db.name;
            db.note = model.db.note;
            db.customer_code = model.db.customer_code;
            db.address = model.db.address;
            db.email = model.db.email;
            db.phone = model.db.phone;
            db.fax = model.db.fax;
            db.customer_service = model.db.customer_service;
            db.name_short = model.db.name_short;
            db.logo_path = model.db.logo_path;
            db.tax_number = model.db.tax_number;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.is_supplier = model.db.is_supplier??false;
            db.is_customer = model.db.is_customer ?? false;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_customer_model> FindAll()
        {
            var result = _context.sys_customers.Select(d => new sys_customer_model()
            {
                createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.FirstName + " " + d.LastName).SingleOrDefault(),
                db = d,
            });

            return result;
        }
        public int delete(string id,string userid)
        {
            var itemToRemove = _context.sys_customers.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }

    }
}
