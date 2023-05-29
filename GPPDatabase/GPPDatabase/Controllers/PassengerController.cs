using GPPDatabase.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GPPDatabase.Controllers
{

    public class PassengerController : ApiController
    {
        public static string ConnectionString = "Server=localhost;Port=5432;User Id=postgres;Password=bootcamp;Database=GppDatabase;";


        // GET: api/Passenger
        public HttpResponseMessage Get()
        {   
            public List<Passenger> ListOfPassengers = new List<Passenger>();
            try
            { 
                using (conn)
                {
                    NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);
                    NpgsqlCommand comm = new NpgsqlCommand("select * from \"Passenger\"; ", conn);
                    conn.Open();            
                    NpgsqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        Passenger passenger = new Passenger
                        {
                            Id = reader.GetGuid(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            DateOfBirth = reader.GetDateTime(3);
                        };
                        ListOfPassengers.Add(passenger);
                    }
                        return Request.CreateResponse(HttpStatusCode.OK, ListOfPassengers);
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message); 
            }
        }

        // GET: api/Passenger/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Passenger
        public HttpResponseMessage Post([FromBody]Passenger passenger)
        {
            try
            { 
                using (conn)
                {
                    NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);
                    NpgsqlCommand comm = new NpgsqlCommand("insert into \"Passenger\" values(@Id,@FirstName,@LastName,@Dob);", conn);
                    conn.Open();
                    Id = Guid.NewGuid;
                    comm.Parameters.AddWithValue("Id", Id);
                    comm.Parameters.AddWithValue("FirstName", FirstName);
                    comm.Parameters.AddWithValue("LastName", LastName);
                    comm.Parameters.AddWithValue("DateOfBirth", DateOfBirth);
                }
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message); 
            }

            
        }

        // PUT: api/Passenger/5
        public void Put(int id, [FromBody]string value)
        {
        
        }

        // DELETE: api/Passenger/5
        public void Delete(int id)
        {
        }
    }
}
