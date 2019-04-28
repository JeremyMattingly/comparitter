using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comparitter.Cli
{
    class Program
    {
        static int menuItem = -1;


        static void Main(string[] args)
        {
            RunMenu();

            while (menuItem != 0)
            {
                Console.Clear();

                switch (menuItem)
                {
                    case 1:
                        Console.WriteLine("Enter phrase 1:");
                        string phrase1 = Console.ReadLine();

                        Console.WriteLine();
                        Console.WriteLine("Enter phrase 2:");
                        string phrase2 = Console.ReadLine();
                        Console.WriteLine();

                        ComparePhrasePopularity(phrase1, phrase2);
                        break;
                }

                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                RunMenu();
            }

            return;
        }

        private static void ComparePhrasePopularity(string phrase1, string phrase2)
        {
            var compareResults = Comparitter.Compare.Compare.CompareByAppearanceCount(phrase1, phrase2);

            Console.WriteLine(compareResults);
        }

        private static void RunMenu()
        {
            Console.Clear();

            Console.WriteLine("What do you want to do?");
            Console.WriteLine("0. Exit.");
            Console.WriteLine("1. Compare popularity for 2 phrases.");

            Console.WriteLine();
            Console.WriteLine("Enter menu item number:");
            menuItem = int.Parse(Console.ReadLine());
        }

    }
}
