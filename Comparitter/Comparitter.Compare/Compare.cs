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
        public static string CompareByAppearanceCount(string phrase1, string phrase2)
        {
            Stopwatch howLong = new Stopwatch();
            howLong.Start();

            var phrase1Results = Comparitter.TwitterAgent.Search.SearchByPhrase(phrase1).ToList();
            var phrase1ResultsOldestTweetDateTime = phrase1Results[phrase1Results.Count - 1].CreatedAt;
            var phrase1ResultsNewestTweetDateTime = phrase1Results[0].CreatedAt;

            var phrase2Results = Comparitter.TwitterAgent.Search.SearchByPhrase(phrase2).ToList();
            var phrase2ResultsOldestTweetDateTime = phrase2Results[phrase2Results.Count - 1].CreatedAt;
            var phrase2ResultsNewestTweetDateTime = phrase2Results[0].CreatedAt;

            string morePopularPhrase;

            if (phrase1Results.Count == phrase2Results.Count)
            {
                morePopularPhrase = string.Format(phrase1 + " and " + phrase2 + " are equally popular with {0} tweets.", phrase1Results.Count.ToString());
            }
            else if (phrase1Results.Count > phrase2Results.Count)
            {
                morePopularPhrase = string.Format(phrase1 + " is more popular with {0} appearances. " + phrase2 + " had {1} appearances. ", phrase1Results.Count.ToString(), phrase2Results.Count.ToString());
            }
            else
            {
                morePopularPhrase = string.Format(phrase2 + " is more popular with {0} appearances. " + phrase1 + " had {1} appearances. ", phrase2Results.Count.ToString(), phrase1Results.Count.ToString());
            }

            morePopularPhrase = morePopularPhrase + string.Format("{0}Phrase1 OldestDate: {1}. Phrase1 NewestDate: {2}", Environment.NewLine,
                phrase1ResultsOldestTweetDateTime.ToShortDateString() + " " + phrase1ResultsOldestTweetDateTime.ToShortTimeString(),
                phrase1ResultsNewestTweetDateTime.ToShortDateString() + " " + phrase1ResultsNewestTweetDateTime.ToShortTimeString());


            morePopularPhrase = morePopularPhrase + string.Format("{0}Phrase2 OldestDate: {1}. Phrase2 NewestDate: {2}", Environment.NewLine,
                phrase2ResultsOldestTweetDateTime.ToShortDateString() + " " + phrase2ResultsOldestTweetDateTime.ToShortTimeString(),
                phrase2ResultsNewestTweetDateTime.ToShortDateString() + " " + phrase2ResultsNewestTweetDateTime.ToShortTimeString());

            howLong.Stop();

            morePopularPhrase = morePopularPhrase + string.Format("{0}{0}Search took {1} seconds.", Environment.NewLine, howLong.Elapsed.TotalSeconds.ToString());

            return morePopularPhrase;
        }
    }
}
