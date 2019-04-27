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
                        Console.WriteLine("Enter search phrase");
                        string searchPhrase = Console.ReadLine();
                        SearchForPhrase(searchPhrase);
                        break;
                    case 3:
                        Console.WriteLine("Enter phrase 1:");
                        string phrase1 = Console.ReadLine();

                        Console.WriteLine();
                        Console.WriteLine("Enter phrase 2:");
                        string phrase2 = Console.ReadLine();
                        Console.WriteLine();

                        ComparePhrasePopularity(phrase1, phrase2);
                        break;
                }

                Console.WriteLine();
                Console.WriteLine("Press any Enter to continue...");
                Console.ReadLine();
                RunMenu();
            }

            return;
        }

        private static void ComparePhrasePopularity(string phrase1, string phrase2)
        {
            var compareResults = Comparitter.Compare.Compare.CompareByAppearanceCount(phrase1, phrase2);

            Console.WriteLine(compareResults);
        }

        private static void RunMenu()
        {
            Console.Clear();

            Console.WriteLine("What do you want to do?");
            Console.WriteLine("0. Exit.");
            Console.WriteLine("1. Return last 5 tweets of the user whose credentials are stored in configuration.");
            Console.WriteLine("2. Search for a phrase.");
            Console.WriteLine("3. Compare popularity for 2 phrases.");

            Console.WriteLine();
            Console.WriteLine("Enter menu item number:");
            menuItem = int.Parse(Console.ReadLine());
        }

        private static void SearchForPhrase(string searchPhrase)
        {
            var tweets = Comparitter.TwitterAgent.Search.GetTopThreeTweetTextByPhrase(searchPhrase);

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
