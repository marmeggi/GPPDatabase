using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using GPPDatabase.Model;
using GPPDatabase.RepositoryCommon;


namespace GPPDatabase.Repository
{
    public class PassengerRepository:IPassengerRepository
    {
        public static string connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=bootcamp;Database=postgres;";

        public Passenger GetById(Guid id)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand cmd = new NpgsqlCommand("select * from Passenger where Id = @Id", conn);
                cmd.Parameters.AddWithValue("Id", id);

                conn.Open();

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
                return null;
            }
        }


        // GET: api/Passenger
        public List<Passenger> GetAllPassengers()
        {
            List<Passenger> listOfPassengers = new List<Passenger>();

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    NpgsqlCommand cmd = new NpgsqlCommand("select * from Passenger; ", conn);

                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
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
                }
                return listOfPassengers;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // GET: api/Passenger/5
        public Passenger GetPassengerById(Guid id)
        {
            Passenger passenger = GetById(id);
            if (passenger == null)
                return null;
            return passenger;
        }

        // POST: api/Passenger
        public bool CreatePassenger(Passenger passenger)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {

                    NpgsqlCommand cmd = new NpgsqlCommand("insert into Passenger values(@Id,@FirstName,@LastName,@DateOfBirth);", conn);
                    if (passenger != null)
                    {
                        Guid id = Guid.NewGuid();
                        cmd.Parameters.AddWithValue("Id", id);
                        cmd.Parameters.AddWithValue("FirstName", passenger.FirstName);
                        cmd.Parameters.AddWithValue("LastName", passenger.LastName);
                        cmd.Parameters.AddWithValue("DateOfBirth", passenger.DateOfBirth);

                        conn.Open();

                        int numberOfAffectedRows = cmd.ExecuteNonQuery();

                        if (numberOfAffectedRows > 0)
                        {
                            return true;
                        }
                        return false;
                    }
                    return false;
                }

            }
            catch (Exception)
            {
                return false;
            }


        }

        // PUT: api/Passenger/5
        public Passenger UpdatePassenger(Guid id, Passenger updatedPassenger)
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

                        cmd.CommandText = sb.ToString();

                        int numberOfAffectedRows = cmd.ExecuteNonQuery();
                        if (numberOfAffectedRows > 0)
                        {
                            return GetPassengerById(id);
                        }
                        return null;
                    }
                }
                return null;

            }
            catch (Exception)
            {
                return null;
            }

        }

        // DELETE: api/Passenger/5
        public bool DeletePassenger(Guid id)
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
                            return true;
                        }
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }
    }
}
