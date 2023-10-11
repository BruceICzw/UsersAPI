using System.Data.SqlClient;

namespace UsersAPI
{
    public class GlobalConfig
    {
        private readonly IConfiguration _config;
        public GlobalConfig(IConfiguration config)
        {
            _config = config;

        }

        public  SqlConnection CreateConnection()
        {
            return new SqlConnection(_config.GetConnectionString("DefaultConnection");
        }
    }
}
