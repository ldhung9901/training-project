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
    public class sys_item_bom_repo
    {
        public techproDefautContext _context;

        public sys_item_bom_repo(techproDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_item_bom_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == long.Parse(id));
            return obj;
        }

        public async Task<int> insert(sys_item_bom_model model)
        {
            model.db.id = 0;
            if ((model.db.id_specification ?? 0) != 0)
            {
                var specification = _context.sys_item_specifications.Where(t => t.id == model.db.id_specification).SingleOrDefault();
                model.db.id_unit = specification.id_unit;
                model.db.id_unit_main = _context.sys_items.Where(t => t.id == model.db.id_item_bom).Select(t => t.id_unit).SingleOrDefault();
                model.db.quota_main = specification.conversion_factor * model.db.quota;
                model.db.conversion_factor = specification.conversion_factor;
            }
            else
            {
                model.db.id_unit = _context.sys_items.Where(t => t.id == model.db.id_item_bom).Select(t => t.id_unit).SingleOrDefault();
                model.db.id_unit_main = model.db.id_unit;
                model.db.quota_main = model.db.quota;
                model.db.conversion_factor = 1;

            }
            await _context.sys_item_boms.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }
        public async Task<int> insert_config(sys_item_bom_config_model model)
        {
            model.db.id = 0;
            await _context.sys_item_bom_configs.AddAsync(model.db);
            _context.SaveChanges();
            syncItem(model.db.id_item);
            return 1;
        }
        public async Task<int> insert_quality(sys_item_bom_quality_model model)
        {
            model.db.id = 0;
            await _context.sys_item_bom_qualitys.AddAsync(model.db);
            _context.SaveChanges();

            return 1;
        }
        public int update_config(sys_item_bom_config_model model)
        {
            var db = _context.sys_item_bom_configs.Where(d => d.id == model.db.id).FirstOrDefault();
            db.id_item = model.db.id_item;
            db.id_specification = model.db.id_specification;
            db.aplly_date = model.db.aplly_date;
            db.name = model.db.name;
            db.status_use = model.db.status_use;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            _context.SaveChanges();

            return 1;
        }

        public int update(sys_item_bom_model model)
        {
            var db = _context.sys_item_boms.Where(d => d.id == model.db.id).FirstOrDefault();
            db.id_item = model.db.id_item;
            db.quota = model.db.quota;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            _context.SaveChanges();

            return 1;
        }

        public void get_bom_tree(List<sys_item_bom_model> lst, string id_item_parent
            , string indexString_parent, decimal? quata_parent)
        {
            var list = FindAll().Where(d => d.db.id_item == id_item_parent).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].index_string = indexString_parent + "." + i;
                list[i].quota_cal = quata_parent * list[i].db.quota;
                get_bom_tree(lst, list[i].db.id_item_bom, list[i].index_string, list[i].quota_cal);
            }
            lst.AddRange(list);

        }

        public IQueryable<sys_item_bom_quality_model> FindAllQuality()
        {
            var result = _context.sys_item_bom_qualitys.Where(b =>  b.status_del == 1).Select(d => new sys_item_bom_quality_model()
            {
                db = d,
            });

            return result;
        }
        public IQueryable<sys_item_bom_config_model> FindAllConfig()
        {
            var result = _context.sys_item_bom_configs.Select(d => new sys_item_bom_config_model()
            {
                sys_item_name = _context.sys_items.Where(t => t.id == d.id_item).Select(d => d.name).SingleOrDefault(),
                db = d,
                list_item_quality = _context.sys_item_bom_qualitys.Where(b => b.id_item == d.id_item && b.id_item_bom_config == d.id && b.status_del == 1).Select(d => new sys_item_bom_quality_model()
                {
                    db = d,
                }).ToList(),
                list_item_quota = _context.sys_item_boms.Where(b => b.id_item == d.id_item && b.id_item_bom_config == d.id && b.status_del == 1).Select(d => new sys_item_bom_model()
                {
                    sys_item_code = _context.sys_items.Where(t => t.id == d.id_item).Select(d => d.code_item).SingleOrDefault(),
                    sys_config_name = _context.sys_item_bom_configs.Where(t => t.id_item == d.id_item).Select(d => d.name).SingleOrDefault(),
                    createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.FirstName + " " + d.LastName).SingleOrDefault(),
                    sys_item_bom_name = _context.sys_items.Where(t => t.id == d.id_item_bom).Select(d => d.name).SingleOrDefault(),
                    sys_item_name = _context.sys_items.Where(t => t.id == d.id_item).Select(d => d.name).SingleOrDefault(),
                    sys_item_bom_unit_name = _context.sys_units.Where(t => t.id == d.id_unit).Select(d => d.name).SingleOrDefault(),
                    phase_name = _context.sys_phases.Where(t => t.id == d.id_phase).Select(d => d.name).SingleOrDefault(),
                    specification_name = _context.sys_item_specifications.Where(t => t.id == d.id_specification).Select(d => d.name).SingleOrDefault(),
                    db = d,
                }).ToList(),
            }).OrderByDescending(d => d.db.create_date);
            return result;

        }

        public IQueryable<sys_item_bom_model> FindAll()
        {
            var result = _context.sys_item_boms.Select(d => new sys_item_bom_model()
            {
                sys_item_code = _context.sys_items.Where(t => t.id == d.id_item).Select(d => d.code_item).SingleOrDefault(),
                sys_config_name = _context.sys_item_bom_configs.Where(t => t.id_item == d.id_item).Select(d => d.name).SingleOrDefault(),
                createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.FirstName + " " + d.LastName).SingleOrDefault(),
                sys_item_bom_name = _context.sys_items.Where(t => t.id == d.id_item_bom).Select(d => d.name).SingleOrDefault(),
                sys_item_name = _context.sys_items.Where(t => t.id == d.id_item).Select(d => d.name).SingleOrDefault(),
                sys_item_bom_unit_name = _context.sys_units.Where(t => t.id == d.id_unit).Select(d => d.name).SingleOrDefault(),
                phase_name = _context.sys_phases.Where(t => t.id == d.id_phase).Select(d => d.name).SingleOrDefault(),
                specification_name = _context.sys_item_specifications.Where(t => t.id == d.id_specification).Select(d => d.name).SingleOrDefault(),
                db = d,
            });

            return result;
        }
        public int delete(string id)
        {
            var itemToRemove = _context.sys_item_boms.Where(x => x.id == long.Parse(id)).FirstOrDefault();
            itemToRemove.status_del = 2;

            // _context.sys_item_boms.RemoveRange(itemToRemove);
            _context.SaveChanges();

            return 1;
        }
        public int delete_quality(string id)
        {
            var itemToRemove = _context.sys_item_bom_qualitys.Where(x => x.id == int.Parse(id)).FirstOrDefault();
            itemToRemove.status_del = 2;

            // _context.sys_item_boms.RemoveRange(itemToRemove);
            _context.SaveChanges();

            return 1;
        }
        public int delete_bom_config(string id, string userid)
        {
            var itemToRemove = _context.sys_item_bom_configs.Where(x => x.id == int.Parse(id)).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            syncItem(itemToRemove.id_item);

            return 1;
        }

        public void syncItem(string id_item)
        {
            var item = _context.sys_items.Where(t => t.id == id_item).FirstOrDefault();
            var listBom = _context.sys_item_bom_configs.Where(t => t.id_item == id_item).Where(t => t.status_del == 1).Count();
            if (listBom > 0)
            {
                item.has_bom = listBom;
            }
            else
            {
                item.has_bom = 0;
            }
            _context.SaveChanges();
        }
    }
}
