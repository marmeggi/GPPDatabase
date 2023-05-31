using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GPPDatabase.Models
{
    public class Passenger
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }

    }
}