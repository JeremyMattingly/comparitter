using Comparitter.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comparitter.Compare
{
    public class WordCompareResult
    {
        #region Fields

        private const string DBTABLENAME = "WordCompareResult";

        private int mostPopularWordSearchResultID;
        private WordSearchResult mostPopularWordSearchResult;

        private int leastPopularWordSearchResultID;
        private WordSearchResult leastPopularWordSearchResult;

        private int equallyPopularWordSearchResult1ID;
        private int equallyPopularWordSearchResult2ID;
        private List<WordSearchResult> equallyPopularResults;

        #endregion Fields

        #region Public Properties

        public int ID { get; private set; }

        public WordSearchResult MostPopularWordSearchResult
        {
            get
            {
                if (mostPopularWordSearchResult == null)
                {
                    mostPopularWordSearchResult = WordSearchResult.RetrieveWordSearchResult(mostPopularWordSearchResultID);
                }
                return mostPopularWordSearchResult;
            }
            set
            {
                mostPopularWordSearchResult = value;
                mostPopularWordSearchResultID = mostPopularWordSearchResult.ID;
            }
        }

        public WordSearchResult LeastPopularWordSearchResult
        {
            get
            {
                if (leastPopularWordSearchResult == null)
                {
                    leastPopularWordSearchResult = WordSearchResult.RetrieveWordSearchResult(leastPopularWordSearchResultID);
                }
                return leastPopularWordSearchResult;
            }
            set
            {
                leastPopularWordSearchResult = value;
                leastPopularWordSearchResultID = leastPopularWordSearchResult.ID;
            }
        }

        public bool WordsAreEquallyPopular { get; set; }

        public List<WordSearchResult> EquallyPopularResults
        {
            get
            {
                if (equallyPopularResults == null)
                {
                    equallyPopularResults = new List<WordSearchResult>() { WordSearchResult.RetrieveWordSearchResult(equallyPopularWordSearchResult1ID), WordSearchResult.RetrieveWordSearchResult(equallyPopularWordSearchResult2ID) };
                }
                return equallyPopularResults;
            }
            set
            {
                equallyPopularResults = value;
                equallyPopularWordSearchResult1ID = equallyPopularResults[0].ID;
                equallyPopularWordSearchResult2ID = equallyPopularResults[1].ID;
            }
        }

        public double SearchElapsedSeconds { get; set; }

        public DateTime CompareDateTime { get; set; }

        #endregion Public Properties

        #region Constructors

        public WordCompareResult()
        {
            mostPopularWordSearchResultID = -1;
            leastPopularWordSearchResultID = -1;
            equallyPopularWordSearchResult1ID = -1;
            equallyPopularWordSearchResult2ID = -1;

            ID = -1;
        }

        #endregion Constructors

        #region Public Methods

        public static List<WordCompareResult> RetrieveWordCompareResult()
        {
            List<WordCompareResult> items = new List<WordCompareResult>();

            DataTable data = RetrieveRecords();

            if (data.Rows.Count > 0)
            {
                foreach (DataRow itemRow in data.Rows)
                {
                    WordCompareResult item = new WordCompareResult();
                    LoadProperties(item, itemRow);
                    items.Add(item);
                }
            }

            return items;
        }

        public static WordCompareResult RetrieveWordCompareResult(int Id)
        {
            WordCompareResult wordCompareResult = null;

            DataTable data = RetrieveSingleRecord(Id);

            if (data.Rows.Count == 1)
            {
                wordCompareResult = new WordCompareResult();
                LoadProperties(wordCompareResult, data.Rows[0]);
            }

            return wordCompareResult;
        }

        public WordCompareResult Save()
        {
            SqlCommand cmd = GenericDataAccess.CreateCommand(Database.Utility.ConnectionString);
            SqlParameter idParam = new SqlParameter("@ID", SqlDbType.Int);
            cmd.Parameters.Add(idParam);

            if (ID != -1)
            {
                // This WordCompareResult already exists. We'll do an UPDATE.
                cmd.CommandText = @"UPDATE " + DBTABLENAME + @"
                                    SET MostPopularWordSearchResultID = @MostPopularWordSearchResultID,
                                        LeastPopularWordSearchResultID = @LeastPopularWordSearchResultID,
                                        WordsAreEquallyPopular = @WordsAreEquallyPopular,
                                        EquallyPopularWordSearchResult1ID = @EquallyPopularWordSearchResult1ID,
                                        EquallyPopularWordSearchResult2ID = @EquallyPopularWordSearchResult2ID,
                                        SearchElapsedSeconds = @SearchElapsedSeconds,
                                        CompareDateTime = @CompareDateTime,
                                    WHERE ID = @ID";
                idParam.Value = ID;
            }
            else
            {
                // This Customer is new. We'll do an INSERT.
                cmd.CommandText = @"INSERT INTO " + DBTABLENAME + @" (MostPopularWordSearchResultID, LeastPopularWordSearchResultID, WordsAreEquallyPopular, EquallyPopularWordSearchResult1ID, EquallyPopularWordSearchResult2ID, SearchElapsedSeconds, CompareDateTime)
                                    VALUES (@MostPopularWordSearchResultID, @LeastPopularWordSearchResultID, @WordsAreEquallyPopular, @EquallyPopularWordSearchResult1ID, @EquallyPopularWordSearchResult2ID, @SearchElapsedSeconds, @CompareDateTime)
                                    SET @ID = SCOPE_IDENTITY()";
                idParam.Direction = ParameterDirection.Output;
            }

            // Add any necessary parameters here

            cmd.Parameters.AddWithValue("@MostPopularWordSearchResultID", (!WordsAreEquallyPopular) ? MostPopularWordSearchResult.ID : -1);
            cmd.Parameters.AddWithValue("@LeastPopularWordSearchResultID", (!WordsAreEquallyPopular) ? LeastPopularWordSearchResult.ID : -1);
            cmd.Parameters.AddWithValue("@WordsAreEquallyPopular", WordsAreEquallyPopular);
            cmd.Parameters.AddWithValue("@EquallyPopularWordSearchResult1ID", (WordsAreEquallyPopular) ? equallyPopularWordSearchResult1ID : -1);
            cmd.Parameters.AddWithValue("@EquallyPopularWordSearchResult2ID", (WordsAreEquallyPopular) ? equallyPopularWordSearchResult2ID : -1);
            cmd.Parameters.AddWithValue("@SearchElapsedSeconds", SearchElapsedSeconds);
            cmd.Parameters.AddWithValue("@CompareDateTime", CompareDateTime);

            try
            {
                int numRows = GenericDataAccess.ExecuteNonQuery(cmd);
                if (numRows == 1)
                {
                    if (ID == -1)
                    {
                        ID = (int)idParam.Value;
                    }
                }
            }
            catch (System.Exception ex)
            {
                // TODO: Log
                throw;
            }

            return this;
        }

        #endregion Public Methods

        #region Private Methods

        private static DataTable RetrieveRecords()
        {
            SqlCommand cmd = GenericDataAccess.CreateCommand(Database.Utility.ConnectionString);
            cmd.CommandText = @"SELECT ID, MostPopularWordSearchResultID, LeastPopularWordSearchResultID, WordsAreEquallyPopular, EquallyPopularWordSearchResult1ID, EquallyPopularWordSearchResult2ID, SearchElapsedSeconds, CompareDateTime
                            FROM " + DBTABLENAME + @"
                            ORDER BY CompareDateTime DESC";
            return GenericDataAccess.ExecuteSelectCommand(cmd);
        }

        private static DataTable RetrieveSingleRecord(int id)
        {
            SqlCommand cmd = GenericDataAccess.CreateCommand(Database.Utility.ConnectionString);
            cmd.CommandText = @"SELECT ID, MostPopularWordSearchResultID, LeastPopularWordSearchResultID, WordsAreEquallyPopular, EquallyPopularWordSearchResult1ID, EquallyPopularWordSearchResult2ID, SearchElapsedSeconds, CompareDateTime
                                FROM " + DBTABLENAME + @"
                                WHERE ID = @ID";
            cmd.Parameters.AddWithValue("@ID", id);
            return GenericDataAccess.ExecuteSelectCommand(cmd);
        }

        private static void LoadProperties(WordCompareResult item, DataRow row)
        {
            item.ID = row.Field<int>("ID");
            item.mostPopularWordSearchResultID = row.Field<int>("MostPopularWordSearchResultID");
            item.leastPopularWordSearchResultID = row.Field<int>("LeastPopularWordSearchResultID");
            item.WordsAreEquallyPopular = row.Field<bool>("WordsAreEquallyPopular");
            if (row["EquallyPopularWordSearchResult1ID"] != DBNull.Value)
            {
                item.equallyPopularWordSearchResult1ID = row.Field<int>("EquallyPopularWordSearchResult1ID");
            }
            else
            {
                item.equallyPopularWordSearchResult1ID = -1;
            }
            if (row["EquallyPopularWordSearchResult2ID"] != DBNull.Value)
            {
                item.equallyPopularWordSearchResult2ID = row.Field<int>("EquallyPopularWordSearchResult2ID");
            }
            else
            {
                item.equallyPopularWordSearchResult2ID = -1;
            }
            item.SearchElapsedSeconds = row.Field<double>("SearchElapsedSeconds");
            item.CompareDateTime = row.Field<DateTime>("CompareDateTime");
        }

        #endregion Private Methods
    }
}
