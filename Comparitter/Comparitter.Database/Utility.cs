using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Comparitter.Database
{
    public static class Utility
    {
        #region Private Fields

        private const string CONNECTIONSTRINGNAME = "Comparitter";

        #endregion Private Fields

        #region Public Properties

        public static string ConnectionString { get; private set; }

        #endregion Public Properties

        #region Constructors

        static Utility()
        {
            SetStaticProperties();
        }

        #endregion Constructors

        #region Private Methods

        private static void SetStaticProperties()
        {
            SetConnectionString();
        }

        private static void SetConnectionString()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings[CONNECTIONSTRINGNAME].ConnectionString;

            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                //TODO: Log ("Comparitter Database Connection String not set.");
            }
        }

        #endregion Private Methods
    }
}