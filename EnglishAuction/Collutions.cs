using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VickreyAuction
{
    //This class is not used yet
    public class Collution
    {
        private BidderAgent _bidderAgent1;
        private BidderAgent _bidderAgent2;
        private int _procentToBeDiscovered = 0;

        public Collution(BidderAgent bidder1, BidderAgent bidder2, int procentToBeDiscovered)
        {
            _bidderAgent1 = bidder1;
            _bidderAgent2 = bidder2;
            _procentToBeDiscovered = procentToBeDiscovered;
        }

        public KeyValuePair<string, string> removeIfDiscovered()
        {
            int procentToChoose = Utils.RandNoGen.Next(0, 100);

            if (_procentToBeDiscovered <= procentToChoose)
            {
                return new KeyValuePair<string, string>(_bidderAgent1.Name, _bidderAgent2.Name);
            }
            return new KeyValuePair<string, string>("", "");
        }
    }
}
