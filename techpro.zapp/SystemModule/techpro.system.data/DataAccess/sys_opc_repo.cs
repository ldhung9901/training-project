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
    public class sys_opc_repo 
    {
        public techproDefautContext _context;

        public sys_opc_repo(techproDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_opc_model> getElementById(string id)
        {
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        public async Task<sys_opc_model> getElementByName(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.name == id);
            return obj;
        }

        public async Task<int> insert(sys_opc_model model)
        {
            await _context.sys_opcs.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public int update(sys_opc_model model)
        {
           var db= _context.sys_opcs.Where(d=>d.id ==  model.db.id).FirstOrDefault();
            db.opcnode = model.db.opcnode;
            db.id_opcclient = model.db.id_opcclient;
            db.name = model.db.name;
            db.note = model.db.note;
            db.id_unit = model.db.id_unit;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_opc_model> FindAll()
        {
            var result = _context.sys_opcs.Select(d=> new sys_opc_model()
            {
                createby_name =  _context.users.Where(t=>t.Id ==  d.create_by).Select(d=>d.FirstName+ " "+d.LastName).SingleOrDefault(),
                opcclient_name = _context.sys_opc_clients.Where(t => t.id == d.id_opcclient).Select(d => d.name).SingleOrDefault(),
                unit_name = _context.sys_units.Where(t => t.id == d.id_unit).Select(t => t.name).FirstOrDefault(),
                db = d,
            });
         
            return result;
        }
        public int delete(string id, string userid)
        {
            var itemToRemove = _context.sys_opcs.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();

            return 1;
        }

    }
}
