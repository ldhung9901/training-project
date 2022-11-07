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
    public class sys_group_user_repo 
    {
        public techproDefautContext _context;

        public sys_group_user_repo(techproDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_group_user_model> getElementById(string id)
        {
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        
        public async Task<int> insert(sys_group_user_model model)
        {
            await _context.sys_group_users.AddAsync(model.db);
            _context.SaveChanges();
            saveDetail(model);
            return 1;
        }

        public int update(sys_group_user_model model)
        {
           var db= _context.sys_group_users.Where(d=>d.id ==  model.db.id).FirstOrDefault();
            db.status_del = model.db.status_del;
            db.name = model.db.name;
            db.note = model.db.note;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;

            _context.SaveChanges();
            if(model.db.status_del ==1){ saveDetail(model);}
           
            return 1;
        }
        public void saveDetail(sys_group_user_model model)
        {
            var delete1 = _context.sys_group_user_details.Where(t => t.id_group_user == model.db.id);
            _context.RemoveRange(delete1);
            _context.SaveChanges();
            model.list_item.ForEach(t =>
            {
                t.db.id = 0;
                t.db.id_group_user = model.db.id;
            });
            var listInsert1 = model.list_item.Select(d => d.db).ToList();
            _context.sys_group_user_details.AddRange(listInsert1);
            _context.SaveChanges();


            var delete = _context.sys_group_user_roles.Where(t => t.id_group_user == model.db.id);
            _context.RemoveRange(delete);
            _context.SaveChanges();
            model.list_role.ForEach(t =>
            {
                t.db.id = 0;
                t.db.id_group_user = model.db.id;
            });
            var listInsert = model.list_role.Select(d => d.db).ToList();
            _context.sys_group_user_roles.AddRange(listInsert);
            _context.SaveChanges();


        }

        public IQueryable<sys_group_user_model> FindAll()
        {
            var result = _context.sys_group_users.Select(d=> new sys_group_user_model()
            {
                createby_name =  _context.users.Where(t=>t.Id ==  d.create_by).Select(d=>d.FirstName+ " "+d.LastName).SingleOrDefault(),
                db = d,
            });
         
            return result;
        }
        public IQueryable<sys_group_user_role_model> FindAllRole(string id)
        {
            var result = _context.sys_group_user_roles
                .Where(t => t.id_group_user == id)
                .Select(d => new sys_group_user_role_model()
                {
                    db = d,
                });

            return result;
        }
        public IQueryable<sys_group_user_detail_model> FindAllItem(string id)
        {
            var result = _context.sys_group_user_details
                .Where(t => t.id_group_user == id)
                .Select(d => new sys_group_user_detail_model()
                {
                    user_name = _context.users.Where(t => t.Id == d.user_id).Select(d => d.FirstName + " " + d.LastName).SingleOrDefault(),
                    db = d,
                });

            return result;
        }
        public int delete(string id,string userid)
        {
            var itemToRemove =  _context.sys_group_users.Where(x => x.id ==id).SingleOrDefault();           
            itemToRemove.status_del =2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }

    }
}
