using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comparitter.Compare
{
    public class WordSearchResult
    {
        public string Word { get; set; }
        public DateTime? OldestTweetDateTime { get; set; }
        public DateTime? NewestTweetDateTime { get; set; }
        public int AppearanceCount { get; set; }
    }
}
