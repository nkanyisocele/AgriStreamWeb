using Azure;
using Azure.Data.Tables;
using System;

namespace AgriStreamWeb.Models
{
    public class TelemetryRow : ITableEntity
    {
        // Mandatory structural cloud keys required by Azure Table Storage
        public string PartitionKey { get; set; } = "FarmSensors"; // Group key
        public string RowKey { get; set; } = Guid.NewGuid().ToString(); // Unique item ID
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        // Core fields required by the assignment rubric
        public string DeviceId { get; set; } = string.Empty;
        public double SoilMoisturePercentage { get; set; }
        public double AmbientTemperature { get; set; }
    }
}
