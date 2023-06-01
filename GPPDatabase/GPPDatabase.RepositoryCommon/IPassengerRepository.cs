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
        Passenger GetById(Guid id);
        Passenger GetPassengerById(Guid id);
        List<Passenger> GetAllPassengers();
        bool CreatePassenger(Passenger passenger);
        Passenger UpdatePassenger(Guid id ,Passenger passenger);
        bool DeletePassenger(Guid id);
    }

}
