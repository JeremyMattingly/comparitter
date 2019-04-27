using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comparitter.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var tweets = Comparitter.TwitterAgent.Search.FirstContact();

            foreach (var tweet in tweets)
            {
                Console.WriteLine(tweet);
            }

            Console
        }
    }
}
