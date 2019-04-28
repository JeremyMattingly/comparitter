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

        }

        protected void CopmareBtn_Click(object sender, EventArgs e)
        {
            string word1 = Word1Tb.Text;
            string word2 = Word2Tb.Text;

            var result = Comparitter.Compare.Compare.CompareByAppearanceCount(word1, word2);

            ResultsLbl.Text = result.SearchElapsedSeconds.ToString();
        }
    }
}