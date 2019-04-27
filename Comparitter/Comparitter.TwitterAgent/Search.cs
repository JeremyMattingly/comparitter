using System;
using System.Collections.Generic;
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
        public static List<string> FirstContact(Credentials credentials)
        {
            Auth.SetUserCredentials(credentials.ConsumerKey, credentials.ConsumerSecret, credentials.UserAccessToken, credentials.UserAccessSecret);

            var user = User.GetAuthenticatedUser();
            var timelineTweets = Timeline.GetUserTimeline(user, 5);

            List<string> tweetsToReturn = new List<string>();

            foreach (var tweet in timelineTweets)
            {
                tweetsToReturn.Add(tweet.Text);
            }

            return tweetsToReturn;
        }
    }
}
