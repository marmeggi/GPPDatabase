using GPPDatabase.Model;
using GPPDatabase.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPPDatabase.ServiceCommon;

namespace GPPDatabase.Service
{
    public class PassengerService : IPassengerService
    {


        public async Task<Passenger> GetByIdAsync(Guid id)
        {
            PassengerRepository passengerRepository = new PassengerRepository();
            return await passengerRepository.GetByIdAsync(id);
        }

        public async Task<List<Passenger>> GetAllPassengersAsync() 
        {
            PassengerRepository passengerRepository = new PassengerRepository();
            return await passengerRepository.GetAllPassengersAsync(); 
        }

        public async Task<Passenger> GetPassengerByIdAsync(Guid id) 
        {
            PassengerRepository passengerRepository = new PassengerRepository();
            return await passengerRepository.GetPassengerByIdAsync(id);
        }

        public async Task<bool> CreatePassengerAsync(Passenger passenger) 
        {
            PassengerRepository passengerRepository = new PassengerRepository();
            return await passengerRepository.CreatePassengerAsync(passenger);
        }

        public async Task<Passenger> UpdatePassengerAsync(Guid id, Passenger passenger) 
        {
            PassengerRepository passengerRepository = new PassengerRepository();
            return await passengerRepository.UpdatePassengerAsync(id, passenger);
        }

        public async Task<bool> DeletePassengerAsync(Guid id) 
        {
            PassengerRepository passengerRepository = new PassengerRepository();
            return await passengerRepository.DeletePassengerAsync(id);
        }


    }
}
