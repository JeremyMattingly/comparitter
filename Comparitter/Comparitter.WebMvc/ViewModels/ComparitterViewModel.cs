using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Comparitter.WebMvc.ViewModels
{
    public class ComparitterViewModel
    {
        public ComparitterFormViewModel CompareForm { get; set; }
        public List<ComparitterHistoryViewModel> CompareHistory { get; set; }
    }

    public class ComparitterFormViewModel
    {
        public string Word1 { get; set; }
        public string Word2 { get; set; }
        public string ValidationText { get; set; }
        public bool DisplayValidationText { get; set; }
        public bool DisplayResults { get; set; }
        public string PopularityText { get; set; }
        public string SearchElapsedTime { get; set; }
        public string ResultWord1 { get; set; }
        public string ResultWord1Appearances { get; set; }
        public string ResultWord1DateOfOldestTweet { get; set; }
        public string ResultWord1DateOfNewestTweet { get; set; }
        public string ResultWord2 { get; set; }
        public string ResultWord2Appearances { get; set; }
        public string ResultWord2DateOfOldestTweet { get; set; }
        public string ResultWord2DateOfNewestTweet { get; set; }
    }

    public class ComparitterHistoryViewModel
    {
        public DateTime CompareDateTime { get; set; }
        public double ElapsedSeconds { get; set; }
        public bool WordsAreEquallyPopular { get; set; }
        public string Word1 { get; set; }
        public int Word1Appearances { get; set; }
        public string Word2 { get; set; }
        public int Word2Appearances { get; set; }

    }
}