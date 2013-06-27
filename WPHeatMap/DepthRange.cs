using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WPHeatMap
{
    public class DepthRange
    {

        private readonly string fileName;
        public DepthRange(string fileName)
        {
            LowestBid = long.MaxValue;
            LowestAsk = long.MaxValue;
            Entries = new List<DepthEntry>();
            this.fileName = fileName;
        }

        public void Build(DateTime start, DateTime end)
        {
            int line = 0;
            using (StreamReader r = File.OpenText(fileName))
            {
                if (!r.EndOfStream)
                {
                    DepthEntry de;
                    string entry = r.ReadLine(); //Skip header line
                    bool done = r.EndOfStream;
                    while (!done)
                    {
                        entry = r.ReadLine();
                        de = new DepthEntry(entry);

                        if (de.DateTime >= start)
                        {
                            if (de.HighestBid > HighestBid)
                                HighestBid = de.HighestBid;
                            if (de.LowestBid > 0 && de.LowestBid < LowestBid)
                                LowestBid = de.LowestBid;

                            if (de.HighestAsk > HighestAsk)
                                HighestAsk = de.HighestAsk;
                            if (de.LowestAsk > 0 && de.LowestAsk < LowestAsk)
                                LowestAsk = de.LowestAsk;

                            line++;
                            Entries.Add(de);

                        }

                        done = r.EndOfStream || de.DateTime > end;
                    }
                }
            }
        }



        public List<DepthEntry> Entries
        {
            get;
            private set;
        }

        public long LowestBid { get; private set; }
        public long HighestBid { get; private set; }
        public long LowestAsk { get; private set; }
        public long HighestAsk { get; private set; }
    }

}
