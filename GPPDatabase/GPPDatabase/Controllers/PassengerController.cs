using GPPDatabase.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace GPPDatabase.Controllers
{

    public class PassengerController : ApiController
    {
        public static string connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=bootcamp;Database=postgres;";

        private Passenger GetById(Guid id)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand("select * from Passenger where Id = @Id", conn);
                cmd.Parameters.AddWithValue("Id", id);

                NpgsqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Passenger passenger = new Passenger()
                        {
                            Id = reader.GetGuid(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            DateOfBirth = reader.GetDateTime(3)
                        };
                        return passenger;
                    }
                }
                Console.WriteLine($"Passenger with Id {id} not found.");
                return null;
            }
        }


        // GET: api/Passenger
        public HttpResponseMessage Get()
        {   
            List<Passenger> listOfPassengers = new List<Passenger>();
            try
            { 
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    NpgsqlCommand cmd = new NpgsqlCommand("select * from Passenger; ", conn);

                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Passenger passenger = new Passenger
                        {
                            Id = reader.GetGuid(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            DateOfBirth = reader.GetDateTime(3)
                        };

                        listOfPassengers.Add(passenger);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, listOfPassengers);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message); 
            }
        }

        // GET: api/Passenger/5
        public  HttpResponseMessage Get(Guid id)
        {
                Passenger passenger = GetById(id);
                if (passenger == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Passenger with Id {id} not found.");
                return Request.CreateResponse(HttpStatusCode.OK, passenger);
        }

        // POST: api/Passenger
        public HttpResponseMessage Post([FromBody]Passenger passenger)
        {
            try
            { 
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
     
                    NpgsqlCommand cmd = new NpgsqlCommand("insert into Passenger values(@Id,@FirstName,@LastName,@DateOfBirth);", conn);

                    Guid id = Guid.NewGuid();
                    cmd.Parameters.AddWithValue("Id", id);
                    cmd.Parameters.AddWithValue("FirstName", passenger.FirstName);
                    cmd.Parameters.AddWithValue("LastName", passenger.LastName);
                    cmd.Parameters.AddWithValue("DateOfBirth", passenger.DateOfBirth);

                    conn.Open();

                    int numberOfAffectedRows = cmd.ExecuteNonQuery();

                    if (numberOfAffectedRows > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.Created,$"The number of affected rows is : {numberOfAffectedRows}");
                    }
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message); 
            }

            
        }

        // PUT: api/Passenger/5
        public HttpResponseMessage Put(Guid id, [FromBody] Passenger updatedPassenger)
        {
            try
            { 
                Passenger currentPassenger = GetById(id);

                if (currentPassenger != null)
                {
                    using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                    {
                        StringBuilder sb = new StringBuilder();

                        NpgsqlCommand cmd = new NpgsqlCommand();

                        cmd.Connection = conn;

                        sb.Append("update Passenger set ");
                        if (updatedPassenger.FirstName != null & currentPassenger.FirstName != updatedPassenger.FirstName)
                        { 
                            sb.Append("FirstName = @FirstName, ");
                            cmd.Parameters.AddWithValue("FirstName", updatedPassenger.FirstName);
                        }
                        if (updatedPassenger.LastName != null & currentPassenger.LastName != updatedPassenger.LastName)
                        {
                            sb.Append("LastName = @LastName, ");
                            cmd.Parameters.AddWithValue("LastName", updatedPassenger.LastName);
                        }
                        if (updatedPassenger.DateOfBirth != null & currentPassenger.DateOfBirth != updatedPassenger.DateOfBirth)
                        {
                            sb.Append("DateOfBirth = @DateOfBirth, ");
                            cmd.Parameters.AddWithValue("DateOfBirth", updatedPassenger.DateOfBirth);
                        }

                        sb.Remove(sb.Length - 2, 2);

                        sb.Append(" where Id = @Id");
                        cmd.Parameters.AddWithValue("Id", id);

                        conn.Open();
                        
                        cmd.CommandText= sb.ToString();
                        
                        int numberOfAffectedRows = cmd.ExecuteNonQuery();
                        if (numberOfAffectedRows > 0)
                        {
                            return Get(id);
                        }
                        return Request.CreateResponse(HttpStatusCode.BadRequest);
                    }
                }
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Passenger with Id {id} not found.");

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        // DELETE: api/Passenger/5
        public HttpResponseMessage Delete(Guid id)
        {
            Passenger passenger = GetById(id);
            if (passenger != null)
            {
                try
                {
                    using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                    {
                       
                        NpgsqlCommand cmd = new NpgsqlCommand("delete from Passenger where Id = @Id", conn);
                        cmd.Parameters.AddWithValue("Id", id);

                        conn.Open();

                        int numberOfAffectedRows = cmd.ExecuteNonQuery();
                        if (numberOfAffectedRows > 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, $"The number of affected rows is : {numberOfAffectedRows}");
                        }
                        return Request.CreateResponse(HttpStatusCode.BadRequest);
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
                }
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, $"Passenger with Id {id} not found.");
        }
    }
}
