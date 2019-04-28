using System;
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
            TwitterCredentials = GetTwitterCredentials();
            SetupConnectionToTwitter();
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Search Twitter for tweets that contain the specified search word.
        /// </summary>
        /// <param name="searchWord">The word to search twitter for.</param>
        /// <returns><see cref="IEnumerable{Tweetinvi.Models.ITweet}"/> of Tweets that contain the specifed search word.</returns>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="searchWord"/> is null or is only whitespace.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="searchWord"/> length is over 500.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="searchWord"/> contains any space. Method does not support searching for phrases.</exception>
        /// <exception cref="Tweetinvi.Exceptions.TwitterException">Thrown when the Twitter library encounters an error from a bad request, network issue or bad argument.</exception>
        public static IEnumerable<Tweetinvi.Models.ITweet> SearchByWord(string searchWord)
        {
            if (string.IsNullOrWhiteSpace(searchWord))
            {
                throw new ArgumentException("Phrase cannot be null or only whitespace.", nameof(searchWord));
            }

            if (searchWord.Length > 500)
            {
                throw new ArgumentOutOfRangeException(nameof(searchWord), "Phrase is limited to 500 characters.");
            }

            if (searchWord.Contains(" "))
            {
                throw new ArgumentException("SearchByWord() does not support searching for phrases. Only single words are allowed.", nameof(searchWord));
            }

            List<Tweetinvi.Models.ITweet> tweetsToReturn = new List<Tweetinvi.Models.ITweet>();
            int maxNumberOfResults = 100;
            long lastMaxId = 0;
            bool twitterIsStillReturningResults = true;

            while (twitterIsStillReturningResults)
            {
                SearchTweetsParameters searchParameters;

                if (lastMaxId == 0)
                {
                    searchParameters = new SearchTweetsParameters(searchWord)
                    {
                        MaximumNumberOfResults = maxNumberOfResults
                    };
                }
                else
                {
                    searchParameters = new SearchTweetsParameters(searchWord)
                    {
                        MaximumNumberOfResults = maxNumberOfResults,
                        MaxId = lastMaxId,
                    };
                }

                try
                {
                    IEnumerable<Tweetinvi.Models.ITweet> searchResults = Tweetinvi.Search.SearchTweets(searchParameters);

                    twitterIsStillReturningResults = searchResults != null && searchResults.Count() > 0;

                    if (twitterIsStillReturningResults)
                    {
                        List<Tweetinvi.Models.ITweet> searchResultsList = searchResults.ToList();

                        lastMaxId = searchResultsList[searchResultsList.Count - 1].Id - 1;

                        tweetsToReturn.AddRange(searchResultsList);
                    }
                }
                catch (Exception ex)
                {
                    //TODO: Would normally log any exceptions here
                    throw;
                }
            }

            return tweetsToReturn;
        }

        #endregion Public Methods

        #region Private Methods

        private static Credentials GetTwitterCredentials()
        {
            string consumerKey, consumerSecret, userAccessToken, userAccessSecret;

            consumerKey = ConfigurationManager.AppSettings["TwitterConsumerKey"];
            consumerSecret = ConfigurationManager.AppSettings["TwitterConsumerSecret"];
            userAccessToken = ConfigurationManager.AppSettings["TwitterUserAccessToken"];
            userAccessSecret = ConfigurationManager.AppSettings["TwitterUserAccessSecret"];

            return new Credentials { ConsumerKey = consumerKey, ConsumerSecret = consumerSecret, UserAccessToken = userAccessToken, UserAccessSecret = userAccessSecret };
        }

        /// <summary>
        /// Uses set TwitterCredentials to authenticate to Twitter. Sets the Tweetinvi library to not swallow exceptions.
        /// </summary>
        private static void SetupConnectionToTwitter()
        {
            Tweetinvi.ExceptionHandler.SwallowWebExceptions = false;

            try
            {
                Tweetinvi.Auth.SetUserCredentials(TwitterCredentials.ConsumerKey, TwitterCredentials.ConsumerSecret, TwitterCredentials.UserAccessToken, TwitterCredentials.UserAccessSecret);
            }
            catch (ArgumentException ex)
            {
                //TODO: Would normally log error before bubbling it up
                throw new Exception("Credentials were not accepted by Twitter library. Check configured credentials.", ex);
            }
            catch (Tweetinvi.Exceptions.TwitterException ex)
            {
                //TODO: Would normally log error before bubbling it up
                throw new Exception("Twitter API Request has failed due to bad request, network failure or unauthorized request.", ex);
            }
            catch (Exception ex)
            {
                //TODO: Would normally log error before bubbling it up
                throw;
            }
        }

        #endregion Private Methods
    }
}
