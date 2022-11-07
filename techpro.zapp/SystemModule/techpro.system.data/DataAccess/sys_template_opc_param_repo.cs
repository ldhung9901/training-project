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
    public class sys_template_opc_param_repo 
    {
        public techproDefautContext _context;

        public sys_template_opc_param_repo(techproDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_template_opc_param_model> getElementById(string id)
        {
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        public async Task<sys_template_opc_param_model> getElementByName(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.name == id);
            return obj;
        }

        public async Task<int> insert(sys_template_opc_param_model model)
        {
            await _context.sys_template_opc_params.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public int update(sys_template_opc_param_model model)
        {
           var db= _context.sys_template_opc_params.Where(d=>d.id ==  model.db.id).FirstOrDefault();
            db.name = model.db.name;
            db.interval_second = model.db.interval_second;
            db.keep_record = model.db.keep_record;
            db.opc_node_id = model.db.opc_node_id;
            db.update_by = model.db.update_by;
           
            db.note = model.db.note;
            db.update_date = model.db.update_date;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_template_opc_param_model> FindAll()
        {
            var result = _context.sys_template_opc_params.Select(d=> new sys_template_opc_param_model()
            {
                createby_name =  _context.users.Where(t=>t.Id ==  d.create_by).Select(d=>d.FirstName+ " "+d.LastName).SingleOrDefault(),
                opcnode_name = _context.sys_opcs.Where(t => t.id == d.opc_node_id).Select(d => d.name).SingleOrDefault(),
                template_opc_name = _context.sys_template_opcs.Where(t => t.id == d.id_template_opc).Select(d => d.name).SingleOrDefault(),
                unit_name = _context.sys_units.Where(td => td.id ==
                _context.sys_opcs.Where(t => t.id == d.opc_node_id).Select(d => d.id_unit).SingleOrDefault()
                ).Select(d => d.name).SingleOrDefault(),
                db = d,
            });
         
            return result;
        }
        public int delete(string id, string userid)
        {
            var itemToRemove = _context.sys_template_opc_params.Where(x => x.id == id).FirstOrDefault();
            _context.sys_template_opc_params.Remove(itemToRemove);
            _context.SaveChanges();

            return 1;
        }

    }
}
