using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GPPDatabase.WebApi.Models
{
    public class PassengerRest
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string CityOfResidence { get; set; }
        public Guid EmploymentStatusId { get; set; }


    }
}