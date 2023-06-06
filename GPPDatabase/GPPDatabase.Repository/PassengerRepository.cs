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
using GPPDatabase.Common;
using PagedList;


namespace GPPDatabase.Repository
{
    public class PassengerRepository : IPassengerRepository
    {
        public static string connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=bootcamp;Database=postgres;";


        public async Task<Passenger> GetByIdAsync(Guid id)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand cmd = new NpgsqlCommand("select * from Passenger where Id = @Id", conn);
                cmd.Parameters.AddWithValue("Id", id);

                conn.Open();

                NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

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

        /*
        public async Task<List<Passenger>> GetAllPassengersAsync()
        {
            List<Passenger> listOfPassengers = new List<Passenger>();

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    NpgsqlCommand cmd = new NpgsqlCommand("select * from Passenger; ", conn);

                    NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
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
        */

        public async Task<Common.PagedList<Passenger>> GetPassengersAsync(Filtering filtering, Paging paging, Sorting sorting)
        {
            List<Passenger> listOfPassengers = new List<Passenger>();

            List<string> allowedSortColumns = new List<string> { "firstname", "lastname", "dateofbirth", "cityofresidence" };

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                StringBuilder sb = new StringBuilder();

                NpgsqlCommand cmd = new NpgsqlCommand();

                cmd.Connection = conn;

                sb.Append("select * from passenger ");

                if (filtering !=  null)
                {
                    sb.Append("where 1 = 1 ");

                    if (filtering.SearchQuery != null)
                    {
                        string searchQuery = filtering.SearchQuery;
                        sb.Append($" and ((FirstName like @SearchQuery) or (LastName like @SearchQuery) or " +
                            $"(DateOfBirth like @SearchQuery) or (CityOfResidence like @SearchQuery)) ");
                        cmd.Parameters.AddWithValue("@SearchQuery", "%"+filtering.SearchQuery+"%");
                    }

                    if (filtering.EmploymentStatuses != null && filtering.EmploymentStatuses.Any())
                    {
                        /*
                        sb.Append($" and  EmploymentStatusid in (@statusIds)");

                        string statusIds = Helper.JoinGuid(filtering.EmploymentStatuses);
                        
                        cmd.Parameters.AddWithValue("@statusIds",statusIds);
                       */
                        sb.Append(" AND EmploymentStatusid IN (");

                        List<string> statusIdParams = new List<string>();
                        foreach (Guid statusId in filtering.EmploymentStatuses)
                        {
                            string param = $"@statusId{statusIdParams.Count}";
                            statusIdParams.Add(param);
                            cmd.Parameters.AddWithValue(param, statusId);
                        }

                        sb.Append(string.Join(",", statusIdParams));
                        sb.Append(")");
                    }
                }

                if (filtering.MinDateOfBirth != null)
                {
                    sb.Append($" and (DateOfBirth >= @MinDateOfBirth) ");
                    cmd.Parameters.AddWithValue("MinDateOfBirth", filtering.MinDateOfBirth);
                }

                if (filtering.MaxDateOfBirth != null)
                {
                    sb.Append($" and (DateOfBirth <= @MaxDateOfBirth) ");
                    cmd.Parameters.AddWithValue("MaxDateOfBirth", filtering.MaxDateOfBirth);
                }
                
 

                if ( allowedSortColumns.Contains(sorting.OrderBy.ToLower()))
                {
                    sb.Append($" order by {sorting.OrderBy} ");
                }

                if ( (sorting.SortOrder.ToLower() == "asc") || (sorting.SortOrder.ToLower() == "desc") )
                {
                    sb.Append($" {sorting.SortOrder} ");
                }

                sb.Append(" offset @offset rows \r\n fetch next @PageSize rows only");

                cmd.Parameters.AddWithValue("@offset", (paging.PageNumber-1)*paging.PageSize);
                cmd.Parameters.AddWithValue("@PageSize", paging.PageSize);


                conn.Open();

                cmd.CommandText = sb.ToString();

                NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Passenger passenger = new Passenger
                        {
                            Id = reader.GetGuid(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            DateOfBirth = reader.GetDateTime(3),
                            CityOfResidence = reader.GetString(4),
                            EmploymentStatusId = reader.GetGuid(5)
                        };

                        listOfPassengers.Add(passenger);
                    }
                }

