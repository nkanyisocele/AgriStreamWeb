using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AgriStreamWeb.Controllers
{
    public class DroneController : Controller
    {
        private readonly string _storageConnectionString;

        public DroneController(IConfiguration configuration)
        {
            // Grab our secure storage token directly from appsettings.json
            _storageConnectionString = configuration.GetConnectionString("AzureStorageConnection")!;
        }

        // GET: Drone/Upload
        public IActionResult Upload()
        {
            return View();
        }

        // POST: Drone/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                ViewBag.Error = "Please select a valid drone aerial map file to upload.";
                return View();
            }

            try
            {
                // Instantiate our service connection client
                BlobServiceClient blobServiceClient = new BlobServiceClient(_storageConnectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("drone-images");

                // Generate a unique filename to prevent accidental overrides in the cloud
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                BlobClient blobClient = containerClient.GetBlobClient(uniqueFileName);

                // Stream the uploaded bytes directly to Azure Blob Storage
                using (var stream = imageFile.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }

                ViewBag.Message = $"File successfully uploaded to Azure Blob! Saved as: {uniqueFileName}";
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Cloud storage upload failed: {ex.Message}";
            }

            return View();
        }
    }
}

