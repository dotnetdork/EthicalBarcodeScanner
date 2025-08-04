# Ethical Barcode Scanner

A Windows Forms desktop application designed to bring corporate transparency to your shopping cart. By combining a real-time barcode lookup API with the analytical power of large language models, this tool provides consumers with an instant ethical analysis of a product's parent company.

## Tech Stack

* **Language:** C#
* **Framework:** .NET / Windows Forms
* **APIs:**
    * BarcodeLookup.com API for product data
    * Google Gemini API for ethical analysis

## How It Works

1.  **Scan:** The user enters a product's UPC barcode.
2.  **Identify:** The app makes a live API call to BarcodeLookup.com to accurately identify the product and its parent company.
3.  **Analyze:** The parent company's name is then sent to the Google Gemini AI, which performs a deep-dive analysis of its ethical standing based on publicly available data.
4.  **Report:** The app displays a detailed profile, including scores for environmental impact, labor practices, and social responsibility, along with a summary of positive highlights and ethical concerns.

## Setup

To run this application, you will need to obtain two API keys:

1.  **BarcodeLookup.com API Key**: Sign up for a free account at [https://www.barcodelookup.com/api](https://www.barcodelookup.com/api) to get your key.
2.  **Google Gemini API Key**: Get your key from the [Google AI Studio](https://aistudio.google.com/app/apikey).

Once you have your keys, open the project in Visual Studio and navigate to the `EthicalScannerForm.cs` file. Paste your keys into the designated placeholders at the top of the `EthicalScannerForm` constructor:

```csharp
// 1. Get your free API key from [https://www.barcodelookup.com/api](https://www.barcodelookup.com/api)
string barcodeApiKey = ""; // <-- PASTE YOUR BARCODELOOKUP.COM API KEY HERE

// 2. Get your free API key from Google AI Studio
string geminiApiKey = ""; // <-- PASTE YOUR GEMINI API KEY HERE
