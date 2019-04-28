using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Comparitter.Compare
{
    public class Compare
    {
        /// <summary>
        /// Takes two words and determines which is more popular on Twitter by how many tweets contain the word.
        /// </summary>
        /// <param name="searchWord1">The first word to search for.</param>
        /// <param name="searchWord2">The second word to search for.</param>
        /// <returns><see cref="WordCompareResult"/> of the comparison results.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="searchWord1"/> or <paramref name="searchWord2"/> is null, is only whitespace or contains a space.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="searchWord1"/> or <paramref name="searchWord2"/> length is over 500 characters.</exception>
        /// <exception cref="Comparitter.Compare.Exception.CompareException">Thrown when an error occurrs while searching for one of the words.</exception>
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

            WordCompareResult compareResultsToReturn = new WordCompareResult
            {
                CompareDateTime = DateTime.Now
            };

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

            CompareHistory.SaveWordCompareResult(compareResultsToReturn);

            return compareResultsToReturn;
        }

        /// <summary>
        /// Searches Twitter for tweets containg the specified search word and instantiates a new <see cref="WordSearchResult"/> with the results.
        /// </summary>
        /// <param name="searchWord">The word to search Twitter for.</param>
        /// <returns><see cref="WordSearchResult"/> of the search results.</returns>
        private static WordSearchResult GetWordSearchResults(string searchWord)
        {
            List<Tweetinvi.Models.ITweet> tweetsContainingWord = TwitterAgent.Search.SearchByWord(searchWord).ToList();

            WordSearchResult wordResult = new WordSearchResult
            {
                Word = searchWord,
                AppearanceCount = tweetsContainingWord.Count,
                SearchFailed = false,
                SearchDateTime = DateTime.Now
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
