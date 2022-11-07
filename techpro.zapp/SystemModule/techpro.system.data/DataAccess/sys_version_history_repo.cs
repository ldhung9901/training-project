using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using techpro.system.data.Models;
using techpro.DataBase.Provider;

namespace techpro.system.data.DataAccess
{
    public class sys_version_history_repo 
    {
        public techproDefautContext _context;

        public sys_version_history_repo(techproDefautContext context)
        {
            _context = context;
        }


        

        public IQueryable<sys_version_history_model> FindAll()
        {
            var result = _context.sys_versions.Select(d=> new sys_version_history_model()
            {
                createby_name =  _context.users.Where(t=>t.Id ==  d.create_by).Select(d=>d.FirstName+ " "+d.LastName).SingleOrDefault(),
                db = d,
            });
         
            return result;
        }

        public IQueryable<sys_version_history_model> FindAllRange(DateTime startDate, DateTime endDate)
        {
            var result = _context.sys_versions.Where(d => d.release_day.Value.Date >= startDate.Date)
            .Where(d => d.release_day.Value.Date <= endDate.Date).Select(d=> new sys_version_history_model()
            {
                
                createby_name =  _context.users.Where(t=>t.Id ==  d.create_by).Select(d=>d.FirstName+ " "+d.LastName).SingleOrDefault(),
                db = d,
            });
         
            return result;
        }
        
        
    }
}
