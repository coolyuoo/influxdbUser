using InfluxData.Net.Common.Enums;
using InfluxData.Net.InfluxDb;
using InfluxData.Net.InfluxDb.Models;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using influxdbUser.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Xml.Linq;
using InfluxDB.Client.Core;

namespace influxdbUser.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        InfluxDbClient influxDbClient;

        public HomeController(ILogger<HomeController> logger)
        {



            

        }

        public async Task<IActionResult>  Index()
        {

            var token = Environment.GetEnvironmentVariable("r969UkEbddJuZ83tt0masekRe2ReHMNzLcFa-sVU0MTZB8b-v7a6-EanxGqvQUiWdDHnhxXpCBv1cIKSafB8rQ==");
            const string bucket = "ehdrink";
            const string org = "eh-company";

            using var influxDBClient = InfluxDBClientFactory.Create("http://localhost:8086", "r969UkEbddJuZ83tt0masekRe2ReHMNzLcFa-sVU0MTZB8b-v7a6-EanxGqvQUiWdDHnhxXpCBv1cIKSafB8rQ==".ToCharArray());

            using (var writeApi = influxDBClient.GetWriteApi())
            {
                //
                // Write by Point
                //
                var point = PointData.Measurement("temperature")
                    .Tag("location", "west")
                    .Field("value", 55D)
                    .Timestamp(DateTime.UtcNow.AddSeconds(-10), WritePrecision.Ns);

                writeApi.WritePoint(point, bucket, org);

                ////
                //// Write by LineProtocol
                ////
                //writeApi.WriteRecord("temperature,location=north value=60.0", WritePrecision.Ns, "bucket_name", "org_id");

                ////
                //// Write by POCO
                ////
                //var temperature = new Temperature { Location = "south", Value = 62D, Time = DateTime.UtcNow };
                //writeApi.WriteMeasurement(temperature, WritePrecision.Ns, "bucket_name", "org_id");
            }

            return View();
        }

        [Measurement("temperature")]
        private class Temperature
        {
            [Column("location", IsTag = true)] public string? Location { get; set; }

            [Column("value")] public double Value { get; set; }

            [Column(IsTimestamp = true)] public DateTime Time { get; set; }
        }

        public async void AddData()
        {
            //基于InfluxData.Net.InfluxDb.Models.Point实体准备数据
            var point_model = new Point()
            {
                Name = "Reading",//表名
                Tags = new Dictionary<string, object>()
               {
                   { "Id",  158}
               },
                Fields = new Dictionary<string, object>()
               {
                   { "Val", "webInfo" }
               },
                Timestamp = DateTime.UtcNow
            };
            var dbName = "code-hub";

            //从指定库中写入数据，支持传入多个对象的集合
            var response = await influxDbClient.Client.WriteAsync(point_model, "ehfood");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}