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
            Entries = new List<DepthEntry>();
        }

       

        public void Build(DateTime start, DateTime end, int targetRange)
        {
            int line = 0;
            using (StreamReader r = File.OpenText("Data\\EURUSD_20130607_EBS_secondly.csv"))
            {
                DepthEntry de;
                string entry = r.ReadLine(); //Skip header line
                bool beforeEndDate = false;
                do
                {
                    entry = r.ReadLine();
                    de = new DepthEntry(entry);
                    beforeEndDate = de.DateTime <= end;
                    if (de.DateTime >= start && beforeEndDate)
                    {
                        
                        if (de.HighestBid > HighestBid)
                            HighestBid = de.HighestBid;
                        if (de.LowestBid > 0 && de.LowestBid < LowestBid)
                            LowestBid = de.LowestBid;

                        line++;
                        Entries.Add(de);
                    }
                    
                } while (!r.EndOfStream && beforeEndDate);
            }
        }

        public List<DepthEntry> Entries
        {
            get;
            private set;
        }

        public long LowestBid { get; private set; }
        public long HighestBid { get; private set; }
    }

}
