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
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                RunMenu();
            }

            return;
        }

        private static void ComparePhrasePopularity(string phrase1, string phrase2)
        {
            var compareResults = Comparitter.Compare.Compare.CompareByAppearanceCount(phrase1, phrase2);

            string morePopularWordResultText;


            //if (tweetsContainingWord1.Count == phrase2Results.Count)
            //{
            //    morePopularWordResultText = string.Format(searchWord1 + " and " + searchWord2 + " are equally popular with {0} tweets.", tweetsContainingWord1.Count.ToString());
            //}
            //else if (tweetsContainingWord1.Count > phrase2Results.Count)
            //{
            //    morePopularWordResultText = string.Format(searchWord1 + " is more popular with {0} appearances. " + searchWord2 + " had {1} appearances. ", tweetsContainingWord1.Count.ToString(), phrase2Results.Count.ToString());
            //}
            //else
            //{
            //    morePopularWordResultText = string.Format(searchWord2 + " is more popular with {0} appearances. " + searchWord1 + " had {1} appearances. ", phrase2Results.Count.ToString(), tweetsContainingWord1.Count.ToString());
            //}

            //morePopularWordResultText = morePopularWordResultText + string.Format("{0}Phrase1 OldestDate: {1}. Phrase1 NewestDate: {2}", Environment.NewLine,
            //    phrase1ResultsOldestTweetDateTime.ToShortDateString() + " " + phrase1ResultsOldestTweetDateTime.ToShortTimeString(),
            //    phrase1ResultsNewestTweetDateTime.ToShortDateString() + " " + phrase1ResultsNewestTweetDateTime.ToShortTimeString());


            //morePopularWordResultText = morePopularWordResultText + string.Format("{0}Phrase2 OldestDate: {1}. Phrase2 NewestDate: {2}", Environment.NewLine,
            //    phrase2ResultsOldestTweetDateTime.ToShortDateString() + " " + phrase2ResultsOldestTweetDateTime.ToShortTimeString(),
            //    phrase2ResultsNewestTweetDateTime.ToShortDateString() + " " + phrase2ResultsNewestTweetDateTime.ToShortTimeString());


            //morePopularWordResultText = morePopularWordResultText + string.Format("{0}{0}Search took {1} seconds.", Environment.NewLine, howLong.Elapsed.TotalSeconds.ToString());

            Console.WriteLine(compareResults);
        }

        private static void RunMenu()
        {
            Console.Clear();

            Console.WriteLine("What do you want to do?");
            Console.WriteLine("0. Exit.");
            Console.WriteLine("1. Compare popularity for 2 phrases.");

            Console.WriteLine();
            Console.WriteLine("Enter menu item number:");
            menuItem = int.Parse(Console.ReadLine());
        }

    }
}
