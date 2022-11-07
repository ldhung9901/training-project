using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using techpro.DataBase.mongodb.log;
using techpro.DataBase.Provider;
using techpro.system.data.Models;

namespace techpro.system.data.DataAccess
{
    public class sys_mongodb_user_auth_log_repo
    {
        public MongodbDefautContext _mongo;

        public sys_mongodb_user_auth_log_repo(MongodbDefautContext mongo)
        {
            _mongo = mongo;
        }


        public async Task<int> insert(mongodb_user_log_detail_db model)
        {
            _mongo.mongodb_user_log_details.InsertOne(model);
            return 1;
        }

        public IQueryable<mongodb_user_log_detail_db> FindAllDetail()
        {
            var result = _mongo.mongodb_user_log_details.AsQueryable();
            return result;
        }
      
        public IQueryable<mongodb_user_auth_log_db> FindAll()
        {
            var result = _mongo.mongodb_user_auth_logs.AsQueryable();

            return result;
        }

    }
}
