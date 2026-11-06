using System;
using System.Text.Json;
using Azure.Data.Tables;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AgriStreamFunctions
{
    public class IrrigationProcessorFunction
    {
        private readonly ILogger<IrrigationProcessorFunction> _logger;

        public IrrigationProcessorFunction(ILogger<IrrigationProcessorFunction> logger)
        {
            _logger = logger;
        }

        [Function("ProcessIrrigationCommand")]
        [TableOutput("AuditLogs", Connection = "AzureStorageConnection")] // Automatic NoSQL Table Output Binding
        public TableEntity Run(
            [QueueTrigger("irrigation-commands", Connection = "AzureStorageConnection")] string queueMessage)
        {
            _logger.LogInformation($"[Serverless Pipeline] Intercepted new queue payload: {queueMessage}");

            try
            {
                // Parse the incoming JSON command message string sent by the MVC app
                using JsonDocument doc = JsonDocument.Parse(queueMessage);
                JsonElement root = doc.RootElement;

                string command = root.GetProperty("Command").GetString() ?? "Unknown";
                int targetZone = root.GetProperty("TargetZone").GetInt32();

                _logger.LogInformation($"[Execution Engine] Activating physical irrigation system valves for Zone {targetZone}...");

                // Construct an Audit log row entity map to automatically save to our AuditLogs table
                TableEntity auditLog = new TableEntity("IrrigationAudits", Guid.NewGuid().ToString())
                {
                    { "ActionExecuted", command },
                    { "TargetZoneId", targetZone },
                    { "ProcessedTimestampUtc", DateTimeOffset.UtcNow }
                };

                return auditLog;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Pipeline Failure] Failed to parse command message payload payload: {ex.Message}");
                throw;
            }
        }
    }
}
