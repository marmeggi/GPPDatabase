using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPPDatabase.Common
{
    public class Filtering
    {
        public string SearchQuery { get; set; } = null;
       public DateTime? MinDateOfBirth { get; set; }
       public DateTime? MaxDateOfBirth { get; set; }

       public List<Guid> EmploymentStatuses { get; set; } = null;
    }
}
