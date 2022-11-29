using InfluxDB.Client.Writes;
using InfluxDB.Client;
using System;
using System.Diagnostics;
using System.Timers;
using System.Collections.Generic;
using System.Linq;

namespace GetComputerInfo
{
    internal class Program
    {

        static PerformanceCounter cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        static PerformanceCounter memory = new PerformanceCounter("Memory", "% Committed Bytes in Use");

        const string bucket = "ehdrink"; //database

        const string org = "eh-company"; //organization

        const string table = "ComputerInfo"; //organization

        static InfluxDBClient influxDBClient;

        static void Main(string[] args)
        {
            Console.WriteLine("start");

            influxDBClient = InfluxDBClientFactory.Create("http://localhost:8086", "r969UkEbddJuZ83tt0masekRe2ReHMNzLcFa-sVU0MTZB8b-v7a6-EanxGqvQUiWdDHnhxXpCBv1cIKSafB8rQ==".ToCharArray());



            var timer = new Timer();
            timer.AutoReset = true;
            timer.Interval = 10 * 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();


            Console.ReadLine();

        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {

            var infos = new Dictionary<string, float>
            {
                { "CPU", cpu.NextValue() },
                { "memory", memory.NextValue() }
            };


            using (var writeApi = influxDBClient.GetWriteApi())
            {
                var ps = infos.Select(i =>
                  {

                      var point = PointData
                          .Measurement(table)
                          .Tag("Item", i.Key)
                          .Field("data", i.Value);

                      return point;
                  }).ToList();


                writeApi.WritePoints(ps, bucket, org);
            }
        }
    }
}
