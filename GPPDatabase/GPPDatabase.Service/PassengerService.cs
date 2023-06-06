using GPPDatabase.Model;
using GPPDatabase.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPPDatabase.ServiceCommon;
using GPPDatabase.Common;
using GPPDatabase.RepositoryCommon;

namespace GPPDatabase.Service
{
    public class PassengerService : IPassengerService
    {
        public PassengerService(IPassengerRepository passengerRepository)
        {
            PassengerRepository = passengerRepository;
        }
        protected IPassengerRepository PassengerRepository { get; set; }
        public async Task<Passenger> GetByIdAsync(Guid id)
        {
           
            return await PassengerRepository.GetByIdAsync(id);
        }

        public async Task<PagedList<Passenger>> GetPassengersAsync(Filtering filtering, Paging paging, Sorting sorting ) 
        {
           
            return await PassengerRepository.GetPassengersAsync(filtering, paging, sorting); 
        }

        public async Task<Passenger> GetPassengerByIdAsync(Guid id) 
        {
           
            return await PassengerRepository.GetPassengerByIdAsync(id);
        }

        public async Task<bool> CreatePassengerAsync(Passenger passenger) 
        {
            
            return await PassengerRepository.CreatePassengerAsync(passenger);
        }

        public async Task<Passenger> UpdatePassengerAsync(Guid id, Passenger passenger) 
        {
            
            return await PassengerRepository.UpdatePassengerAsync(id, passenger);
        }

        public async Task<bool> DeletePassengerAsync(Guid id) 
        {
           
            return await PassengerRepository.DeletePassengerAsync(id);
        }


    }
}
