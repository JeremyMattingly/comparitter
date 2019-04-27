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

            while (menuItem != 0)
            {
                Console.Clear();

                switch (menuItem)
                {
                    case 1:
                        GetLast5TweetsOfConfiguredUser();
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("Enter search phrase");
                        string searchPhrase = Console.ReadLine();
                        SearchForPhrase(searchPhrase);
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

        private static void SearchForPhrase(string searchPhrase)
        {
            var tweets = Comparitter.TwitterAgent.Search.SearchByPhrase(searchPhrase);

            foreach (var tweet in tweets)
            {
                Console.WriteLine(tweet);
            }
        }

        private static void GetLast5TweetsOfConfiguredUser()
        {
            var tweets = Comparitter.TwitterAgent.Search.FirstContact();

            foreach (var tweet in tweets)
            {
                Console.WriteLine(tweet);
            }
        }

    }
}
