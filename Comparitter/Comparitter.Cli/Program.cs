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
        static int menuItem = -1;


        static void Main(string[] args)
        {
            RunMenu();

            TwitterAgent.Credentials credentials = GetTwitterCredentials();

            while (menuItem != 0)
            {
                Console.Clear();

                switch (menuItem)
                {
                    case 1:
                        GetLast5TweetsOfConfiguredUser(credentials);
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("Enter search phrase");
                        string searchPhrase = Console.ReadLine();
                        SearchForPhrase(credentials, searchPhrase);
                        break;
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
                RunMenu();
            }

            return;
        }

        private static void RunMenu()
        {
            Console.Clear();

            Console.WriteLine("What do you want to do?");
            Console.WriteLine("0. Exit.");
            Console.WriteLine("1. Return last 5 tweets of the user whose credentials are stored in configuration.");
            Console.WriteLine("2. Search for a phrase.");

            Console.WriteLine();
            Console.WriteLine("Enter menu item number:");
            menuItem = int.Parse(Console.ReadLine());
        }

        private static void SearchForPhrase(TwitterAgent.Credentials credentials, string searchPhrase)
        {
            var tweets = Comparitter.TwitterAgent.Search.SearchByPhrase(credentials, searchPhrase);

            foreach (var tweet in tweets)
            {
                Console.WriteLine(tweet);
            }
        }

        private static void GetLast5TweetsOfConfiguredUser(TwitterAgent.Credentials credentials)
        {
            var tweets = Comparitter.TwitterAgent.Search.FirstContact(credentials);

            foreach (var tweet in tweets)
            {
                Console.WriteLine(tweet);
            }
        }

        private static TwitterAgent.Credentials GetTwitterCredentials()
        {
            string consumerKey, consumerSecret, userAccessToken, userAccessSecret;

            consumerKey = ConfigurationManager.AppSettings["TwitterConsumerKey"];
            consumerSecret = ConfigurationManager.AppSettings["TwitterConsumerSecret"];
            userAccessToken = ConfigurationManager.AppSettings["TwitterUserAccessToken"];
            userAccessSecret = ConfigurationManager.AppSettings["TwitterUserAccessSecret"];

            var credentials = new TwitterAgent.Credentials { ConsumerKey = consumerKey, ConsumerSecret = consumerSecret, UserAccessToken = userAccessToken, UserAccessSecret = userAccessSecret };
            return credentials;
        }
    }
}