                conn.Close();

                NpgsqlCommand cmd2 = new NpgsqlCommand();

                StringBuilder sb2 = new StringBuilder();

                cmd2.Connection = conn;

                    sb2.Append("select count (*) from passenger ");

                    if (filtering != null)
                    {
                        sb2.Append("where 1 = 1 ");

                        if (filtering.SearchQuery != null)
                        {
                            string searchQuery = filtering.SearchQuery;
                            sb2.Append($" and ((FirstName like @SearchQuery) or (LastName like @SearchQuery) or " +
                                $"(DateOfBirth like @SearchQuery) or (CityOfResidence like @SearchQuery)) ");
                            cmd2.Parameters.AddWithValue("@SearchQuery", "%" + filtering.SearchQuery + "%");
                        }

                        if (filtering.EmploymentStatuses != null && filtering.EmploymentStatuses.Any())
                        {
                        /*
                        sb.Append($" and  EmploymentStatusid in (@statusIds)");

                        string statusIds = Helper.JoinGuid(filtering.EmploymentStatuses);

                        cmd.Parameters.AddWithValue("@statusIds", statusIds);
                        */
                        List<string> statusIdParams = new List<string>();
                        foreach (Guid statusId in filtering.EmploymentStatuses)
                        {
                            string param = $"@statusId{statusIdParams.Count}";
                            statusIdParams.Add(param);
                            cmd.Parameters.AddWithValue(param, statusId);
                        }

                        sb.Append(string.Join(",", statusIdParams));
                        sb.Append(")");

                        }

                        if (filtering.MinDateOfBirth != null)
                        {
                            sb2.Append($" and (DateOfBirth >= @MinDateOfBirth) ");
                            cmd2.Parameters.AddWithValue("MinDateOfBirth", filtering.MinDateOfBirth);
                        }

                        if (filtering.MaxDateOfBirth != null)
                        {
                            sb2.Append($" and (DateOfBirth <= @MaxDateOfBirth) ");
                            cmd2.Parameters.AddWithValue("MaxDateOfBirth", filtering.MaxDateOfBirth);
                        }
                    }
                    conn.Open();



                    cmd2.CommandText = sb2.ToString();

                    int numberOfRows= Convert.ToInt16(await cmd2.ExecuteScalarAsync());

                   
     
                    return new Common.PagedList<Passenger>(listOfPassengers, paging.PageNumber, paging.PageSize, numberOfRows);
                    
                   
            }
        }

        // GET: api/Passenger/5
        public async Task<Passenger> GetPassengerByIdAsync(Guid id)
        {
            Passenger passenger = await GetByIdAsync(id);
            if (passenger == null)
                return null;
            return passenger;
        }

        // POST: api/Passenger
        public async Task<bool> CreatePassengerAsync(Passenger passenger)
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

                        int numberOfAffectedRows = await cmd.ExecuteNonQueryAsync();

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
        public async Task<Passenger> UpdatePassengerAsync(Guid id, Passenger updatedPassenger)
        {
            try
            {
                Passenger currentPassenger = await GetByIdAsync(id);

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

                        int numberOfAffectedRows = await cmd.ExecuteNonQueryAsync();
                        if (numberOfAffectedRows > 0)
                        {
                            Passenger getPassengerByIdAsyncResult = await GetPassengerByIdAsync(id);
                            return getPassengerByIdAsyncResult;
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
        public async Task<bool> DeletePassengerAsync(Guid id)
        {
            Passenger passenger = await GetByIdAsync(id);
            if (passenger != null)
            {
                try
                {
                    using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                    {

                        NpgsqlCommand cmd = new NpgsqlCommand("delete from Passenger where Id = @Id", conn);
                        cmd.Parameters.AddWithValue("Id", id);

                        conn.Open();

                        int numberOfAffectedRows = await cmd.ExecuteNonQueryAsync();
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
