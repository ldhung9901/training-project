
using techpro.common.hubs.configs;
using techpro.DataBase.Provider;


namespace techpro.common.hubs
{
    public class notification_hub : common_hub
    {
        private techproDefautContext _context;
        public notification_hub(techproDefautContext context)
        {
            _context = context;
        }


    }

}
