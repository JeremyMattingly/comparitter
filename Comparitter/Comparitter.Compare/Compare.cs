using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Comparitter.Compare
{
    public struct WordSearchResult
    {
        public string Word { get; set; }
        public DateTime? OldestTweetDateTime { get; set; }
        public DateTime? NewestTweetDateTime { get; set; }
        public int AppearanceCount { get; set; }
    }

    public struct WordCompareResult
    {
        public WordSearchResult MostPopularWordSearchResult { get; set; }
        public WordSearchResult LeastPopularWordSearchResult { get; set; }

        public bool WordsAreEquallyPopular { get; set; }

        public List<WordSearchResult> EquallyPopularResults { get; set; }

        public double SearchElapsedSeconds { get; set; }
    }

    public class Compare
    {
        public static WordCompareResult CompareByAppearanceCount(string searchWord1, string searchWord2)
        {
            if (string.IsNullOrWhiteSpace(searchWord1))
            {
                throw new ArgumentException("Phrase cannot be null or only whitespace.", nameof(searchWord1));
            }

            if (searchWord1.Length > 500)
            {
                throw new ArgumentOutOfRangeException(nameof(searchWord1), "Phrase is limited to 500 characters.");
            }

            if (searchWord1.Contains(" "))
            {
                throw new ArgumentException("SearchByWord() does not support searching for phrases. Only single words are allowed.", nameof(searchWord1));
            }

            if (string.IsNullOrWhiteSpace(searchWord2))
            {
                throw new ArgumentException("Phrase cannot be null or only whitespace.", nameof(searchWord2));
            }

            if (searchWord2.Length > 500)
            {
                throw new ArgumentOutOfRangeException(nameof(searchWord2), "Phrase is limited to 500 characters.");
            }

            if (searchWord2.Contains(" "))
            {
                throw new ArgumentException("SearchByWord() does not support searching for phrases. Only single words are allowed.", nameof(searchWord2));
            }

            WordCompareResult compareResultsToReturn = new WordCompareResult();

            Stopwatch howLong = new Stopwatch();

            try
            {
                howLong.Start();

                WordSearchResult word1SearchResult = GetWordSearchResults(searchWord1);

                WordSearchResult word2SearchResult = GetWordSearchResults(searchWord2);

                howLong.Stop();
                
                if (word1SearchResult.AppearanceCount == word2SearchResult.AppearanceCount)
                {
                    compareResultsToReturn.WordsAreEquallyPopular = true;
                    compareResultsToReturn.EquallyPopularResults = new List<WordSearchResult>() { word1SearchResult, word2SearchResult };
                }
                else if (word1SearchResult.AppearanceCount > word2SearchResult.AppearanceCount)
                {
                    compareResultsToReturn.WordsAreEquallyPopular = false;
                    compareResultsToReturn.MostPopularWordSearchResult = word1SearchResult;
                    compareResultsToReturn.LeastPopularWordSearchResult = word2SearchResult;
                }
                else
                {
                    compareResultsToReturn.WordsAreEquallyPopular = false;
                    compareResultsToReturn.MostPopularWordSearchResult = word2SearchResult;
                    compareResultsToReturn.LeastPopularWordSearchResult = word1SearchResult;
                }

                compareResultsToReturn.SearchElapsedSeconds = howLong.Elapsed.TotalSeconds;
                
            }
            catch (Tweetinvi.Exceptions.TwitterException ex)
            {
                //TODO: Would normally log this

                string message = "Communication with Twitter failed for one of the word searches. This could be caused by a network issue, Twitter rate limit or a bad request.";

                throw new Comparitter.Compare.Exception.CompareException(message, ex);
            }
            catch (System.Exception ex)
            {
                //TODO: Log this

                throw;
            }

            return compareResultsToReturn;
        }

        private static WordSearchResult GetWordSearchResults(string searchWord)
        {
            List<Tweetinvi.Models.ITweet> tweetsContainingWord = TwitterAgent.Search.SearchByWord(searchWord).ToList();

            WordSearchResult wordResult = new WordSearchResult
            {
                Word = searchWord,
                AppearanceCount = tweetsContainingWord.Count
            };

            if (tweetsContainingWord.Count > 0)
            {
                wordResult.OldestTweetDateTime = tweetsContainingWord[tweetsContainingWord.Count - 1].CreatedAt;
                wordResult.NewestTweetDateTime = tweetsContainingWord[0].CreatedAt;
            }
            else
            {
                wordResult.OldestTweetDateTime = null;
                wordResult.NewestTweetDateTime = null;
            }

            return wordResult;
        }
    }
}
