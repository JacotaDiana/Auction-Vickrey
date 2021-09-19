using ActressMas;
using System;
using System.Collections.Generic;

namespace VickreyAuction
{
    public class BidderAgent : TurnBasedAgent
    {
        private int _valuation;
        private bool _isMallious;

        public BidderAgent(int val, bool isM)
        {
            _valuation = val;
            _isMallious = isM;
        }

        public BidderAgent(int val)
        {
            _valuation = val;
        }

        public override void Setup()
        {
            if (_isMallious == true)
            {
                Console.Write("**");
            }
            Console.WriteLine("[{0}]: My valuation is {1}", this.Name, _valuation);        
        }


        public override void Act(Queue<Message> messages)
        {
            while(messages.Count > 0)
            {
                Message message = messages.Dequeue();
                
                //Console.WriteLine("\t[{1} -> {0}]: {2}", this.Name, message.Sender, message.Content);

                string action;
                string parameters;
                Utils.ParseMessage(message.Content, out action, out parameters);

                switch (action)
                {
                    case "start":
                        HandleStart();
                        break;
                        
                    case "winner":
                        HandleWinner(parameters);
                        break;

                    default:
                        break;
                }
            }
        }

        private void HandleStart()
        {
            Send("auctioneer", $"bid {_valuation}");
        }

        private void HandleWinner(string winner)
        {
            if (winner == Name)
                Console.WriteLine($"[{Name}]: I have won.");

            Stop();
        }
    }
}