using AutomobileLibrary.BusinessObject;

namespace AutomobileLibrary.Repository
{
    public interface ICarRepository
    {
        public IEnumerable<Car>? GetCars();
        public void InsertCar(Car car);
        public void DeleteCar(int carId);
        public Car? GetCarById(int carId);
        public void UpdateCar(Car car);
    }
}
