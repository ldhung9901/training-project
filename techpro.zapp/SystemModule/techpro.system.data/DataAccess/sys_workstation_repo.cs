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
    public class sys_workstation_repo 
    {
        public techproDefautContext _context;

        public sys_workstation_repo(techproDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_workstation_model> getElementById(string id)
        {
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        public async Task<sys_workstation_model> getElementByName(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.name == id);
            return obj;
        }

        public async Task<int> insert(sys_workstation_model model)
        {
            await _context.sys_workstations.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public int update(sys_workstation_model model)
        {
           var db= _context.sys_workstations.Where(d=>d.id ==  model.db.id).FirstOrDefault();
            db.name = model.db.name;
            db.manage_workstation = model.db.manage_workstation;
            db.pathimg = model.db.pathimg;
            db.ip_address = model.db.ip_address;
            db.status_del = model.db.status_del;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.note = model.db.note;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_workstation_model> FindAll()
        {
            var result = _context.sys_workstations.Select(d=> new sys_workstation_model()
            {
                createby_name =  _context.users.Where(t=>t.Id ==  d.create_by).Select(d=>d.FirstName+ " "+d.LastName).SingleOrDefault(),
               
                factory_line_name= _context.sys_factory_lines.Where(t => t.status_del ==1).Where(t=> t.id == d.id_sys_factory_line).Select(t=> t.name).SingleOrDefault(),
                factory_name = _context.sys_factorys.Where(t => t.id == d.id_sys_factory).Select(t => t.name).SingleOrDefault(),
                phase_name = _context.sys_phases.Where(t => t.id == d.id_sys_phase).Select(t => t.name).SingleOrDefault(),
                db = d,
            });
         
            return result;
        }
        public int delete(string id, string userid)
        {
            var itemToRemove = _context.sys_workstations.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();

            return 1;
        }

    }
}
