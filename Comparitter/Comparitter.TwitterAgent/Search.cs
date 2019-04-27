using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;

namespace Comparitter.TwitterAgent
{
    /// <summary>
    /// Search Twitter
    /// </summary>
    public class Search
    {
        #region Properties

        private static Credentials TwitterCredentials { get; set; }

        #endregion Properties

        #region Constructors

        static Search()
        {
            SetTwitterCredentials();
            Auth.SetUserCredentials(TwitterCredentials.ConsumerKey, TwitterCredentials.ConsumerSecret, TwitterCredentials.UserAccessToken, TwitterCredentials.UserAccessSecret);
        }

        #endregion Constructors

        #region Public Methods

        public static List<string> FirstContact()
        {
            var user = User.GetAuthenticatedUser();
            var timelineTweets = Timeline.GetUserTimeline(user, 5);

            List<string> tweetsToReturn = new List<string>();

            foreach (var tweet in timelineTweets)
            {
                tweetsToReturn.Add(tweet.Text);
            }

            return tweetsToReturn;
        }

        public static List<string> SearchByPhrase(string phrase)
        {
            var matchingTweets = Tweetinvi.Search.SearchTweets(phrase).ToList();

            List<string> tweetsToReturn = new List<string>();

            for (int i = 0; i < 3; i++)
            {
                tweetsToReturn.Add(matchingTweets[i].Text);
            }

            return tweetsToReturn;
        }

        #endregion Public Methods

        #region Private Methods

        private static void SetTwitterCredentials()
        {
            string consumerKey, consumerSecret, userAccessToken, userAccessSecret;

            consumerKey = ConfigurationManager.AppSettings["TwitterConsumerKey"];
            consumerSecret = ConfigurationManager.AppSettings["TwitterConsumerSecret"];
            userAccessToken = ConfigurationManager.AppSettings["TwitterUserAccessToken"];
            userAccessSecret = ConfigurationManager.AppSettings["TwitterUserAccessSecret"];

            TwitterCredentials = new TwitterAgent.Credentials { ConsumerKey = consumerKey, ConsumerSecret = consumerSecret, UserAccessToken = userAccessToken, UserAccessSecret = userAccessSecret };
        }

        #endregion Private Methods
    }
}
