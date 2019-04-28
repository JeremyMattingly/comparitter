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

        public static List<WordCompareResult> RetrieveWordCompareHistory()
        {
            var cmd = Database.GenericDataAccess.CreateCommand(Database.Utility.ConnectionString);

            cmd.CommandText = @"SELECT * 
                                FROM WordCompareResult
                                ORDER BY CompareDateTime DESC";

            var results = Database.GenericDataAccess.ExecuteSelectCommand(cmd);

            List<WordCompareResult> WordCompareResultsToReturn = new List<WordCompareResult>();

            foreach (DataRow result in results.Rows)
            {
                WordCompareResult savedResult = WordCompareResult.RetrieveWordCompareResult(result.Field<int>("ID"));
                WordCompareResultsToReturn.Add(savedResult);
            }

            return WordCompareResultsToReturn;
        }
    }
}
