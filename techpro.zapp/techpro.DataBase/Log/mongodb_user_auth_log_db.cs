using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace techpro.DataBase.mongodb.log
{
    public class mongodb_user_auth_log_db
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string user_name { get; set; }
        public string account { get; set; }
        public string user_id { get; set; }
        public string device_name{get;set;}
        public DateTime? login_time { get; set; }
        public DateTime? logout_time { get; set; }
        public string device_ip_address { get; set; } 
        public DateTime create_date { get; set; }

    }
}