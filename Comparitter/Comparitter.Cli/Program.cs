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

                    case 2:
                        RetrieveComparisonHistory();
                        break;
                }

                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
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
            Console.WriteLine("1. Compare popularity for 2 phrases.");
            Console.WriteLine("2. Retrieve comparison history.");

            Console.WriteLine();
            Console.WriteLine("Enter menu item number:");
            menuItem = int.Parse(Console.ReadLine());
        }

        private static void ComparePhrasePopularity(string phrase1, string phrase2)
        {
            string morePopularWordResultText;

            try
            {
                Compare.WordCompareResult compareResults = Comparitter.Compare.Compare.CompareByAppearanceCount(phrase1, phrase2);

                if (compareResults.WordsAreEquallyPopular)
                {
                    morePopularWordResultText = string.Format(compareResults.EquallyPopularResults[0].Word + " and " + compareResults.EquallyPopularResults[1].Word + " are equally popular with {0} tweets.",
                        compareResults.EquallyPopularResults[0].AppearanceCount.ToString());
                }
                else
                {
                    morePopularWordResultText = string.Format(compareResults.MostPopularWordSearchResult.Word + " is more popular with {0} appearances. " + compareResults.LeastPopularWordSearchResult.Word + " had {1} appearances. ",
                        compareResults.MostPopularWordSearchResult.AppearanceCount.ToString(),
                        compareResults.LeastPopularWordSearchResult.AppearanceCount.ToString());

                    morePopularWordResultText += string.Format("{0}{1} Oldest Date: {2}",
                        Environment.NewLine,
                        compareResults.MostPopularWordSearchResult.Word,
                        compareResults.MostPopularWordSearchResult.OldestTweetDateTime?.ToShortDateString() + " " + compareResults.MostPopularWordSearchResult.OldestTweetDateTime?.ToShortTimeString());


                    morePopularWordResultText += string.Format("{0}{1} Newest Date: {2}",
                        Environment.NewLine,
                        compareResults.MostPopularWordSearchResult.Word,
                        compareResults.MostPopularWordSearchResult.NewestTweetDateTime?.ToShortDateString() + " " + compareResults.MostPopularWordSearchResult.NewestTweetDateTime?.ToShortTimeString());

                    morePopularWordResultText += string.Format("{0}{1} Oldest Date: {2}",
                    Environment.NewLine,
                    compareResults.LeastPopularWordSearchResult.Word,
                    compareResults.LeastPopularWordSearchResult.OldestTweetDateTime?.ToShortDateString() + " " + compareResults.LeastPopularWordSearchResult.OldestTweetDateTime?.ToShortTimeString());

                    morePopularWordResultText += string.Format("{0}{1} Newest Date: {2}",
                    Environment.NewLine,
                    compareResults.LeastPopularWordSearchResult.Word,
                    compareResults.LeastPopularWordSearchResult.NewestTweetDateTime?.ToShortDateString() + " " + compareResults.LeastPopularWordSearchResult.NewestTweetDateTime?.ToShortTimeString());
                }

                morePopularWordResultText += string.Format("{0}{0}Search took {1} seconds.", Environment.NewLine, compareResults.SearchElapsedSeconds.ToString());
            }
            catch (ArgumentException ex)
            {
                morePopularWordResultText = "Your input isn't valid. One word, no spaces, up to 500 characters.";
            }
            catch (Comparitter.Compare.Exception.CompareException ex)
            {
                morePopularWordResultText = "Yikes! Twitter didn't like what we sent them, or they disappeared for a moment. Make sure your search words are not too common. I am cheap and didn't pay for access to return gobs and gobs of results.";
            }
            catch (Exception ex)
            {
                //TODO: Log This
                morePopularWordResultText = "Shoot! Something didn't work right. Please try again later.";
            }

            Console.WriteLine(morePopularWordResultText);
        }

        private static void RetrieveComparisonHistory()
        {
            Console.Clear();

            var history = Comparitter.Compare.CompareHistory.RetrieveWordCompareHistory();

            foreach (var item in history)
            {
                string text = item.CompareDateTime.ToString() + " WordsAreEquallyPopular: " + item.WordsAreEquallyPopular.ToString();

                if (item.WordsAreEquallyPopular)
                {
                    text += " Word1: " + item.EquallyPopularResults[0].Word;
                    text += " Word2: " + item.EquallyPopularResults[1].Word;
                }
                else
                {
                    text += " MostPopularWord: " + item.MostPopularWordSearchResult.Word;
                    text += " LeastPopularWord: " + item.LeastPopularWordSearchResult.Word;
                }

                Console.WriteLine(text);
            }
        }

    }
}
