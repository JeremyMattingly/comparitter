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
    public class WordSearchResult
    {
        #region Fields

        private const string DBTABLENAME = "WordSearchResult";

        #endregion Fields

        #region Public Properties

        public int ID { get; private set; }
        public string Word { get; set; }
        public DateTime? OldestTweetDateTime { get; set; }
        public DateTime? NewestTweetDateTime { get; set; }
        public int AppearanceCount { get; set; }
        public bool SearchFailed { get; set; }
        public DateTime SearchDateTime { get; set; }

        #endregion Public Properties

        #region Constructors

        public WordSearchResult()
        {
            ID = -1;
        }

        #endregion Constructors

        #region Public Methods

        public static List<WordSearchResult> RetrieveWordSearchResults()
        {
            List<WordSearchResult> items = new List<WordSearchResult>();

            DataTable data = RetrieveRecords();

            if (data.Rows.Count > 0)
            {
                foreach (DataRow itemRow in data.Rows)
                {
                    WordSearchResult item = new WordSearchResult();
                    LoadProperties(item, itemRow);
                    items.Add(item);
                }
            }

            return items;
        }

        public static WordSearchResult RetrieveWordSearchResult(int Id)
        {
            WordSearchResult wordSearchResult = null;

            DataTable data = RetrieveSingleRecord(Id);

            if (data.Rows.Count == 1)
            {
                wordSearchResult = new WordSearchResult();
                LoadProperties(wordSearchResult, data.Rows[0]);
            }

            return wordSearchResult;
        }

        public WordSearchResult Save()
        {
            SqlCommand cmd = GenericDataAccess.CreateCommand(Database.Utility.ConnectionString);
            SqlParameter idParam = new SqlParameter("@ID", SqlDbType.Int);
            cmd.Parameters.Add(idParam);

            if (ID != -1)
            {
                // This WordSearchResult already exists. We'll do an UPDATE.
                cmd.CommandText = @"UPDATE " + DBTABLENAME + @"
                                    SET Word = @Word,
                                        AppearanceCount = @AppearanceCount,
                                        OldestTweetDateTime = @OldestTweetDateTime,
                                        NewestTweetDateTime = @NewestTweetDateTime,
                                        SearchFailed = @SearchFailed,
                                        SearchDateTime = @SearchDateTime
                                    WHERE ID = @ID";
                idParam.Value = ID;
            }
            else
            {
                // This WordSearchResult is new. We'll do an INSERT.
                cmd.CommandText = @"INSERT INTO " + DBTABLENAME + @" (Word, AppearanceCount, OldestTweetDateTime, NewestTweetDateTime, SearchFailed, SearchDateTime)
                                    VALUES (@Word, @AppearanceCount, @OldestTweetDateTime, @NewestTweetDateTime, @SearchFailed, @SearchDateTime)
                                    SET @ID = SCOPE_IDENTITY()";
                idParam.Direction = ParameterDirection.Output;
            }

            // Add any necessary parameters here

            cmd.Parameters.AddWithValue("@Word", Word);
            cmd.Parameters.AddWithValue("@AppearanceCount", AppearanceCount);
            SqlParameter oldestTweetDateTimeParameter = new SqlParameter
            {
                ParameterName = "@OldestTweetDateTime"
            };
            if (OldestTweetDateTime != null)
            {
                oldestTweetDateTimeParameter.Value = OldestTweetDateTime.Value;
            }
            else
            {
                oldestTweetDateTimeParameter.Value = DBNull.Value;
            }
            cmd.Parameters.Add(oldestTweetDateTimeParameter);
            SqlParameter newestTweetDateTimeParameter = new SqlParameter()
            {
                ParameterName = "@NewestTweetDateTime"
            };
            if (NewestTweetDateTime != null)
            {
                newestTweetDateTimeParameter.Value = NewestTweetDateTime.Value;
            }
            else
            {
                newestTweetDateTimeParameter.Value = DBNull.Value;
            }
            cmd.Parameters.Add(newestTweetDateTimeParameter);
            cmd.Parameters.AddWithValue("@SearchFailed", SearchFailed);
            cmd.Parameters.AddWithValue("@SearchDateTime", SearchDateTime);

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
            cmd.CommandText = @"SELECT ID, Word, AppearanceCount, OldestTweetDateTime, NewestTweetDateTime, SearchFailed, SearchDateTime
                            FROM " + DBTABLENAME + @"
                            ORDER BY SearchDateTime DESC";
            return GenericDataAccess.ExecuteSelectCommand(cmd);
        }

        private static DataTable RetrieveSingleRecord(int id)
        {
            SqlCommand cmd = GenericDataAccess.CreateCommand(Database.Utility.ConnectionString);
            cmd.CommandText = @"SELECT ID, Word, AppearanceCount, OldestTweetDateTime, NewestTweetDateTime, SearchFailed, SearchDateTime
                                FROM " + DBTABLENAME + @"
                                WHERE ID = @ID";
            cmd.Parameters.AddWithValue("@ID", id);
            return GenericDataAccess.ExecuteSelectCommand(cmd);
        }

        private static void LoadProperties(WordSearchResult item, DataRow row)
        {
            item.ID = row.Field<int>("ID");
            item.Word = row.Field<string>("Word");
            item.AppearanceCount = row.Field<int>("AppearanceCount");
            if (row["OldestTweetDateTime"] != DBNull.Value)
            {
                item.OldestTweetDateTime = row.Field<DateTime>("OldestTweetDateTime");
            }
            else
            {
                item.OldestTweetDateTime = null;
            }
            if (row["NewestTweetDateTime"] != DBNull.Value)
            {
                item.NewestTweetDateTime = row.Field<DateTime>("NewestTweetDateTime");
            }
            else
            {
                item.NewestTweetDateTime = null;
            }
            item.SearchFailed = row.Field<bool>("SearchFailed");
            item.SearchDateTime = row.Field<DateTime>("SearchDateTime");
        }

        #endregion Private Methods
    }
}
