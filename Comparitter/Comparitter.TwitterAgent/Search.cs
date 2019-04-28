﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Parameters;

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

        public static List<string> GetTopThreeTweetTextByPhrase(string phrase)
        {
            var matchingTweets = Tweetinvi.Search.SearchTweets(phrase).ToList();

            List<string> tweetsToReturn = new List<string>();

            for (int i = 0; i < 3; i++)
            {
                tweetsToReturn.Add(matchingTweets[i].Text);
            }

            return tweetsToReturn;
        }

        public static IEnumerable<Tweetinvi.Models.ITweet> SearchByPhrase(string phrase)
        {
            DateTime dateTimeNow = DateTime.Now;
            DateTime oneDayAgo = dateTimeNow.AddDays(-1);
            int maxNumberOfResults = 100;

            List<Tweetinvi.Models.ITweet> tweetsToReturn = new List<Tweetinvi.Models.ITweet>();
            long lastMaxId = 0;
            bool twitterIsStillReturningResults = true;

            int numberOfLoops = 0;

            while (twitterIsStillReturningResults)
            {
                SearchTweetsParameters searchParameters;

                if (lastMaxId == 0)
                {
                    searchParameters = new SearchTweetsParameters(phrase)
                    {
                        MaximumNumberOfResults = maxNumberOfResults//,
                        //Since = oneDayAgo
                        //Until = dateTimeNow
                    };
                }
                else
                {
                    searchParameters = new SearchTweetsParameters(phrase)
                    {
                        MaximumNumberOfResults = maxNumberOfResults,
                        //Since = oneDayAgo,
                        //Until = dateTimeNow,
                        MaxId = lastMaxId,
                    };
                }

                var searchResults = Tweetinvi.Search.SearchTweets(searchParameters);

                var searchResultsList = searchResults.ToList();

                twitterIsStillReturningResults = searchResultsList.Count > 0;

                if (twitterIsStillReturningResults)
                {

                    lastMaxId = searchResultsList[searchResultsList.Count - 1].Id - 1;

                    tweetsToReturn.AddRange(searchResultsList);

                }

                numberOfLoops++;
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
