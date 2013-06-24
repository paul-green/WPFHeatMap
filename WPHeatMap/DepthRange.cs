using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WPHeatMap
{
    class DepthRange
    {
        public DepthRange()
        {
            LowestBid = long.MaxValue;
        }

        public void Build(int low, int high)
        {
            int line = 0;
            using (StreamReader r = File.OpenText("Data\\EURUSD_20130607_EBS_secondly.csv"))
            {

                string entry = r.ReadLine(); ;
                while (!r.EndOfStream && line <= high)
                {
                    if (line >= low)
                    {
                        entry = r.ReadLine();
                        DepthEntry de = new DepthEntry(entry);
                        if (de.HighBid > HighestBid)
                            HighestBid = de.HighBid;
                        if (de.LowBid < LowestBid)
                            LowestBid = de.LowBid;


                    }
                    line++;
                }
            }
        }

        public long LowestBid { get; private set; }
        public long HighestBid { get; private set; }
    }

}
