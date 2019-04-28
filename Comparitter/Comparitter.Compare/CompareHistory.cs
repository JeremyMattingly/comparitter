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
            if (wordCompareResult.WordsAreEquallyPopular)
            {
                List<WordSearchResult> savedWordSearchResults = new List<WordSearchResult>();

                foreach (var item in wordCompareResult.EquallyPopularResults)
                {
                    WordSearchResult savedResult = item.Save();
                    savedWordSearchResults.Add(savedResult);
                }

                wordCompareResult.EquallyPopularResults = savedWordSearchResults;
            }
            else
            {
                wordCompareResult.MostPopularWordSearchResult = wordCompareResult.MostPopularWordSearchResult.Save();
                wordCompareResult.LeastPopularWordSearchResult = wordCompareResult.LeastPopularWordSearchResult.Save();
            }

            wordCompareResult.Save();
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
