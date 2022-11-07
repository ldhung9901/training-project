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
    public class sys_factory_line_repo
    {
        public techproDefautContext _context;

        public sys_factory_line_repo(techproDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_factory_line_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }

        public async Task<int> insert(sys_factory_line_model model)
        {
            await _context.sys_factory_lines.AddAsync(model.db);
            _context.SaveChanges();
            var db = await _context.sys_factory_lines.Where(t => t.status_del == 1).Where(t => t.status_del == 1).Where(t => t.name == model.db.name).Select(t => t).SingleOrDefaultAsync();
            string id_factory_line = db.id;
            string id_factory = db.id_factory;
            if (model.list_maintenance_system.Count != 0)
            {
                for (int i = 0; i < model.list_maintenance_system.Count; i++)
                {
                    model.list_maintenance_system[i].db.id = 0;
                    model.list_maintenance_system[i].db.id_factory = id_factory;
                    model.list_maintenance_system[i].db.id_factory_line = id_factory_line;
                }
                saveDetailList(model);
            }


            _context.SaveChanges();

            return 1;
        }

        private void saveDetailList(sys_factory_line_model model)
        {
            for (int i = 0; i < model.list_maintenance_system.Count(); i++)
            {


                _context.sys_factory_line_list_maintenance_systems.Add(new sys_factory_line_list_maintenance_system_db
                {

                    id = 0,
                    id_system = model.list_maintenance_system[i].db.id_system,
                    id_factory_line = model.db.id,
                    id_factory = model.db.id_factory,
                    note = model.list_maintenance_system[i].db.note,
                    create_date = DateTime.Now,

                });
            }

            _context.SaveChanges();
        }

        public int update(sys_factory_line_model model)
        {
            var db = _context.sys_factory_lines.Where(t => t.status_del == 1).Where(d => d.id == model.db.id).FirstOrDefault();
            db.name = model.db.name;
            db.note = model.db.note;
            db.factory_line_code = model.db.factory_line_code;
            db.id_factory = model.db.id_factory;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.status_del = model.db.status_del;

            _context.SaveChanges();

            deleteDetailList(model);
            saveDetailList(model);

            return 1;
        }

        private void deleteDetailList(sys_factory_line_model model)
        {
            var listDelete = _context.sys_factory_line_list_maintenance_systems

            .Where(d => d.id_factory_line == model.db.id)
            .ToList();
            _context.sys_factory_line_list_maintenance_systems.RemoveRange(listDelete);
            _context.SaveChanges();

        }

        public IQueryable<sys_factory_line_model> FindAll()
        {
            var result = _context.sys_factory_lines.Where(t => t.status_del == 1).ToList().Select(d => new sys_factory_line_model()
            {
                factory_name = _context.sys_factorys.Where(t => t.id == d.id_factory).Select(d => d.name).SingleOrDefault(),
                createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.FirstName + " " + d.LastName).SingleOrDefault(),
                db = d,
                list_maintenance_system = _context.sys_factory_line_list_maintenance_systems
                 .Where(m => m.id_factory == d.id_factory)
                              .Where(m => m.id_factory_line == d.id)
                              .Select(m => new sys_factory_line_list_maintenance_system_model
                              {
                                 
                                  factory_line_name = _context.sys_factory_lines.Where(t => t.id == d.id).Select(d => d.name).SingleOrDefault(),
                                  factory_name = _context.sys_factorys.Where(t => t.id == d.id_factory).Select(d => d.name).SingleOrDefault(),
                              }).ToList(),

            });

            return result.AsQueryable();
        }
        public int delete(string id, string userid)
        {
            var itemToRemove = _context.sys_factory_lines.Where(t => t.status_del == 1).Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }

    }
}
