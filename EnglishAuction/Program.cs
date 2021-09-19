using ActressMas;
using System;
using System.Collections.Generic;
using System.Timers;

namespace VickreyAuction
{
    public class Program
    {
        private static void Main(string[] args)
        {
            int NoWiningsHonest = 0;
            int NoWiningsCollutions = 0;
            KeyValuePair<int, int> noWinings = new KeyValuePair<int, int>(0, 0);

            for(int i = 0; i < Utils.NoAuctions; i++)
            {
                int realPrice = Utils.RandNoGen.Next(Utils.AuctionRealPriceMin, Utils.AuctionRealPriceMax);
                int noBidders = Utils.RandNoGen.Next(Utils.NoBiddersMin, Utils.NoBiddersMax);

                Console.WriteLine("\n\n---------------Auction no {0}---------------", (i + 1));
                Console.WriteLine("[{0}]: Real price of product {1}", (i + 1), realPrice);
                Auction auction = new Auction(realPrice, noBidders);
                noWinings = auction.startAuction();

                NoWiningsHonest += noWinings.Key;
                NoWiningsCollutions += noWinings.Value;              
            }

            Console.WriteLine("-----------------Results------------------");
            Console.WriteLine("Probability to win the honest auctioners: {0}", (double)NoWiningsHonest/Utils.NoAuctions);
            Console.WriteLine("Probability to win the malitious auctioners: {0}", (double)NoWiningsCollutions / Utils.NoAuctions);
            Console.ReadLine();
        }

    }
}