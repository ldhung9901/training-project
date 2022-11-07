
using techpro.DataBase.Provider;
using System.Threading.Tasks;
using techpro.DataBase.mongodb.notification;
using System.Collections.Generic;
using MongoDB.Driver;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using techpro.common.hubs;

namespace techpro.common.Services
{
    public interface INotificationService
    {
        Task<List<mongodb_notification_db>> insert_mutiple_notification(List<string> list_id_user, mongodb_notification_db db);
        Task<mongodb_notification_db> insert_notification(string id_user, mongodb_notification_db db);
        Task<mongodb_notification_db> update_notification(string id_user, mongodb_notification_db db);
        Task<string> remove_notification(string id_notification, string id_user);
        Task<List<mongodb_notification_db>> get_all_notification(string id_user);
    }

    public class NotificationService : INotificationService
    {
        private techproDefautContext _context;
        private MongodbDefautContext _mongodb;
        private IHubContext<notification_hub> _hub;

        public NotificationService(techproDefautContext context, MongodbDefautContext mongodb, IHubContext<notification_hub> hub)
        {
            _context = context;
            _mongodb = mongodb;
            _hub = hub;
        }

        public async Task<List<mongodb_notification_db>> get_all_notification(string id_user)
        {
            var filterId = Builders<mongodb_notification_db>.Filter.Eq(nameof(mongodb_notification_db.id_user), id_user);
            var filterStatusDel = Builders<mongodb_notification_db>.Filter.Eq(nameof(mongodb_notification_db.status_del), 1);
            var result = await _mongodb.mongodb_notifications.Find(filterId & filterStatusDel).SortByDescending(e => e.create_date).ToListAsync();
            return result;
        }


        public async Task<mongodb_notification_db> insert_notification(string id_user, mongodb_notification_db db)
        {
            await _mongodb.mongodb_notifications.InsertOneAsync(db);
            await _hub.Clients.Group(db.id_user).SendAsync("new_notification", db);
            return db;
        }
        public async Task<mongodb_notification_db> update_notification(string id_user, mongodb_notification_db db)
        {
            var filter = Builders<mongodb_notification_db>.Filter.Eq(nameof(mongodb_notification_db.id), db.id);
            await _mongodb.mongodb_notifications.ReplaceOneAsync(filter, db);
            await _hub.Clients.Group(db.id_user).SendAsync("notification", db);
            return db;
        }
        public async Task<List<mongodb_notification_db>> insert_mutiple_notification(List<string> list_id_user, mongodb_notification_db db)
        {
            var list_insert_data = list_id_user.Select(d => new mongodb_notification_db
            {
                color = db.color,
                content = db.content,
                create_by = db.create_by,
                create_date = db.create_date,
                data = db.data,
                id_user = d,
                module = db.module,
                title = db.title,
                type = db.type,
                is_read = db.is_read,
                status_del = db.status_del,
            }).ToList();
            if (list_insert_data.Count > 0)
            {

                await _mongodb.mongodb_notifications.InsertManyAsync(list_insert_data);
                await _hub.Clients.Groups(list_id_user).SendAsync("new_notification", db);
            }
            return list_insert_data;
        }

        public async Task<string> remove_notification(string id_notification, string id_user)
        {
            var filter = Builders<mongodb_notification_db>.Filter.Eq(nameof(mongodb_notification_db.id), id_notification);
            await _mongodb.mongodb_notifications.UpdateOneAsync(filter, Builders<mongodb_notification_db>.Update.Set(rec => rec.status_del, 2));
            await _hub.Clients.Group(id_user).SendAsync("notification", null);
            return id_notification;
        }
    }
}