using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Comparitter.Web
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadHistory();
        }

        private void LoadHistory()
        {
            CompareHistoryGv.DataSource = Comparitter.Compare.CompareHistory.RetrieveWordCompareHistory();
            CompareHistoryGv.DataBind();
        }

        protected void CompareBtn_Click(object sender, EventArgs e)
        {
            string word1 = Word1Tb.Text;
            string word2 = Word2Tb.Text;

            if (ValidateInput(word1, word2))
            {
                DoComparison(word1, word2);
            }
            else
            {
                ValidationLbl.Text = "Input is not valid. Words must not be empty, not contain a space and be 500 characters or less.";
                ValidationLbl.Visible = true;
            }
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

        private void DoComparison(string word1, string word2)
        {
            var result = Comparitter.Compare.Compare.CompareByAppearanceCount(word1, word2);

            PopularityLbl.Text = result.WordsAreEquallyPopular ? "Both are equally popular." : result.MostPopularWordSearchResult.Word + " is more popular.";
            SearchElapsedTimeLbl.Text = "Search took " + result.SearchElapsedSeconds.ToString() + " seconds";

            if (result.WordsAreEquallyPopular)
            {
                Word1Lbl.Text = result.EquallyPopularResults[0].Word;
                Word1AppearancesLbl.Text = result.EquallyPopularResults[0].AppearanceCount.ToString();
                Word1OldestTweetDateLbl.Text = result.EquallyPopularResults[0].OldestTweetDateTime?.ToString();
                Word1NewestTweetDateLbl.Text = result.EquallyPopularResults[0].NewestTweetDateTime?.ToString();

                Word2Lbl.Text = result.EquallyPopularResults[1].Word;
                Word2AppearancesLbl.Text = result.EquallyPopularResults[1].AppearanceCount.ToString();
                Word2OldestTweetDateLbl.Text = result.EquallyPopularResults[1].OldestTweetDateTime?.ToString();
                Word2NewestTweetDateLbl.Text = result.EquallyPopularResults[1].NewestTweetDateTime?.ToString();
            }
            else
            {
                Word1Lbl.Text = result.MostPopularWordSearchResult.Word;
                Word1AppearancesLbl.Text = result.MostPopularWordSearchResult.AppearanceCount.ToString();
                Word1OldestTweetDateLbl.Text = result.MostPopularWordSearchResult.OldestTweetDateTime?.ToString();
                Word1NewestTweetDateLbl.Text = result.MostPopularWordSearchResult.NewestTweetDateTime?.ToString();

                Word2Lbl.Text = result.LeastPopularWordSearchResult.Word;
                Word2AppearancesLbl.Text = result.LeastPopularWordSearchResult.AppearanceCount.ToString();
                Word2OldestTweetDateLbl.Text = result.LeastPopularWordSearchResult.OldestTweetDateTime?.ToString();
                Word2NewestTweetDateLbl.Text = result.LeastPopularWordSearchResult.NewestTweetDateTime?.ToString();
            }

            CompareSection.Visible = true;
            Word1Tb.Text = "";
            Word2Tb.Text = "";

            LoadHistory();
        }
    }
}