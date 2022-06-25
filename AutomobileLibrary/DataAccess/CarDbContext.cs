using AutomobileLibrary.BusinessObject;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AutomobileLibrary.DataAccess
{
    class CarDbContext : BaseDataAccessLayer
    {
        private static CarDbContext? _instance;
        private static readonly object _instanceLock = new();
        private CarDbContext() { }
        public static CarDbContext GetDbContext
        {
            get
            {
                lock (_instanceLock)
                {
                    return _instance ??= new CarDbContext();
                }
            }
        }

        public IEnumerable<Car>? GetListCar()
        {
            if (dataProvider == null)
            {
                return null;
            }
            IDataReader? reader = null;
            string selectQuery = "SELECT car_id, car_name, manufacturer, price, released_year FROM cars";
            var cars = new List<Car>();
            try
            {
                reader = dataProvider.GetDataReader(selectQuery, CommandType.Text, out connection);
                while (reader.Read())
                {
                    cars.Add(new Car
                    {
                        CarId = reader.GetInt32(0),
                        CarName = reader.GetString(1),
                        Manufacturer = reader.GetString(2),
                        Price = reader.GetDecimal(3),
                        ReleaseYear = reader.GetInt32(4)
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                reader?.Close();
                CloseConnection();
            }
            return cars;
        }

        public Car? GetCarByID(int carID)
        {
            if (dataProvider == null)
            {
                return null;
            }
            Car? car = null;
            IDataReader? dataReader = null;
            string SQLSelect = "SELECT car_id, car_name, manufacturer, price, released_year FROM cars WHERE car_id = @CarID";
            try
            {
                var param = dataProvider.CreateParameter("@CarID", 4, carID, DbType.Int32);
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection, param);
                if (dataReader.Read())
                {
                    car = new Car
                    {
                        CarId = dataReader.GetInt32(0),
                        CarName = dataReader.GetString(1),
                        Manufacturer = dataReader.GetString(2),
                        Price = dataReader.GetDecimal(3),
                        ReleaseYear = dataReader.GetInt32(4)
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader?.Close();
                CloseConnection();
            }
            return car;
        }

        public void AddNew(Car car)
        {
            try
            {
                if (dataProvider == null)
                {
                    return;
                }
                Car? pro = GetCarByID(car.CarId);
                if (pro == null)
                {
                    string SQLInsert = "INSERT cars VALUES (@CarID, @CarName, @Manufacturer, @Price, @ReleasedYear)";
                    var parameters = new List<SqlParameter>();
                    parameters.Add(dataProvider.CreateParameter("@CarID", 4, car.CarId, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@CarName", 50, car.CarName ??= String.Empty, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@Manufacturer", 50, car.Manufacturer ??= String.Empty, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@Price", 50, car.Price, DbType.Decimal));
                    parameters.Add(dataProvider.CreateParameter("@ReleasedYear", 4, car.ReleaseYear, DbType.Int32));
                    dataProvider.Insert(SQLInsert, CommandType.Text, parameters.ToArray());
                }
                else
                {
                    throw new Exception("The car is already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public void Update(Car car)
        {
            if (dataProvider == null)
            {
                return;
            }
            try
            {
                Car? c = GetCarByID(car.CarId);
                if (c != null)
                {
                    string SQLUpdate = "UPDATE Cars SET car_name = @CarName, " +
                                                        "manufacturer = @Manufacturer, " +
                                                        "price = @Price, " +
                                                        "released_year = @ReleasedYear " +
                                       "WHERE CarID = @CarID";
                    var parameters = new List<SqlParameter>();
                    parameters.Add(dataProvider.CreateParameter("@CarID", 4, car.CarId, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@CarName", 50, car.CarName ??= String.Empty, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@Manufacturer", 50, car.Manufacturer ??= String.Empty, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@Price", 50, car.Price, DbType.Decimal));
                    parameters.Add(dataProvider.CreateParameter("@ReleasedYear", 4, car.ReleaseYear, DbType.Int32));
                    dataProvider.Update(SQLUpdate, CommandType.Text, parameters.ToArray());
                }
                else
                {
                    throw new Exception("The car does not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.StackTrace);
            }
            finally
            {
                CloseConnection();
            }
        }
        public void Remove(int carID)
        {
            if (dataProvider == null)
            {
                return;
            }
            try
            {
                Car? car = GetCarByID(carID);
                if (car != null)
                {
                    String SQLDelete = "DELETE cars WHERE car_id = @CarID";
                    var param = dataProvider.CreateParameter("@CarID", 4, carID, DbType.Int32);
                    dataProvider.Delete(SQLDelete, CommandType.Text, param);
                }
                else
                {
                    throw new Exception("The car does not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
