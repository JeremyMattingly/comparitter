using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Comparitter.WebMvc.ViewModels;

namespace Comparitter.WebMvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var viewModel = new ViewModels.ComparitterViewModel
            {
                CompareForm = new ComparitterFormViewModel(),
                CompareHistory = GetHistoryCompareViewModel()
            };
            return View(viewModel);
        }

        [HttpPost]
        [Route("Home/Compare")]
        public ActionResult Compare(ComparitterViewModel viewModel)
        {
            string word1 = viewModel.CompareForm.Word1;
            string word2 = viewModel.CompareForm.Word2;

            ViewModels.ComparitterViewModel returnViewModel;

            if (ValidateInput(word1, word2))
            {
                returnViewModel = DoComparison(word1, word2);
            }
            else
            {
                returnViewModel = viewModel;
                returnViewModel.CompareForm.ValidationText = "Input is not valid. Words must not be empty, not contain a space and be 500 characters or less.";
                returnViewModel.CompareForm.DisplayValidationText = true;
            }

            returnViewModel.CompareHistory = GetHistoryCompareViewModel();

            return View("Index", returnViewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        private List<ComparitterHistoryViewModel> GetHistoryCompareViewModel()
        {
            List<ComparitterHistoryViewModel> historyViewModels = new List<ComparitterHistoryViewModel>();
            var compareHistory = Comparitter.Compare.CompareHistory.RetrieveWordCompareHistory();

            foreach (var comparison in compareHistory)
            {
                ComparitterHistoryViewModel comparitterHistoryViewModel = new ComparitterHistoryViewModel()
                {
                    CompareDateTime = comparison.CompareDateTime,
                    ElapsedSeconds = comparison.SearchElapsedSeconds,
                    WordsAreEquallyPopular = comparison.WordsAreEquallyPopular,
                    Word1 = (comparison.WordsAreEquallyPopular) ? comparison.EquallyPopularResults[0].Word : comparison.MostPopularWordSearchResult.Word,
                    Word1Appearances = (comparison.WordsAreEquallyPopular) ? comparison.EquallyPopularResults[0].AppearanceCount : comparison.MostPopularWordSearchResult.AppearanceCount,
                    Word2 = (comparison.WordsAreEquallyPopular) ? comparison.EquallyPopularResults[1].Word : comparison.LeastPopularWordSearchResult.Word,
                    Word2Appearances = (comparison.WordsAreEquallyPopular) ? comparison.EquallyPopularResults[1].AppearanceCount : comparison.LeastPopularWordSearchResult.AppearanceCount,
                };

                historyViewModels.Add(comparitterHistoryViewModel);
            }

            return historyViewModels;
        }

        private bool ValidateInput(string word1, string word2)
        {
            bool inputIsValid = !string.IsNullOrWhiteSpace(word1) && !string.IsNullOrWhiteSpace(word2);

            if (inputIsValid)
            {
                inputIsValid = word1.Length <= 500 && word2.Length <= 500;
            }

            if (inputIsValid)
            {
                inputIsValid = !word1.Contains(" ") && !word2.Contains(" ");
            }

            return inputIsValid;
        }

        private ViewModels.ComparitterViewModel DoComparison(string word1, string word2)
        {
            ViewModels.ComparitterViewModel returnViewModel = new ComparitterViewModel()
            {
                CompareForm = new ComparitterFormViewModel(),
                CompareHistory = new List<ComparitterHistoryViewModel>()
            };
            var result = Comparitter.Compare.Compare.CompareByAppearanceCount(word1, word2);

            returnViewModel.CompareForm.PopularityText = result.WordsAreEquallyPopular ? "Both are equally popular." : result.MostPopularWordSearchResult.Word + " is more popular.";
            returnViewModel.CompareForm.SearchElapsedTime = "Search took " + result.SearchElapsedSeconds.ToString() + " seconds";

            if (result.WordsAreEquallyPopular)
            {
                returnViewModel.CompareForm.ResultWord1 = result.EquallyPopularResults[0].Word;
                returnViewModel.CompareForm.ResultWord1Appearances = result.EquallyPopularResults[0].AppearanceCount.ToString();
                returnViewModel.CompareForm.ResultWord1DateOfOldestTweet = result.EquallyPopularResults[0].OldestTweetDateTime?.ToString();
                returnViewModel.CompareForm.ResultWord1DateOfNewestTweet = result.EquallyPopularResults[0].NewestTweetDateTime?.ToString();

                returnViewModel.CompareForm.ResultWord2 = result.EquallyPopularResults[1].Word;
                returnViewModel.CompareForm.ResultWord2Appearances = result.EquallyPopularResults[1].AppearanceCount.ToString();
                returnViewModel.CompareForm.ResultWord2DateOfOldestTweet = result.EquallyPopularResults[1].OldestTweetDateTime?.ToString();
                returnViewModel.CompareForm.ResultWord2DateOfNewestTweet = result.EquallyPopularResults[1].NewestTweetDateTime?.ToString();
            }
            else
            {
                returnViewModel.CompareForm.ResultWord1 = result.MostPopularWordSearchResult.Word;
                returnViewModel.CompareForm.ResultWord1Appearances = result.MostPopularWordSearchResult.AppearanceCount.ToString();
                returnViewModel.CompareForm.ResultWord1DateOfOldestTweet = result.MostPopularWordSearchResult.OldestTweetDateTime?.ToString();
                returnViewModel.CompareForm.ResultWord1DateOfNewestTweet = result.MostPopularWordSearchResult.NewestTweetDateTime?.ToString();

                returnViewModel.CompareForm.ResultWord2 = result.LeastPopularWordSearchResult.Word;
                returnViewModel.CompareForm.ResultWord2Appearances = result.LeastPopularWordSearchResult.AppearanceCount.ToString();
                returnViewModel.CompareForm.ResultWord2DateOfOldestTweet = result.LeastPopularWordSearchResult.OldestTweetDateTime?.ToString();
                returnViewModel.CompareForm.ResultWord2DateOfNewestTweet = result.LeastPopularWordSearchResult.NewestTweetDateTime?.ToString();
            }

            returnViewModel.CompareForm.DisplayResults = true;

            return returnViewModel;
        }
    }
}