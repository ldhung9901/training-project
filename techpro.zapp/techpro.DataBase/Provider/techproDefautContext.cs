using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using techpro.DataBase.Function;
using techpro.DataBase.System;
using Audit.EntityFramework;

namespace techpro.DataBase.Provider
{
    public partial class techproDefautContext : AuditDbContext 
    {
        public techproDefautContext(DbContextOptions<techproDefautContext> options)
            : base(options)
        {
        }
        private string connectionString;
        public techproDefautContext(DbContextOptions<techproDefautContext> options, string connection)
        {
            connectionString = connection;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            systemTableBuilder(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (connectionString != null)
            {
                var config = connectionString;
                optionsBuilder.UseSqlServer(config);
            }

            base.OnConfiguring(optionsBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        [DbFunction("Fn_remove_unicode", "dbo")]
        public string FN_ENCRYPT(string strInput)
        {
            throw new NotImplementedException();
        }
        [DbFunction("Fn_status_finish", "dbo")]
        public int Fn_status_finish(string id)
        {
            throw new NotImplementedException();
        }

        [DbFunction("Fn_approval_last_date_action", "dbo")]
        public DateTime Fn_approval_last_date_action(string id)
        {
            throw new NotImplementedException();
        }

        [DbFunction("Fn_check_finish_approval", "dbo")]
        public bool Fn_check_finish_approval(string id)
        {
            throw new NotImplementedException();
        }
        [DbFunction("Fn_check_finish_approval_to_next_step", "dbo")]
        public bool Fn_check_finish_approval_to_next_step(string id)
        {
            throw new NotImplementedException();
        }

        [DbFunction("Fn_check_valid_approval", "dbo")]
        public bool Fn_check_valid_approval(string id, int? status_del)
        {
            throw new NotImplementedException();
        }



        public List<Fn_get_sys_approval> Fn_get_sys_approval(string id_approval)
        {
            // Initialization.  
            // Processing.  
            string sqlQuery = "select * from  [dbo].[Fn_get_sys_approval] ('" + id_approval + "')";

            var lst = this.Set<Fn_get_sys_approval>().FromSqlRaw(sqlQuery).ToList();

            // Info.  
            return lst;
        }
    }
}
