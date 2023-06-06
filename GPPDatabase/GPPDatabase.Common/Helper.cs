using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GPPDatabase.Common
{
    public class Helper
    {

       public static List<Guid> ToGuidList(string listOfStrings)
        {
            List<Guid> listOfGuids = new List<Guid>();
            string[] arrayOfStrings = listOfStrings.Split(',');
            for( int i = 0; i < arrayOfStrings.Length; i++ )
            {
                listOfGuids.Add(new Guid(arrayOfStrings[i]));
            }
            return listOfGuids;
        }
       public static string JoinGuid(List<Guid>  listOfGuids)
        {
            string listOfStrings = " ";

            foreach( Guid guid in listOfGuids ) 
            {
                listOfStrings+="'"+Convert.ToString(guid)+"',";
            }
            
            return listOfStrings.TrimEnd(',');

        }
       
    }
}
