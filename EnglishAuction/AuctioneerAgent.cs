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
        private Auction _auction;

        public void removeBidder(int name)
        {

        }

        private Bid getBidByBidder(String bidder)
        { 
            for(int i = 0; i < _bids.Count; i++)
            {
                if (_bids[i].Bidder.Equals(bidder))
                {
                    return _bids[i];
                }
            }
            throw new Exception("Couldn't find bidder");
        }

        public AuctioneerAgent(int realPrice, Auction auction)
        {
            _bids = new List<Bid>();
            _realPrice = realPrice;
            _auction = auction;
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

                //Console.WriteLine("\t[{1} -> {0}]: {2}", this.Name, message.Sender, message.Content);

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
            SearchForCollutions();
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

            if (highestBidder == "" || bidValues.Length < 2) // no bids above reserve price
            {
                Console.WriteLine("[auctioneer]: Auction finished. No winner.");
                Broadcast("winner none");
            }
            else
            {
                int index = Int32.Parse(highestBidder.Substring(highestBidder.IndexOf('0') + 1));

                KeyValuePair<int, int> noWinings = _auction.NoWinings;
                if (_auction.collutions.ContainsKey(index))
                {
                    int x = noWinings.Value + 1;
                    _auction.NoWinings = new KeyValuePair<int, int>(noWinings.Key, x);
                }else
                {
                    int x = noWinings.Key + 1;
                    _auction.NoWinings = new KeyValuePair<int, int>(x, noWinings.Value);
                }

                Array.Sort(bidValues);
                Array.Reverse(bidValues);
                int winningPrice = bidValues[1]; // second price

                Console.WriteLine($"[auctioneer]: Auction finished. Sold to {highestBidder} for price {winningPrice}.");
                Broadcast($"winner {highestBidder}");
                
            }

            Stop();
        }

        private void SearchForCollutions()
        {
            int procentToChoose = Utils.RandNoGen.Next(0, 100);

            if(procentToChoose <= Utils.ProbabilityToDiscoverCollutions)
            {
                int value = 0;
                int key = 0;
                for (int i = 0; i < _auction.collutions.Count; i++)
                {
                    for(int j = 1; j <= _auction.NoBidders; j++)
                    {
                        if (_auction.collutions.ContainsKey(j))
                        {
                            key = j;
                            _auction.collutions.TryGetValue(key, out value);
                            _auction.collutions.Remove(key);
                            _auction.collutions.Remove(value);
                            try
                            {
                                Bid bidder = getBidByBidder(string.Format("bidder{0:D2}", key));
                                _bids.Remove(bidder);
                                Bid bidder2 = getBidByBidder(string.Format("bidder{0:D2}", value));
                                _bids.Remove(bidder2);
                                Console.WriteLine("[auctioneer]: Discovered a collution: {0} - {1}", key, value);
                                return;
                            }
                            catch
                            {
                                return;
                            }
                            
                        }
                    }                                             
                    

                    
                    
                }
                
            }
        }
    }
}