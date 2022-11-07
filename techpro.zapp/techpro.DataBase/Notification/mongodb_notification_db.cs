using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace techpro.DataBase.mongodb.notification
{
    public class mongodb_notification_db
    {


        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string id_user { get; set; }
        public string module { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string data { get; set; }
        // reminder: Nhắc nhở. warning: Cảnh báo, info: Thông tin
        public string type { get; set; }
        public string color { get; set; }
        public string url { get; set; }
        public int? status_del { get; set; }
        public bool? is_read { get; set; }

        public DateTime create_date { get; set; }
        public string create_by { get; set; }

    }
}