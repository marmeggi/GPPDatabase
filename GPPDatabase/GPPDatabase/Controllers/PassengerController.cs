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
        public static string ConnectionString = "Server=localhost;Port=5432;User Id=postgres;Password=bootcamp;Database=postgres;";

        private Passenger GetById(Guid Id)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand("select * from Passenger where Id = @Id", conn);
                cmd.Parameters.AddWithValue("Id", Id);

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
                Console.WriteLine($"Passenger with Id {Id} not found.");
                return null;
            }
        }


        // GET: api/Passenger
        public HttpResponseMessage Get()
        {   
            List<Passenger> ListOfPassengers = new List<Passenger>();
            try
            { 
                using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
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

                        ListOfPassengers.Add(passenger);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, ListOfPassengers);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message); 
            }
        }

        // GET: api/Passenger/5
        public  HttpResponseMessage Get(Guid Id)
        {
                Passenger passenger = GetById(Id);
                if (passenger == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Passenger with Id {Id} not found.");
                return Request.CreateResponse(HttpStatusCode.OK, passenger);
        }

        // POST: api/Passenger
        public HttpResponseMessage Post([FromBody]Passenger passenger)
        {
            try
            { 
                using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    NpgsqlCommand cmd = new NpgsqlCommand("insert into Passenger values(@Id,@FirstName,@LastName,@DateOfBirth);", conn);

                    Guid Id = Guid.NewGuid();
                    cmd.Parameters.AddWithValue("Id", Id);
                    cmd.Parameters.AddWithValue("FirstName", passenger.FirstName);
                    cmd.Parameters.AddWithValue("LastName", passenger.LastName);
                    cmd.Parameters.AddWithValue("DateOfBirth", passenger.DateOfBirth);

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
        public HttpResponseMessage Put(Guid Id, [FromBody] Passenger newPassenger)
        {
            try
            { 
                Passenger oldPassenger = GetById(Id);

                if (oldPassenger != null)
                {
                    using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
                    {
                        StringBuilder sb = new StringBuilder();

                        sb.Append("update Passenger set ");
                        if (newPassenger.FirstName != null & oldPassenger.FirstName != newPassenger.FirstName)
                            sb.Append("FirstName = @FirstName, ");
                        if (newPassenger.LastName != null & oldPassenger.LastName != newPassenger.LastName)
                            sb.Append("LastName = @LastName, ");
                        if (newPassenger.DateOfBirth != null & oldPassenger.DateOfBirth != newPassenger.DateOfBirth)
                            sb.Append("DateOfBirth = @DateOfBirth, ");
                        sb.Remove(sb.Length - 2, 2);
                        sb.Append(" where Id = @Id");

                        conn.Open();

                        NpgsqlCommand cmd = new NpgsqlCommand(sb.ToString(), conn);

                        cmd.Parameters.AddWithValue("Id", Id);
                        if (newPassenger.FirstName != null)
                            cmd.Parameters.AddWithValue("FirstName", newPassenger.FirstName);
                        if (newPassenger.LastName != null)
                            cmd.Parameters.AddWithValue("LastName", newPassenger.LastName);
                        if (newPassenger.DateOfBirth != null)
                            cmd.Parameters.AddWithValue("DateOfBirth", newPassenger.DateOfBirth);

                        int numberOfAffectedRows = cmd.ExecuteNonQuery();
                        if (numberOfAffectedRows > 0)
                        {
                            return Get(Id);
                        }
                        return Request.CreateResponse(HttpStatusCode.BadRequest);
                    }
                }
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Passenger with Id {Id} not found.");

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        // DELETE: api/Passenger/5
        public HttpResponseMessage Delete(Guid Id)
        {
            Passenger passenger = GetById(Id);
            if (passenger != null)
            {
                try
                {
                    using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
                    {
                        conn.Open();
                        NpgsqlCommand cmd = new NpgsqlCommand("delete from Passenger where Id = @Id", conn);
                        cmd.Parameters.AddWithValue("Id", Id);
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
            return Request.CreateResponse(HttpStatusCode.NotFound, $"Passenger with Id {Id} not found.");
        }
    }
}
