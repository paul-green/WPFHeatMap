using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPHeatMap
{

    public class DepthSlice
    {
        public DepthSlice()
        {

        }


    }

    class DepthEntry
    {
        public DepthEntry()
        {
            
        }
        public DepthEntry(string line)
        {
            LowBid = long.MaxValue;
            string[] split = line.Split(',');
            Bids = new double[10];

            for (int i = 0; i < 10; i++)
            {
                long bid;
                long.TryParse(split[5 + i * 3], out bid);
                Bids[i] = bid;
                if (bid > HighBid)
                    HighBid = bid;
                if (bid > 0 && bid < LowBid)
                    LowBid = bid;
            }
            Asks = new double[10];

            for (int i = 0; i < 10; i++)
            {
                long ask;
                long.TryParse(split[35 + i * 3], out ask);
                Asks[i] = ask;
                
            }

            DateTime date;
            
            DateTime.TryParse(split[1].Replace('D', ' '), out date);
            this.DateTime = date;
            

        }

        public long HighBid { get; private set; }
        public long LowBid { get; private set; }

        public DateTime DateTime { get; private set; }

        public double[] Bids
        {
            get;
            private set;
        }
        public double[] Asks
        {
            get;
            private set;
        }
    
    }
}
