using System.Data.SqlClient;

namespace ProductSample.Utilities
{
    public class DapperUtility
    {
        private readonly IConfiguration _configuration;
        public DapperUtility(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlConnection GetMyConnection()
        {
            var connectionString = _configuration.GetConnectionString("ConnectionStringToNorthWind");
         
            return new SqlConnection(connectionString);
        }
    }
}
