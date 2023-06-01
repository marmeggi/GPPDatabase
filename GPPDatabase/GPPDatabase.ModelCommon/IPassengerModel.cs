using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPPDatabase.ModelCommon
{
    public interface IPassengerModel
    {
        Guid Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        DateTime? DateOfBirth { get; set; }

    }
}
