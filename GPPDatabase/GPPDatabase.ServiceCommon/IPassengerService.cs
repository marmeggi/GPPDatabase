using GPPDatabase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPPDatabase.ServiceCommon
{
    public interface IPassengerService
    {
       Task<Passenger> GetByIdAsync(Guid id);
       Task<List<Passenger>> GetAllPassengersAsync();
       Task<Passenger> GetPassengerByIdAsync(Guid id);
       Task<bool> CreatePassengerAsync(Passenger passenger);
       Task<Passenger> UpdatePassengerAsync(Guid id, Passenger passenger);
       Task<bool> DeletePassengerAsync(Guid id);





    }
}
