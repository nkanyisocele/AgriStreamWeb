using Microsoft.AspNetCore.Mvc;
using Azure.Data.Tables;
using AgriStreamWeb.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AgriStreamWeb.Controllers
{
    public class TelemetryController : Controller
    {
        private readonly TableClient _tableClient;

        public TelemetryController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("AzureStorageConnection")!;
            // Connect directly to our TelemetryData NoSQL table instance
            _tableClient = new TableClient(connectionString, "TelemetryData");
        }

        // GET: Telemetry/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            // Ensure table metadata structures exist in the cloud ecosystem
            await _tableClient.CreateIfNotExistsAsync();

            List<TelemetryRow> logsList = new List<TelemetryRow>();

            // Query out all matching sensor row data records
            var queryResults = _tableClient.QueryAsync<TelemetryRow>(filter: $"PartitionKey eq 'FarmSensors'");
            await foreach (var row in queryResults)
            {
                logsList.Add(row);
            }

            return View(logsList);
        }

        // POST: Telemetry/AddMockRecord
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMockRecord()
        {
            // Generate some randomized sensor readings to emulate real field hardware updates
            var random = new Random();
            var mockEntry = new TelemetryRow
            {
                DeviceId = "ZONAL-NODE-" + random.Next(101, 109),
                SoilMoisturePercentage = Math.Round(random.NextDouble() * (85.0 - 20.0) + 20.0, 1),
                AmbientTemperature = Math.Round(random.NextDouble() * (38.0 - 14.0) + 14.0, 1),
                Timestamp = DateTimeOffset.UtcNow
            };

            // Push our structured NoSQL data entry straight to the cloud instance
            await _tableClient.AddEntityAsync(mockEntry);

            return RedirectToAction("Dashboard");
        }
    }
}
