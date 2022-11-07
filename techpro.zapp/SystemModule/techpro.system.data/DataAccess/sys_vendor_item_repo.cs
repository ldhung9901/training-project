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
    public class sys_vendor_item_repo
    {
        public techproDefautContext _context;

        public sys_vendor_item_repo(techproDefautContext context)
        {
            _context = context;
        }
        public async Task<sys_vendor_item_model> getElementById(long id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        public async Task<int> insert(sys_vendor_item_model model, sys_vendor_item_history_model modelHistory)
        {
            if ((model.db.id_specification ?? 0) != 0)
            {
                var specification = _context.sys_item_specifications.Where(t => t.id == model.db.id_specification).SingleOrDefault();
                model.db.id_unit = specification.id_unit;
                model.db.id_unit_main = _context.sys_items.Where(t => t.id == model.db.id_item).Select(t => t.id_unit).SingleOrDefault();
                model.db.conversion_factor = specification.conversion_factor;
            }
            modelHistory.db.action = "Tạo mới";


            await _context.sys_vendor_item_historys.AddAsync(modelHistory.db);
            await _context.sys_vendor_items.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public int update(sys_vendor_item_model model, sys_vendor_item_history_model modelHistory)
        {
            var db = _context.sys_vendor_items.Where(d => d.id == model.db.id).FirstOrDefault();
            db.min_stock_order = model.db.min_stock_order;
            db.delivery_time = model.db.delivery_time;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.MPQ = model.db.MPQ;

            modelHistory.db.action = "Sửa";
            _context.sys_vendor_item_historys.AddAsync(modelHistory.db);

            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_vendor_item_model> FindAll()
        {
            var result = _context.sys_vendor_items.Select(d => new sys_vendor_item_model()
            {
                supplier_name = _context.sys_customers.Where(t => t.id == d.id_supplier).Select(d => d.name).SingleOrDefault(),
                createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.FirstName + " " + d.LastName).SingleOrDefault(),
                sys_item_specification_name = _context.sys_item_specifications.Where(t => t.id == d.id_specification).Select(d => d.name).SingleOrDefault(),
                sys_unit_name = _context.sys_units.Where(t => t.id == d.id_unit).Select(d => d.name).SingleOrDefault(),
                sys_unit_main_name = _context.sys_units.Where(t => t.id == d.id_unit_main).Select(d => d.name).SingleOrDefault(),
                sys_item_name = _context.sys_items.Where(t => t.id == d.id_item).Select(d => d.name).SingleOrDefault(),

                db = d,
            });

            return result;
        }
        public IQueryable<sys_vendor_item_history_model> FindAllHistory()
        {


            var result = _context.sys_vendor_item_historys.Select(d => new sys_vendor_item_history_model()
            {
                supplier_name = _context.sys_customers.Where(t => t.id == d.id_supplier).Select(d => d.name).SingleOrDefault(),
                createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.FirstName + " " + d.LastName).SingleOrDefault(),
                sys_item_specification_name = _context.sys_item_specifications.Where(t => t.id == d.id_specification).Select(d => d.name).SingleOrDefault(),
                sys_unit_name = _context.sys_units.Where(t => t.id == d.id_unit).Select(d => d.name).SingleOrDefault(),
                sys_unit_main_name = _context.sys_units.Where(t => t.id == d.id_unit_main).Select(d => d.name).SingleOrDefault(),
                sys_item_name = _context.sys_items.Where(t => t.id == d.id_item).Select(d => d.name).SingleOrDefault(),
                updateby_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.FirstName + " " + d.LastName).SingleOrDefault(),


                db = d,
            });
            return result;
        }
        public int delete(long id, string userid)
        {
            var itemToRemove = _context.sys_vendor_items.Where(x => x.id == id).FirstOrDefault();

            sys_vendor_item_history_db itemToRemoveHistory = new sys_vendor_item_history_db()
            {
                action = "Xóa",

                update_by = userid,

                create_by = itemToRemove.create_by,
                conversion_factor = itemToRemove.conversion_factor,
                create_date = itemToRemove.create_date,
                delivery_time = itemToRemove.delivery_time,
                id_item = itemToRemove.id_item,
                id_specification = itemToRemove.id_specification,
                id_supplier = itemToRemove.id_supplier,
                id_unit = itemToRemove.id_unit,
                id_unit_main = itemToRemove.id_unit,
                min_stock_order = itemToRemove.min_stock_order,
                note = itemToRemove.note,
                price_item = itemToRemove.price_item,
                update_date = DateTime.Now


            };

            _context.sys_vendor_item_historys.AddAsync(itemToRemoveHistory);
            _context.sys_vendor_items.RemoveRange(itemToRemove);
            _context.SaveChanges();
            return 1;
        }

    }
}
