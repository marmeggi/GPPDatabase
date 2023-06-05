using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using GPPDatabase.Model;
using GPPDatabase.Common;

namespace GPPDatabase.RepositoryCommon
{
    public interface IPassengerRepository
    {
        Task<Passenger> GetByIdAsync(Guid id);
        Task<Passenger> GetPassengerByIdAsync(Guid id);

        Task<PagedList<Passenger>> GetPassengersAsync(Filtering filtering, Paging paging, Sorting sorting);

        //Task<List<Passenger>> GetAllPassengersAsync();
        Task<bool> CreatePassengerAsync(Passenger passenger);
        Task<Passenger> UpdatePassengerAsync(Guid id ,Passenger passenger);
        Task<bool> DeletePassengerAsync(Guid id);
    }

}
