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
    public class sys_item_specification_repo 
    {
        public techproDefautContext _context;

        public sys_item_specification_repo(techproDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_item_specification_model> getElementById(string id)
        {
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db.id+"" == id);
            return obj;
        }

        public async Task<int> insert(sys_item_specification_model model)
        {
            model.db.id = 0;
            await _context.sys_item_specifications.AddAsync(model.db);
            _context.SaveChanges();
            syncItem(model.db.id_item);
            return 1;
        }

        public int update(sys_item_specification_model model)
        {
           var db= _context.sys_item_specifications.Where(d=>d.id ==  model.db.id).FirstOrDefault();
            db.name = model.db.name;
            db.note = model.db.note;
            db.other_parameter = model.db.other_parameter;
            db.long_ = model.db.long_;
            db.wide = model.db.wide;
            db.high = model.db.high;
            db.outside_radius = model.db.outside_radius;
            db.inside_radius = model.db.inside_radius;
            db.id_item = model.db.id_item;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.conversion_factor = model.db.conversion_factor;
            db.id_unit = model.db.id_unit;
            _context.SaveChanges();
           
            return 1;
        }

      


        public IQueryable<sys_item_specification_model> FindAll()
        {
            var result = _context.sys_item_specifications.Select(d=> new sys_item_specification_model()
            {
                createby_name =  _context.users.Where(t=>t.Id ==  d.create_by).Select(d=>d.FirstName+ " "+d.LastName).SingleOrDefault(),
                sys_unit_name = _context.sys_units.Where(t => t.id == d.id_unit).Select(d => d.name).SingleOrDefault(),
                sys_item_name = _context.sys_items.Where(t => t.id == d.id_item).Select(d => d.name).SingleOrDefault(),
                sys_item_unit_name = _context.sys_units.Where(td=>td.id ==
                _context.sys_items.Where(t => t.id == d.id_item).Select(d => d.id_unit).SingleOrDefault())
                .Select(d=>d.name).SingleOrDefault(),
                db = d,
            });
            return result;
        }
        public int delete(string id,string userid)
        {
            var itemToRemove =  _context.sys_item_specifications.Where(x => x.id ==long.Parse(id)).FirstOrDefault();
            var id_item = itemToRemove.id_item;
            _context.sys_item_specifications.RemoveRange(itemToRemove);
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            syncItem(id_item);
            return 1;
        }

        public void syncItem(string id_item)
        {
            var item = _context.sys_items.Where(t => t.id == id_item).FirstOrDefault();
            var listBom = _context.sys_item_specifications.Where(t => t.id_item == id_item).Where(t=>t.status_del==1).Count();
            if (listBom > 0)
            {
                item.has_specification = listBom;
            }
            else
            {
                item.has_specification = 0;
            }
            _context.SaveChanges();
        }

    }
}
