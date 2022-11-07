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
    public class sys_inventory_position_item_capacity_repo 
    {
        public techproDefautContext _context;

        public sys_inventory_position_item_capacity_repo(techproDefautContext context)
        {
            _context = context;
        }
        public async Task<sys_inventory_position_item_capacity_model> getElementById(long id)
        {
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        public async Task<int> insert(sys_inventory_position_item_capacity_model model)
        {
            if ((model.db.id_specification ?? 0) != 0)
            {
                var specification = _context.sys_item_specifications.Where(t => t.id == model.db.id_specification).SingleOrDefault();
                model.db.id_unit = specification.id_unit;
                model.db.id_unit_main = _context.sys_items.Where(t => t.id == model.db.id_item).Select(t => t.id_unit).SingleOrDefault();
                model.db.conversion_factor = specification.conversion_factor;
            }
            await _context.sys_inventory_position_item_capacitys.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public int update(sys_inventory_position_item_capacity_model model)
        {
           var db= _context.sys_inventory_position_item_capacitys.Where(d=>d.id ==  model.db.id).FirstOrDefault();
            db.min_stock = model.db.min_stock;
            db.max_stock = model.db.max_stock;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_inventory_position_item_capacity_model> FindAll()
        {
            var result = _context.sys_inventory_position_item_capacitys.Select(d=> new sys_inventory_position_item_capacity_model()
            {
                inventory_name = _context.sys_warehouses.Where(t => t.id == d.id_warehouse).Select(d => d.name).SingleOrDefault(),
                position_name =  _context.sys_warehouse_positions.Where(t=>t.id ==  d.id_position).Select(d=> d.name).SingleOrDefault(),
                sys_item_specification_name = _context.sys_item_specifications.Where(t => t.id == d.id_specification).Select(d => d.name).SingleOrDefault(),
                sys_unit_name = _context.sys_units.Where(t => t.id == d.id_unit).Select(d => d.name).SingleOrDefault(),
                sys_unit_main_name = _context.sys_units.Where(t => t.id == d.id_unit_main).Select(d => d.name).SingleOrDefault(),
                sys_item_name = _context.sys_items.Where(t => t.id == d.id_item).Select(d => d.name).SingleOrDefault(),
                createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.FirstName + " " + d.LastName).SingleOrDefault(),
                db = d,
            });
         
            return result;
        }
        public int delete(long id,string userid)
        {
            var itemToRemove = _context.sys_inventory_position_item_capacitys.Where(x => x.id == id);
             _context.sys_inventory_position_item_capacitys.RemoveRange(itemToRemove);
             _context.SaveChanges();
            return 1;
        }

    }
}
