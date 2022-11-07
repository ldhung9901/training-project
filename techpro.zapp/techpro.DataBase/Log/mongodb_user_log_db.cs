using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace techpro.DataBase.mongodb.log
{
    public class mongodb_user_log_db
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string user_name { get; set; }
        public string user_id { get; set; }
        public string controller_name { get; set; }
        public string action_name { get; set; }
        public string request_method { get; set; }
        public string request_route { get; set; }
        public string request_body_data { get; set; }
        public string device_ip_address { get; set; }
        public DateTime create_date { get; set; }


    }
}