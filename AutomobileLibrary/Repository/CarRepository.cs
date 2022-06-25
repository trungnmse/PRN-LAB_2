using AutomobileLibrary.BusinessObject;
using AutomobileLibrary.DataAccess;

namespace AutomobileLibrary.Repository
{
    public class CarRepository : ICarRepository
    {
        public void DeleteCar(int carId) => CarDbContext.GetDbContext.Remove(carId);

        public Car? GetCarById(int carId) => CarDbContext.GetDbContext.GetCarByID(carId);
        public IEnumerable<Car>? GetCars() => CarDbContext.GetDbContext.GetListCar();

        public void InsertCar(Car car) => CarDbContext.GetDbContext.AddNew(car);

        public void UpdateCar(Car car) => CarDbContext.GetDbContext.Update(car);
    }
}
