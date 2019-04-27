using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comparitter.Compare
{
    public class Compare
    {
        public static string CompareByAppearanceCount(string phrase1, string phrase2)
        {
            var phrase1Results = Comparitter.TwitterAgent.Search.SearchByPhrase(phrase1);
            var phrase1ResultsCount = phrase1Results.Count();

            var phrase2Results = Comparitter.TwitterAgent.Search.SearchByPhrase(phrase2);
            var phrase2ResultsCount = phrase2Results.Count();

            string morePopularPhrase;

            if (phrase1ResultsCount == phrase2ResultsCount)
            {
                morePopularPhrase = string.Format(phrase1 + " and " + phrase2 + " are equally popular with {0} tweets.", phrase1ResultsCount.ToString());
            }
            else if (phrase1ResultsCount > phrase2ResultsCount)
            {
                morePopularPhrase = string.Format(phrase1 + " is more popular with {0} appearances." + phrase2 + " had {1} appearances.", phrase1ResultsCount.ToString(), phrase2ResultsCount.ToString());
            }
            else
            {
                morePopularPhrase = string.Format(phrase2 + " is more popular with {0} appearances." + phrase1 + " had {1} appearances.", phrase2ResultsCount.ToString(), phrase1ResultsCount.ToString());
            }

            return morePopularPhrase;
        }
    }
}
