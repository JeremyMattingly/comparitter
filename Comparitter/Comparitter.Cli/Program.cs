using System;
using System.Collections.Generic;
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

            Console.WriteLine("Enter Twitter Consumer Key:");
            consumerKey = Console.ReadLine();

            Console.WriteLine("Enter Twitter Consumer Secret:");
            consumerSecret = Console.ReadLine();

            Console.WriteLine("Enter Twitter User Access Token:");
            userAccessToken = Console.ReadLine();

            Console.WriteLine("Enter Twitter User Access Secret:");
            userAccessSecret = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("Retrieving Tweets...");
            Console.WriteLine();

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
