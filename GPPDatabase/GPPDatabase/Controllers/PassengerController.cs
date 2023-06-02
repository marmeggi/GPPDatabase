using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using GPPDatabase.Service;
using GPPDatabase.Model;
using GPPDatabase.WebApi.Models;
using System.Threading.Tasks;

namespace GPPDatabase.Controllers
{

    public class PassengerController : ApiController
    {
        public List<PassengerRest> MapToPassengerRestList (List<Passenger> listOfPassengers)
        {
            if (listOfPassengers.Count > 0)
            {
                List<PassengerRest> mappedPassengers = new List<PassengerRest>();
                foreach (Passenger passenger in listOfPassengers)
                {
                    PassengerRest passengerRest = new PassengerRest()
                    {
                        Id = passenger.Id,
                        FirstName = passenger.FirstName,
                        LastName = passenger.LastName,
                        DateOfBirth = passenger.DateOfBirth
                    };
                    mappedPassengers.Add(passengerRest);
                }
                return mappedPassengers;
            }
            return null;
        }


        // GET: api/Passenger
        public async Task<HttpResponseMessage> Get()
        {
            PassengerService passengerService = new PassengerService();
            List<Passenger> listOfPassengers = await passengerService.GetAllPassengersAsync();
            List<PassengerRest> listOfMappedPassengers = MapToPassengerRestList(listOfPassengers);
            if(listOfMappedPassengers != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, listOfMappedPassengers);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
            
        }

        // GET: api/Passenger/5
        public async Task<HttpResponseMessage> Get(Guid id)
        {
            PassengerService passengerService = new PassengerService();
            Passenger passenger = await passengerService.GetPassengerByIdAsync(id);
            if (passenger == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Passenger with Id {id} not found.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, passenger);
        }

        // POST: api/Passenger
        public async Task<HttpResponseMessage> Post([FromBody] Passenger passenger)
        {
            PassengerService passengerService = new PassengerService();
            bool createdPassenger = await passengerService.CreatePassengerAsync(passenger);
            if (createdPassenger)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        // PUT: api/Passenger/5
        public async Task<HttpResponseMessage> Put(Guid id, [FromBody] Passenger updatedPassenger)
        {
            PassengerService passengerService = new PassengerService();
           Passenger newPassenger = await passengerService.UpdatePassengerAsync(id, updatedPassenger);
            if (newPassenger != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);

        }

        // DELETE: api/Passenger/5
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            PassengerService passengerService = new PassengerService();
            bool deletedPassenger = await passengerService.DeletePassengerAsync(id);
            if (deletedPassenger)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
