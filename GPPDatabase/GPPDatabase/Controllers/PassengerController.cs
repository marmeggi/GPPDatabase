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
using GPPDatabase.Common;
using GPPDatabase.ServiceCommon;

namespace GPPDatabase.Controllers
{

    public class PassengerController : ApiController
    {
        public PassengerController(IPassengerService passengerService)
        {
            PassengerService = passengerService;
        }
        protected IPassengerService PassengerService { get; set; }

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
                        DateOfBirth = passenger.DateOfBirth,
                        CityOfResidence = passenger.CityOfResidence,
                        EmploymentStatusId = passenger.EmploymentStatusId
                    };
                    mappedPassengers.Add(passengerRest);
                }
                return mappedPassengers;
            }
            return null;
        }


        // GET: api/Passenger
        /* public async Task<HttpResponseMessage> Get()
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
        */
        

        public async Task<HttpResponseMessage> Get(int pageSize = 3, int pageNumber = 1, string orderBy = "FirstName", string sortOrder ="asc",
            string searchQuery = null, string employmentStatuses = null, DateTime? minDateOfBirth = null, DateTime? maxDateOfBirth = null)
        {
           

            Paging paging = new Paging()
            {
                PageSize = pageSize,
                PageNumber = pageNumber
            };

            Sorting sorting = new Sorting()
            {
                OrderBy = orderBy,
                SortOrder = sortOrder
            };

            Filtering filtering = new Filtering()
            {
                SearchQuery = searchQuery,
                MinDateOfBirth = minDateOfBirth,
                MaxDateOfBirth = maxDateOfBirth,
                EmploymentStatuses = !string.IsNullOrEmpty(employmentStatuses) ? Helper.ToGuidList(employmentStatuses) : null

            };

            PagedList<Passenger> listOfPassengers = await PassengerService.GetPassengersAsync(filtering, paging, sorting);


            List<PassengerRest> listOfMappedPassengers = MapToPassengerRestList(listOfPassengers);
             if (listOfMappedPassengers != null)
             {
                 return Request.CreateResponse(HttpStatusCode.OK, listOfMappedPassengers);
             }
             return Request.CreateResponse(HttpStatusCode.BadRequest);
                     
        }


        // GET: api/Passenger/5
        public async Task<HttpResponseMessage> Get(Guid id)
        {
           
            Passenger passenger = await PassengerService.GetPassengerByIdAsync(id);
            if (passenger == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Passenger with Id {id} not found.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, passenger);
        }

        // POST: api/Passenger
        public async Task<HttpResponseMessage> Post([FromBody] Passenger passenger)
        {
          
            bool createdPassenger = await PassengerService.CreatePassengerAsync(passenger);
            if (createdPassenger)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        // PUT: api/Passenger/5
        public async Task<HttpResponseMessage> Put(Guid id, [FromBody] Passenger updatedPassenger)
        {
            
           Passenger newPassenger = await PassengerService.UpdatePassengerAsync(id, updatedPassenger);
            if (newPassenger != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);

        }

        // DELETE: api/Passenger/5
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
          
            bool deletedPassenger = await PassengerService.DeletePassengerAsync(id);
            if (deletedPassenger)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
