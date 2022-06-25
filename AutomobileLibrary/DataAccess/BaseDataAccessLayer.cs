using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AutomobileLibrary.DataAccess
{
    internal class BaseDataAccessLayer
    {
        public StockDataProvider? dataProvider { get; set; }
        public SqlConnection? connection;

        public string GetConnectionString()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            return configuration["ConnectionStrings:MyStockDb"];
        }
        public BaseDataAccessLayer()
        {
            dataProvider = new StockDataProvider(GetConnectionString());
        }

        public void CloseConnection() => dataProvider?.CloseConnection(connection);

    }
}
