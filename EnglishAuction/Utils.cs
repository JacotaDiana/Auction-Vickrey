using System;

namespace VickreyAuction
{
    public class Utils
    {
        public static int NoAuctions = 1000;
        public static int AuctionRealPriceMin = 50; //this is the minimum value of the auctioned product
        public static int AuctionRealPriceMax = 10000; //this is the maximum value of the auctioned product
        public static int NoBiddersMin = 2;
        public static int NoBiddersMax = 100;

        public static Random RandNoGen = new Random();

        public static void ParseMessage(string content, out string action, out string parameters)
        {
            string[] t = content.Split();

            action = t[0];

            parameters = "";

            if (t.Length > 1)
            {
                for (int i = 1; i < t.Length - 1; i++)
                    parameters += t[i] + " ";
                parameters += t[t.Length - 1];
            }
        }
    }
}