using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Queues;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AgriStreamWeb.Controllers
{
    public class CommandController : Controller
    {
        private readonly QueueClient _queueClient;

        public CommandController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("AzureStorageConnection")!;
            // Establish a dedicated connection client mapping down to our serverless command route
            _queueClient = new QueueClient(connectionString, "irrigation-commands");
        }

        // POST: Command/TriggerIrrigation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TriggerIrrigation(int zoneId)
        {
            await _queueClient.CreateIfNotExistsAsync();

            // Construct a lightweight structured payload message string
            var commandPayload = new
            {
                Command = "ActivateIrrigationSolenoid",
                TargetZone = zoneId,
                DispatchedUtc = DateTimeOffset.UtcNow
            };

            string messageString = JsonSerializer.Serialize(commandPayload);

            // Drop the message payload cleanly straight onto the asynchronous queue highway
            await _queueClient.SendMessageAsync(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(messageString)));

            TempData["QueueMessage"] = $"Asynchronous irrigation command payload successfully dispatched for Zone {zoneId}!";
            return RedirectToAction("Dashboard", "Telemetry");
        }
    }
}
