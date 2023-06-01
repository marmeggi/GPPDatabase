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


        public Passenger GetById(Guid id)
        {
            PassengerRepository passengerRepository = new PassengerRepository();
            return passengerRepository.GetById(id);
        }

        public List<Passenger> GetAllPassengers() 
        {
            PassengerRepository passengerRepository = new PassengerRepository();
            return passengerRepository.GetAllPassengers(); 
        }

        public Passenger GetPassengerById(Guid id) 
        {
            PassengerRepository passengerRepository = new PassengerRepository();
            return passengerRepository.GetPassengerById(id);
        }

        public bool CreatePassenger(Passenger passenger) 
        {
            PassengerRepository passengerRepository = new PassengerRepository();
            return passengerRepository.CreatePassenger(passenger);
        }

        public Passenger UpdatePassenger(Guid id, Passenger passenger) 
        {
            PassengerRepository passengerRepository = new PassengerRepository();
            return passengerRepository.UpdatePassenger(id, passenger);
        }

        public bool DeletePassenger(Guid id) 
        {
            PassengerRepository passengerRepository = new PassengerRepository();
            return passengerRepository.DeletePassenger(id);
        }


    }
}
