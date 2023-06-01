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
       Passenger GetById(Guid id);
       List<Passenger> GetAllPassengers();
       Passenger GetPassengerById(Guid id);
       bool CreatePassenger(Passenger passenger);
       Passenger UpdatePassenger(Guid id, Passenger passenger);
       bool DeletePassenger(Guid id);





    }
}
