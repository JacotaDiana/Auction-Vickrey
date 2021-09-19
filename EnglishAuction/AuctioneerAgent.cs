using ActressMas;
using System;
using System.Collections.Generic;
using System.Timers;

namespace VickreyAuction
{
    public class AuctioneerAgent : TurnBasedAgent
    {
        private struct Bid
        {
            public string Bidder { get; set; }
            public int BidValue { get; set; }

            public Bid(string bidder, int bidValue)
            {
                Bidder = bidder;
                BidValue = bidValue;
            }
        }

        private List<Bid> _bids;
        private int _realPrice;

        public AuctioneerAgent(int realPrice)
        {
            _bids = new List<Bid>();
            _realPrice = realPrice;
        }

        public override void Setup()
        {
            Broadcast("start");
        }

        /*
        public void ActDefault()
        {
            if (--_turnsToWait <= 0)
                HandleFinish();
        }*/


        public override void Act(Queue<Message> messages)
        {       
            if(messages.Count == 0)
            {
                HandleFinish();
                
            }   
            while (messages.Count > 0)
            {
                Message message = messages.Dequeue();

                Console.WriteLine("\t[{1} -> {0}]: {2}", this.Name, message.Sender, message.Content);

                string action; string parameters;
                Utils.ParseMessage(message.Content, out action, out parameters);

                switch (action)
                {
                    case "bid":
                        HandleBid(message.Sender, Convert.ToInt32(parameters));
                        break;

                    default:
                        break;
                }
            }
        }

        private void HandleBid(string sender, int price)
        {
            _bids.Add(new Bid(sender, price));
        }

        private void HandleFinish()
        {
            string highestBidder = "";
            int highestBid = int.MinValue;
            int[] bidValues = new int[_bids.Count];

            for (int i = 0; i < _bids.Count; i++)
            {
                int b = _bids[i].BidValue;
                if (b > highestBid && b >= _realPrice)
                {
                    highestBid = b;
                    highestBidder = _bids[i].Bidder;
                }
                bidValues[i] = b;
            }

            if (highestBidder == "") // no bids above reserve price
            {
                Console.WriteLine("[auctioneer]: Auction finished. No winner.");
                Broadcast("winner none");
            }
            else
            {
                Array.Sort(bidValues);
                Array.Reverse(bidValues);
                int winningPrice = bidValues[1]; // second price

                Console.WriteLine($"[auctioneer]: Auction finished. Sold to {highestBidder} for price {winningPrice}.");
                Broadcast($"winner {highestBidder}");
                
            }

            Stop();
        }
    }
}