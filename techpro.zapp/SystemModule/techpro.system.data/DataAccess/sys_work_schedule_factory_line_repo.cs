using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using techpro.DataBase.Provider;
using techpro.DataBase.System;
using techpro.system.data.Models;

namespace techpro.system.data.DataAccess
{
    public class sys_work_schedule_factory_line_repo
    {
        public techproDefautContext _context;

        public sys_work_schedule_factory_line_repo(techproDefautContext context)
        {
            _context = context;
        }



        public IQueryable<sys_work_schedule_factory_line_model> FindAll()
        {
            var result = _context.sys_work_schedule_factory_lines.Select(d => new sys_work_schedule_factory_line_model()
            {
                updateby_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.Username).SingleOrDefault(),
                createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.Username).SingleOrDefault(),
                factory_name = _context.sys_factorys.Where(t => t.id == d.id_sys_factory).Select(d => d.name).SingleOrDefault(),
                factory_line_name = _context.sys_factory_lines.Where(t => t.status_del ==1).Where(t => t.id == d.id_sys_factory_line).Select(d => d.name).SingleOrDefault(),
                db = d,
            }); 

            return result;
        }
        public async Task<int> save(sys_work_schedule_factory_line_model model, int action)
        {
            if (action == 1)
            {


           
                await _context.sys_work_schedule_factory_lines.AddAsync(model.db);
                _context.SaveChanges();
            }
            else
            {
                var db =  _context.sys_work_schedule_factory_lines.Where(d => d.id == model.db.id).FirstOrDefault();
                db.timeStart_1 = model.db.timeStart_1;
                db.timeStart_2 = model.db.timeStart_2;
                db.timeStart_3 = model.db.timeStart_3;
                db.timeEnd_1 = model.db.timeEnd_1;
                db.timeEnd_2 = model.db.timeEnd_2;
                db.timeEnd_3 = model.db.timeEnd_3;
                _context.SaveChanges();
            }


            return 1;
        }
    }
}
