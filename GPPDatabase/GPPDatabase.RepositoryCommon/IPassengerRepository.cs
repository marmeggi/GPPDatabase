using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPPDatabase.Model;

namespace GPPDatabase.RepositoryCommon
{
    public interface IPassengerRepository
    {
        Task<Passenger> GetByIdAsync(Guid id);
        Task<Passenger> GetPassengerByIdAsync(Guid id);
        Task<List<Passenger>> GetAllPassengersAsync();
        Task<bool> CreatePassengerAsync(Passenger passenger);
        Task<Passenger> UpdatePassengerAsync(Guid id ,Passenger passenger);
        Task<bool> DeletePassengerAsync(Guid id);
    }

}
