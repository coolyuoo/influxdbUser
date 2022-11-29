using InfluxDB.Client.Core;
using System;

namespace GetComputerInfo
{
    [Measurement("temperature")]
    public class Temperature
    {
        [Column("location", IsTag = true)] 
        public string Location { get; set; }

        [Column("value")] 
        public double Value { get; set; }

        [Column(IsTimestamp = true)] 
        public DateTime Time { get; set; }
    }
}
