using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comparitter.Compare
{
    public class CompareHistory
    {
        public static void SaveWordCompareResult(WordCompareResult wordCompareResult)
        {
            // save compare object
            // save word 1 object
            // save word 2 object
        }

        public static List<string> RetrieveWordCompareHistory()
        {
            var cmd = Database.GenericDataAccess.CreateCommand(Database.Utility.ConnectionString);

            cmd.CommandText = @"SELECT * FROM WordCompareResult";

            var results = Database.GenericDataAccess.ExecuteSelectCommand(cmd);

            List<string> datesToReturn = new List<string>();

            foreach (DataRow result in results.Rows)
            {
                string newDate = result.Field<DateTime>("CompareDateTime").ToShortDateString();
                datesToReturn.Add(newDate);
            }

            return datesToReturn;
        }
    }
}
