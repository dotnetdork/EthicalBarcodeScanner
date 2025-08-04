using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EthicalScannerFormsApp
{
    // =================================================================================
    //  DATA MODELS
    // =================================================================================
    public class ProductInfo
    {
        public string ProductName { get; set; }
        public string ParentCompany { get; set; }
    }

    public class EthicalProfile
    {
        [JsonPropertyName("summary")] public string Summary { get; set; }
        [JsonPropertyName("environmentalScore")] public int EnvironmentalScore { get; set; }
        [JsonPropertyName("laborPracticesScore")] public int LaborPracticesScore { get; set; }
        [JsonPropertyName("socialResponsibilityScore")] public int SocialResponsibilityScore { get; set; }
        [JsonPropertyName("positiveHighlights")] public List<string> PositiveHighlights { get; set; }
        [JsonPropertyName("ethicalConcerns")] public List<string> EthicalConcerns { get; set; }
        [JsonIgnore] public double AverageScore => (EnvironmentalScore + LaborPracticesScore + SocialResponsibilityScore) / 3.0;
    }

    // --- Helper classes for parsing the BarcodeLookup.com API response ---
    public class BarcodeLookupResponse
    {
        [JsonPropertyName("products")]
        public List<ProductData> Products { get; set; }
    }

    public class ProductData
    {
        [JsonPropertyName("product_name")]
        public string ProductName { get; set; }

        [JsonPropertyName("manufacturer")]
        public string Manufacturer { get; set; }

        [JsonPropertyName("brand")]
        public string Brand { get; set; }
    }

    // =================================================================================
    //  SERVICES
    // =================================================================================
    public class BarcodeLookupService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiKey;

        public BarcodeLookupService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<ProductInfo> GetProductInfoFromBarcodeAsync(string barcode)
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
                MessageBox.Show("BarcodeLookup.com API key is not configured.", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            // Construct the API request URL
            string apiUrl = $"https://api.barcodelookup.com/v3/products?barcode={barcode}&formatted=y&key={_apiKey}";

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Barcode API Error: {response.StatusCode}", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                var jsonString = await response.Content.ReadAsStringAsync();
                var lookupResponse = JsonSerializer.Deserialize<BarcodeLookupResponse>(jsonString);

                var productData = lookupResponse?.Products?.FirstOrDefault();
                if (productData == null)
                {
                    return null; // Barcode not found
                }

                // Create our standard ProductInfo object from the API response
                // Prioritize the 'manufacturer' field, but fall back to 'brand' if needed.
                return new ProductInfo
                {
                    ProductName = productData.ProductName,
                    ParentCompany = !string.IsNullOrEmpty(productData.Manufacturer) ? productData.Manufacturer : productData.Brand
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during barcode lookup: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }

    public class EthicalAiService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiKey;
        public EthicalAiService(string apiKey) { _apiKey = apiKey; }

        public async Task<EthicalProfile> GetEthicalProfileAsync(string companyName)
        {
            if (string.IsNullOrEmpty(companyName)) return null;

            var prompt = $@"
You are an expert corporate ethics analyst. Your task is to research the company '{companyName}' and provide an objective, summary ethical profile.
Based on publicly available information, analyze their environmental impact, labor practices, and social responsibility.
Provide your response ONLY as a JSON object that strictly follows this C# class structure:
```json
{{
  ""summary"": ""A brief, neutral summary of the company's overall ethical standing."",
  ""environmentalScore"": <integer, 1-10>,
  ""laborPracticesScore"": <integer, 1-10>,
  ""socialResponsibilityScore"": <integer, 1-10>,
  ""positiveHighlights"": [""A list of specific, positive ethical actions or initiatives.""],
  ""ethicalConcerns"": [""A list of specific, documented ethical concerns or controversies.""]
}}
```
Do not include any text or formatting outside of the JSON object.";

            var responseJson = await CallAiServiceAsync(prompt);
            if (string.IsNullOrEmpty(responseJson)) return null;

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<EthicalProfile>(responseJson, options);
        }

        private async Task<string> CallAiServiceAsync(string prompt)
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
                MessageBox.Show("Gemini AI API Key is not configured.", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            string apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash-preview-05-20:generateContent?key={_apiKey}";
            var payload = new { contents = new[] { new { parts = new[] { new { text = prompt } } } } };
            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(apiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"AI Service Error: {response.StatusCode}\n{errorBody}", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(responseBody);

            var rawText = jsonDoc.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();
            var cleanedJson = rawText.Trim().Replace("```json", "").Replace("```", "");

            return cleanedJson;
        }
    }
}