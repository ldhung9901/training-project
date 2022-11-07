

using Audit.Core;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using techpro.DataBase.mongodb.log;
using techpro.DataBase.mongodb.notification;

namespace techpro.DataBase.Provider

{
    public class MongoDBSettings
    {

        public string ConnectionURI { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
    }

    public class MongodbDefautContext
    {

        private readonly IMongoDatabase _database;

        public MongodbDefautContext(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            _database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        }
        public IMongoCollection<mongodb_user_log_db> mongodb_user_logs => _database.GetCollection<mongodb_user_log_db>(nameof(mongodb_user_log_db));
        public IMongoCollection<mongodb_user_auth_log_db> mongodb_user_auth_logs => _database.GetCollection<mongodb_user_auth_log_db>(nameof(mongodb_user_auth_log_db));
        public IMongoCollection<mongodb_user_log_detail_db> mongodb_user_log_details => _database.GetCollection<mongodb_user_log_detail_db>(nameof(mongodb_user_log_detail_db));
        public IMongoCollection<mongodb_notification_db> mongodb_notifications => _database.GetCollection<mongodb_notification_db>(nameof(mongodb_notification_db));

    }
}
