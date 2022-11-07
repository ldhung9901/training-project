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
    public class sys_configuration_repo 
    {
        public techproDefautContext _context;

        public sys_configuration_repo(techproDefautContext context)
        {
            _context = context;
        }
        public IQueryable<sys_configuration_model> FindAll()
        {
            var result = _context.sys_companys.Select(d=> new sys_configuration_model()
            {
                createby_name =  _context.users.Where(t=>t.Id ==  d.create_by).Select(d=>d.FirstName+ " "+d.LastName).SingleOrDefault(),
                db_company = d,
            });
         
            return result;
        }
        public async Task<sys_configuration_model> getElementById(string id)
        {
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db_company.id == id);
            return obj;
        }
        

        public int update(sys_configuration_model model)
        {
           var db= _context.sys_companys.Where(d=>d.id ==  model.db_company.id).FirstOrDefault();
            db.name = model.db_company.name;
            db.address = model.db_company.address;
            db.tax_number = model.db_company.tax_number;
            db.phone = model.db_company.phone;
            if (db.pathimg != model.db_company.pathimg)
            {
                db.pathimg = model.db_company.pathimg;
            }
            db.user_service = model.db_company.user_service;
            db.status_del = model.db_company.status_del;
            db.update_by = model.db_company.update_by;
            db.update_date = model.db_company.update_date;
            _context.SaveChanges();
            return 1;
        }
    
        public async Task<int> insert_license(sys_configuration_model_v2 model,string user_id)
        {
            var list_data_old= _context.sys_format_license_configs.Where(t=>t.status_del==1);
            var list_update=model.db_format_license_config.Where(t=>t.id!=0).ToList();
            var list_delete=list_data_old.Where(t=>!list_update.Select(d=>d.id).Contains(t.id)).ToList();
            var list_add= model.db_format_license_config.Where(t=>t.id==0).ToList();


            for (int i = 0; i < list_add.Count(); i++)
            {
                list_add[i].create_by=user_id;
                list_add[i].create_date= DateTime.Now;
                _context.sys_format_license_configs.Add(list_add[i]);
            }
            list_update.ForEach(item_model=>{
                var item_update= _context.sys_format_license_configs.Where(t=>t.id==item_model.id).FirstOrDefault();
                if(item_update!=null){
                    item_update.first_character=item_model.first_character;
                    item_update.menu=item_model.menu;
                    item_update.is_raise=item_model.is_raise;
                    item_update.update_date=DateTime.Now;
                    item_update.update_by=user_id;
                }

            });

            list_delete.ForEach(t=>t.status_del=2);
            _context.sys_format_license_configs.UpdateRange(list_delete);
            _context.SaveChanges();
            return 1;
        }

        public void delete_license(sys_configuration_model_v2 model)
        {
            var listDelete = _context.sys_format_license_configs;
            _context.sys_format_license_configs.RemoveRange(listDelete);
            _context.SaveChanges();

        }

        public IQueryable<sys_format_license_config_db> Find_license()
        {
            var result = _context.sys_format_license_configs;
            return result;
        }


         public async Task<int> insert_time(sys_working_time_model model)
        {
            await _context.sys_working_time_configs.AddAsync(model.db);

            var id_group = await _context.sys_working_time_configs.Where(t => t.name == model.db.name).Select(t => t.id).SingleOrDefaultAsync();
            if (model.list_schedule.Count != 0)
            {
                for (int i = 0; i < model.list_schedule.Count; i++)
                {
                    model.list_schedule[i].db.id_group = model.db.id;
                }

            save_working_time_detail(model);
            }
            await _context.SaveChangesAsync();
            return 1;
        }

        public IQueryable<sys_working_time_model> find_working_time()
        {
            var result = _context.sys_working_time_configs.Select(d=> new sys_working_time_model()
            {
                createby_name =  _context.users.Where(t=>t.Id ==  d.create_by).Select(d=>d.FirstName+ " "+d.LastName).SingleOrDefault(),
                // list_schedule = _context.sys_working_time_detail_configs.Where(m => m.id_group == d.id).ToList(),
                db = d,
            });
         
            return result;
        }

        public IQueryable<sys_working_time_detail_model> FindAllItem(string id)
        {
            var result = _context.sys_working_time_detail_configs
                .Where(t => t.id_group == id)
                .Select(d => new sys_working_time_detail_model()
                {
                   createby_name =  _context.users.Where(t=>t.Id ==  d.create_by).Select(d=>d.FirstName+ " "+d.LastName).SingleOrDefault(),
                    db = d,
                });

            return result;
        }

        public int delete_working_time(string id, string userid)
        {
            var Remove = _context.sys_working_time_configs.Where(x => x.id == id).FirstOrDefault();          
            Remove.status_del = 2;
            Remove.update_by = userid;
            Remove.update_date = DateTime.Now;
            _context.SaveChanges();

            return 1;
        }

        public async Task<int> update_working_time(sys_working_time_model model)
        {
            var db = _context.sys_working_time_configs.Where(d => d.id == model.db.id).FirstOrDefault();

            db.name = model.db.name;
            db.schedual_date = model.db.schedual_date;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.note = model.db.note;
            delete_working_time_detail(model);
             save_working_time_detail(model);
            await _context.SaveChangesAsync();
            return 1;
        }

        public void save_working_time_detail(sys_working_time_model model)
        {

            for (int i = 0; i < model.list_schedule.Count(); i++)
            {
                model.list_schedule[i].db.id = 0;
                _context.sys_working_time_detail_configs.Add(model.list_schedule[i].db);
            }
            _context.SaveChanges();
        }
        public void delete_working_time_detail(sys_working_time_model model)
        {
            var listDelete = _context.sys_working_time_detail_configs.Where(d => d.id_group == model.db.id)
            .ToList();
            _context.sys_working_time_detail_configs.RemoveRange(listDelete);
            _context.SaveChanges();

        }

    }
}
