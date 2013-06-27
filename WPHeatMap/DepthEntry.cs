using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPHeatMap
{
    public class DepthEntry
    {
        private const int DEPTH_MAX = 10;

        public DepthEntry(string line)
        {
            string[] split = line.Split(',');
            bids = ParseSizes(split, out lowestBid, out highestBid, 5);
            asks = ParseSizes(split, out lowestAsk, out highestAsk, 35);
            date = DateTime.Parse(split[1].Replace('D', ' '));
        }

        private long[] bids = new long[DEPTH_MAX];
        public long[] Bids
        {
            get { return bids; }
        }

        private long[] asks = new long[DEPTH_MAX];
        public long[] Asks
        {
            get { return asks; }
        }


        private long[] ParseSizes(string[] split, out long low, out long high, int fi)
        {
            low = 0;
            high = 0;
            long[] target = new long[DEPTH_MAX];
            for (int i = 0; i < DEPTH_MAX; i++)
            {
                long value;
                long.TryParse(split[fi + i * 3], out value);
                target[i] = value;
                if (value > high)
                    high = value;
                else if (low == 0 || (value < low && value > 0))
                    low = value;
            }
            return target;


        }

        private long lowestBid;
        public long LowestBid
        {
            get { return lowestBid; }
        }

        private long highestBid;
        public long HighestBid
        {
            get { return highestBid; }
        }

        private long lowestAsk;
        public long LowestAsk
        {
            get { return lowestAsk; }
        }

        private long highestAsk;
        public long HighestAsk
        {
            get { return highestAsk; }
        }

        private DateTime date;
        public DateTime DateTime
        {
            get
            {
                return date;
            }
        }



    }
}
