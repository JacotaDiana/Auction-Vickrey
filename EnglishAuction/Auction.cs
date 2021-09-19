using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VickreyAuction
{
    public class Auction
    {
        public int RealPrice = 0;
        public int MinPrice = 0;
        public int MaxPrice = 0;
        public int NoBidders = 0;
        public int NoPairsOfCollutions = 0;
        public KeyValuePair<int, int> NoWinings = new KeyValuePair<int, int>(0, 0);
        public Dictionary<int, int> collutions= new Dictionary<int, int>();
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
            this.NoPairsOfCollutions = Utils.RandNoGen.Next(noBidders / 2);
        }

        public KeyValuePair<int, int> startAuction()
        {
            var env = new ActressMas.TurnBasedEnvironment(0, 200);           

            for(int i = 0; i <= NoPairsOfCollutions; i++)
            {
                int agentID1 = Utils.RandNoGen.Next(1, NoBidders);
                while (collutions.ContainsKey(agentID1))
                {
                    agentID1 = Utils.RandNoGen.Next(1, NoBidders);                
                }

                int agentID2 = Utils.RandNoGen.Next(1, NoBidders);
                while (collutions.ContainsKey(agentID2) || agentID1 == agentID2)
                {
                    agentID2 = Utils.RandNoGen.Next(1, NoBidders);            
                }

                int agentValuation1 = MinPrice + Utils.RandNoGen.Next(MaxPrice - MinPrice) * 5;
                var bidderAgent = new BidderAgent(agentValuation1, true);
                env.Add(bidderAgent, string.Format("bidder{0:D2}", agentID1));

                int agentValuation2 = agentValuation1 * 25 / 100;
                var bidderAgent2 = new BidderAgent(agentValuation2, true);
                env.Add(bidderAgent2, string.Format("bidder{0:D2}", agentID2));

                collutions.Add(agentID1, agentID2);
                collutions.Add(agentID2, agentID1);
            }

            for (int i = 1; i <= NoBidders; i++)
            {
               if(!collutions.ContainsValue(i))
                {
                    int agentValuation = MinPrice + Utils.RandNoGen.Next(MaxPrice - MinPrice);
                    var bidderAgent = new BidderAgent(agentValuation, false);
                    env.Add(bidderAgent, string.Format("bidder{0:D2}", i));
                }       
            }

            var auctioneerAgent = new AuctioneerAgent(RealPrice, this);
            env.Add(auctioneerAgent, "auctioneer");

            env.Start();

            return NoWinings;           
        }
    }
}
