using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VickreyAuction
{
    class Auction
    {
        public int RealPrice = 0;
        public int MinPrice = 0;
        public int MaxPrice = 0;
        public int NoBidders = 0;
        //public static int Increment = 10;

        public int getRealPrice()
        {
            return RealPrice;
        }

        public Auction(int realPrice, int noBidders)
        {
            this.RealPrice = realPrice;           
            this.NoBidders = noBidders;
            this.MinPrice = realPrice - (int)(50 / 100 * realPrice);
            this.MaxPrice = realPrice + (int)(150 / 100 * realPrice);
        }

        public void startAuction()
        {
            var env = new ActressMas.TurnBasedEnvironment(0, 200);           

            for (int i = 1; i <= NoBidders; i++)
            {
                int agentValuation = MinPrice + Utils.RandNoGen.Next(MaxPrice - MinPrice);
                var bidderAgent = new BidderAgent(agentValuation);
                env.Add(bidderAgent, string.Format("bidder{0:D2}", i));
            }

            var auctioneerAgent = new AuctioneerAgent(RealPrice);
            env.Add(auctioneerAgent, "auctioneer");

            env.Start();           
        }
    }
}
