using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comparitter.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            string consumerKey, consumerSecret, userAccessToken, userAccessSecret;

            consumerKey = ConfigurationManager.AppSettings["TwitterConsumerKey"];
            consumerSecret = ConfigurationManager.AppSettings["TwitterConsumerSecret"];
            userAccessToken = ConfigurationManager.AppSettings["TwitterUserAccessToken"];
            userAccessSecret = ConfigurationManager.AppSettings["TwitterUserAccessSecret"];

            var credentials = new TwitterAgent.Credentials { ConsumerKey = consumerKey, ConsumerSecret = consumerSecret, UserAccessToken = userAccessToken, UserAccessSecret = userAccessSecret };

            var tweets = Comparitter.TwitterAgent.Search.FirstContact(credentials);

            foreach (var tweet in tweets)
            {
                Console.WriteLine(tweet);
            }

            Console.ReadKey();
        }
    }
}
