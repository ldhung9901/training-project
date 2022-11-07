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
    public class sys_template_opc_repo 
    {
        public techproDefautContext _context;

        public sys_template_opc_repo(techproDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_template_opc_model> getElementById(string id)
        {
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        public async Task<sys_template_opc_model> getElementByName(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.name == id);
            return obj;
        }

        public async Task<int> insert(sys_template_opc_model model)
        {
            await _context.sys_template_opcs.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public int update(sys_template_opc_model model)
        {
           var db= _context.sys_template_opcs.Where(d=>d.id ==  model.db.id).FirstOrDefault();
            db.name = model.db.name;
            db.location = model.db.location;
            db.opcnode_id_error = model.db.opcnode_id_error;
            db.opcnode_id_input = model.db.opcnode_id_input;
            db.opcnode_id_output = model.db.opcnode_id_output;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.note = model.db.note;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_template_opc_model> FindAll()
        {
            var result = _context.sys_template_opcs.Select(d=> new sys_template_opc_model()
            {
                createby_name =  _context.users.Where(t=>t.Id ==  d.create_by).Select(d=>d.FirstName+ " "+d.LastName).SingleOrDefault(),
                opcnode_error_name = _context.sys_opcs.Where(t => t.id == d.opcnode_id_error).Select(d => d.name).SingleOrDefault(),
                opcnode_input_name = _context.sys_opcs.Where(t => t.id == d.opcnode_id_input).Select(d => d.name).SingleOrDefault(),
                opcnode_output_name = _context.sys_opcs.Where(t => t.id == d.opcnode_id_output).Select(d => d.name).SingleOrDefault(),
                db = d,
            });
         
            return result;
        }
        public int delete(string id, string userid)
        {
            var itemToRemove = _context.sys_template_opcs.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();

            return 1;
        }

    }
}
