using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using techpro.system.data.Models;
using techpro.DataBase.Provider;
using Duende.IdentityServer.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace techpro.system.data.DataAccess
{
    public class sys_item_repo
    {
        public techproDefautContext _context;

       
        public sys_item_repo(techproDefautContext context, IServiceScopeFactory serviceFactory)
        {
            _context = context;
           
        }

        public async Task<sys_item_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        public IQueryable<sys_item_unit_other_model> FindAllUnitOther(string id)
        {
            var result = _context.sys_item_unit_others
                .Where(t => t.id_item == id)
                .Select(d => new sys_item_unit_other_model()
                {
                    sys_unit_name = _context.sys_units.Where(t => t.id == d.id_unit).Select(d => d.name).SingleOrDefault(),
                    db = d,
                });

            return result;
        }
        public IQueryable<sys_item_min_max_stock_model> FindAllMinMaxStock(string id)
        {
            var result = _context.sys_item_min_max_stocks
                .Where(t => t.id_item == id)
                .Select(d => new sys_item_min_max_stock_model()
                {
                    sys_warehouse_name = _context.sys_warehouses.Where(t => t.id == d.id_warehouse).Select(d => d.name).SingleOrDefault(),
                    db = d,
                });

            return result;
        }
        public IQueryable<sys_item_quality_model> FindAllQuanlity(string id)
        {
            var result = _context.sys_item_quanlitys
                .Where(t => t.id_item == id && t.status_del == 1)
                .Select(d => new sys_item_quality_model()
                {
                    db = d,
                    list_item_quality = _context.sys_item_quanlity_details.Where(t => t.id_sys_item_quality == d.id).ToList(),

                });

            return result;
        }
        public async Task<int> insert(sys_item_model model)
        {
            await _context.sys_items.AddAsync(model.db);
            _context.SaveChanges();
            saveDetail(model);
            var id_repair_process = await _context.sys_items.Where(t => t.name == model.db.name).Select(t => t.id).SingleOrDefaultAsync();
            model.db.id = id_repair_process;
            if (model.list_item_quality.Count != 0)
            {
                var listInsert1 = model.list_item_quality.Select(d => d.db).ToList();
                _context.sys_item_quanlitys.AddRange(listInsert1);
                _context.SaveChanges();
            }
            return 1;
        }


        public void deleteListItemQuality(sys_item_model model)
        {

            var listDelete = _context.sys_item_quanlitys
            .Where(d => d.id_item == model.db.id)
            .FirstOrDefault();
            if (listDelete != null)
            {

                listDelete.status_del = 2;
            }
            // var listDetailDetele = _context.sys_item_quanlity_details
            // .Where(t => t.id_sys_item_quality == listDelete.id).FirstOrDefault();
            // listDetailDetele.status_del = 2;
            _context.SaveChanges();

        }
        public int update(sys_item_model model)
        {
            var db = _context.sys_items.Where(d => d.id == model.db.id).FirstOrDefault();
            db.name = model.db.name;
            db.note = model.db.note;
            db.price = model.db.price;
            db.id_item_type = model.db.id_item_type;
            db.id_unit = model.db.id_unit;
            db.id_cost_type = model.db.id_cost_type;
            db.type = model.db.type;

            if (db.pathImg != model.db.pathImg)
            {
                db.pathImg = model.db.pathImg;
            }

            if (model.db.status_del == db.status_del)
            {
                saveDetail(model);
            }
            db.status_del = model.db.status_del;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.code_item = model.db.code_item;
            db.description = model.db.description;

            _context.SaveChanges();



            return 1;
        }

        public void saveDetail(sys_item_model model)
        {
            var delete1 = _context.sys_item_unit_others.Where(t => t.id_item == model.db.id);
            _context.RemoveRange(delete1);
            _context.SaveChanges();
            model.list_sys_item_unit_other.Where(t => t.db.id_unit != null).ToList().ForEach(t => {
                t.db.id = 0;
                t.db.id_item = model.db.id;
            });
            var listInsert1 = model.list_sys_item_unit_other.Where(t => t.db.id_unit != null).Select(d => d.db).ToList();
            _context.sys_item_unit_others.AddRange(listInsert1);
            _context.SaveChanges();

            var delete = _context.sys_item_min_max_stocks.Where(t => t.id_item == model.db.id);
            _context.RemoveRange(delete);
            _context.SaveChanges();
            model.list_item_min_max_stock.Where(t => t.db.id_warehouse != null).ToList().ForEach(t => {
                t.db.id = 0;
                t.db.id_item = model.db.id;
            });
            var listInsert = model.list_item_min_max_stock.Where(t => t.db.id_warehouse != null).Select(d => d.db).ToList();
            _context.sys_item_min_max_stocks.AddRange(listInsert);
            _context.SaveChanges();



            for (int i = 0; i < model.list_item_quality.Count(); i++)
            {

                var old_item = _context.sys_item_quanlitys.Where(t => t.id_item == model.db.id && t.id == model.list_item_quality[i].db.id).ToList();
                if (old_item.IsNullOrEmpty())
                {
                    // sửa : thêm mới
                    _context.sys_item_quanlitys.AddRange(model.list_item_quality[i].db);
                }
                else
                {
                    // sửa : thay đổi hiện tại

                    old_item.ForEach(item => {

                        item.aplly_date = model.list_item_quality[i].db.aplly_date;
                        item.config_code = model.list_item_quality[i].db.config_code;
                        item.config_name = model.list_item_quality[i].db.config_name;
                        item.status_use = model.list_item_quality[i].db.status_use;
                    }
                    );
                }
                model.list_item_quality[i].list_item_quality.ForEach(e => {
                    var list_old_quality = _context.sys_item_quanlity_details.Where(t => t.id_sys_item_quality == model.list_item_quality[i].db.id).ToList();
                    if (list_old_quality.IsNullOrEmpty())
                    {

                        e.id = 0;
                        e.id_sys_item_quality = model.list_item_quality[i].db.id;
                        _context.sys_item_quanlity_details.Add(e);
                    }
                    else
                    {
                        _context.sys_item_quanlity_details.RemoveRange(list_old_quality);
                        e.id = 0;
                        e.id_sys_item_quality = model.list_item_quality[i].db.id;
                        _context.sys_item_quanlity_details.Add(e);
                    }
                }
                        );

            }
            _context.SaveChanges();
        }


        public IQueryable<sys_item_model> FindAll()
        {
            var result = _context.sys_items.Select(d => new sys_item_model()
            {
                createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.FirstName + " " + d.LastName).SingleOrDefault(),
                sys_item_type_name = _context.sys_item_types.Where(t => t.id == d.id_item_type).Select(d => d.name).SingleOrDefault(),
                sys_unit_name = _context.sys_units.Where(t => t.id == d.id_unit).Select(d => d.name).SingleOrDefault(),
                sys_cost_type_name = _context.sys_cost_types.Where(t => t.id == d.id_cost_type).Select(d => d.name).SingleOrDefault(),
                sys_cost_type_code = _context.sys_cost_types.Where(t => t.id == d.id_cost_type).Select(d => d.cost_type_code).SingleOrDefault(),
                db = d,
            }).OrderByDescending(d => d.db.type);

            return result;
        }

        public async Task caculate_report_inventory_item()
        {
            var list_inventory = _context.sys_warehouses.Where(t => t.status_del == 1).ToList();

            DateTime date_now = DateTime.Now;

            
        }

        public int delete(string id, string userid)
        {
            var itemToRemove = _context.sys_items.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }
        public int delete_item(string id, string userid)
        {
            var itemToRemove = _context.sys_item_quanlitys.Where(x => x.id == id && x.status_del == 1).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }
    }
}
