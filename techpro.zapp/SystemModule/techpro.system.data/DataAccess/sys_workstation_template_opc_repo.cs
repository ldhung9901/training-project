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
    public class sys_workstation_template_opc_repo 
    {
        public techproDefautContext _context;

        public sys_workstation_template_opc_repo(techproDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_workstation_template_opc_model> getElementById(string id)
        {
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
       
        public async Task<int> insert(sys_workstation_template_opc_model model)
        {
            await _context.sys_workstation_template_opcs.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public int update(sys_workstation_template_opc_model model)
        {
           var db= _context.sys_workstation_template_opcs.Where(d=>d.id ==  model.db.id).FirstOrDefault();
            db.update_by = model.db.update_by;
            db.note = model.db.note;
            db.update_date = model.db.update_date;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_workstation_template_opc_model> FindAll()
        {
            var result = _context.sys_workstation_template_opcs.Select(d=> new sys_workstation_template_opc_model()
            {
                createby_name =  _context.users.Where(t=>t.Id ==  d.create_by).Select(d=>d.FirstName+ " "+d.LastName).SingleOrDefault(),
                workstation_name = _context.sys_workstations.Where(t => t.id == d.id_workstation).Select(d => d.name).SingleOrDefault(),
                template_opc_name = _context.sys_template_opcs.Where(t => t.id == d.id_template_opc).Select(d => d.name).SingleOrDefault(),
                
                db = d,
            });
         
            return result;
        }
        public int delete(string id, string userid)
        {
            var itemToRemove = _context.sys_workstation_template_opcs.Where(x => x.id == id).FirstOrDefault();
            _context.sys_workstation_template_opcs.Remove(itemToRemove);
            _context.SaveChanges();

            return 1;
        }

    }
}
