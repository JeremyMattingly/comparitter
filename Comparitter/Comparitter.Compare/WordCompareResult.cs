using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comparitter.Compare
{
    public class WordCompareResult
    {
        public WordSearchResult MostPopularWordSearchResult { get; set; }
        public WordSearchResult LeastPopularWordSearchResult { get; set; }

        public bool WordsAreEquallyPopular { get; set; }

        public List<WordSearchResult> EquallyPopularResults { get; set; }

        public double SearchElapsedSeconds { get; set; }
    }
}
