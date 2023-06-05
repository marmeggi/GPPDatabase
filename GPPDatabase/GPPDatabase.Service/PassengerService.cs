using GPPDatabase.Model;
using GPPDatabase.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPPDatabase.ServiceCommon;
using GPPDatabase.Common;

namespace GPPDatabase.Service
{
    public class PassengerService : IPassengerService
    {


        public async Task<Passenger> GetByIdAsync(Guid id)
        {
            PassengerRepository passengerRepository = new PassengerRepository();
            return await passengerRepository.GetByIdAsync(id);
        }

        public async Task<PagedList<Passenger>> GetPassengersAsync(Filtering filtering, Paging paging, Sorting sorting ) 
        {
            PassengerRepository passengerRepository = new PassengerRepository();
            return await passengerRepository.GetPassengersAsync(filtering, paging, sorting); 
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
