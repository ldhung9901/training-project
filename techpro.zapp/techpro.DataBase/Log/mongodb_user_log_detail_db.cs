using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace techpro.DataBase.mongodb.log
{
    public class mongodb_user_log_detail_db
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string id_user_auth_log { get; set; }
        public string controller_name { get; set; }
        public string action_name { get; set; }
        public string request_work { get; set; }
        public string request_body_data { get; set; }
        public string response_data { get; set; }
        public DateTime? request_time { get; set; }
        public DateTime? create_date { get; set; }
        public string device_ip_address { get; set; }
        public string user_name { get; set; }
        public string account { get; set; }
        public string user_id { get; set; }
        public string device_name { get; set; }
        public string voucher_number { get; set; }

    }
}