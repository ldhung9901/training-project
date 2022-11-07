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

namespace techpro.system.data.DataAccess
{
    public class sys_factory_line_item_capacity_repo 
    {
        public techproDefautContext _context;

        public sys_factory_line_item_capacity_repo(techproDefautContext context)
        {
            _context = context;
        }
        public async Task<sys_factory_line_item_capacity_model> getElementById(long id)
        {
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        public async Task<int> insert(sys_factory_line_item_capacity_model model)
        {
            if ((model.db.id_specification ?? 0) != 0)
            {
                var specification = _context.sys_item_specifications.Where(t => t.id == model.db.id_specification).SingleOrDefault();
                model.db.id_unit = specification.id_unit;
                model.db.id_unit_main = _context.sys_items.Where(t => t.id == model.db.id_item).Select(t => t.id_unit).SingleOrDefault();
                model.db.quantity_unit_main = specification.conversion_factor * model.db.quantity;
                model.db.conversion_factor = specification.conversion_factor;
            }
            model.db.energy = 100 / model.db.quantity;
            await _context.sys_factory_line_item_capacitys.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public int update(sys_factory_line_item_capacity_model model)
        {
           var db= _context.sys_factory_line_item_capacitys.Where(d=>d.id ==  model.db.id).FirstOrDefault();
            db.quantity = model.db.quantity;
           
            db.quantity_unit_main = model.db.quantity_unit_main;
            db.energy = 100 / model.db.quantity;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.productTime = model.db.productTime;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_factory_line_item_capacity_model> FindAll()
        {
            var result = _context.sys_factory_line_item_capacitys.Select(d=> new sys_factory_line_item_capacity_model()
            {
                factory_name = _context.sys_factorys.Where(t => t.id == d.id_sys_factory).Select(d => d.name).SingleOrDefault(),
                factory_line_name =  _context.sys_factory_lines.Where(t => t.status_del ==1).Where(t=>t.id ==  d.id_sys_factory_line).Select(d=> d.name).SingleOrDefault(),
                sys_item_specification_name = _context.sys_item_specifications.Where(t => t.id == d.id_specification).Select(d => d.name).SingleOrDefault(),
                sys_unit_name = _context.sys_units.Where(t => t.id == d.id_unit).Select(d => d.name).SingleOrDefault(),
                sys_unit_main_name = _context.sys_units.Where(t => t.id == d.id_unit_main).Select(d => d.name).SingleOrDefault(),
                sys_item_name = _context.sys_items.Where(t => t.id == d.id_item).Select(d => d.name).SingleOrDefault(),
                createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.FirstName + " " + d.LastName).SingleOrDefault(),
                db = d,
            });
         
            return result;
        }

        public async Task<sys_factory_line_item_capacity_db> getElementByIdItem(string id_sys_factory_line, string id_item)
        {
            var result = await _context.sys_factory_line_item_capacitys.Where(d => d.id_sys_factory_line == id_sys_factory_line).Where(d => d.id_item == id_item).FirstAsync();
            return result;
        }

        public int delete(long id,string userid)
        {
            var itemToRemove = _context.sys_factory_line_item_capacitys.Where(x => x.id == id);
             _context.sys_factory_line_item_capacitys.RemoveRange(itemToRemove);
             _context.SaveChanges();
            return 1;
        }

    }
}
