using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UploadJSONFile.Application.Services
{
    public class JSONValidator : IJSONValidator
    {
        private readonly ILogger<Index> _logger;
        public JSONValidator(ILogger<Index> logger)
        {
            _logger = logger;
        }
        public bool ValidateAndSaveJsonFileAsync(string jsonContent)
        {
            try
            {
                // Attempt to parse the JSON content
                JObject jsonObject = JObject.Parse(jsonContent);

                // If parsing succeeds, save the JSON content to the database
                // Your database interaction logic goes here

                return true; // Return true to indicate successful validation and saving
            }
            catch (JsonReaderException ex)
            {
                // Handle JSON syntax errors
                // Log the error, return false, or throw a custom exception as needed
              _logger.LogInformation("Error parsing JSON: {ex.Message}");
                return false;
            }
        }
    }
}