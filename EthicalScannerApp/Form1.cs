using System;
using System.Text;
using System.Windows.Forms;

namespace EthicalScannerFormsApp
{
    // The class name should match your form's file name (e.g., Form1)
    public partial class Form1 : Form
    {
        // --- Services ---
        private readonly BarcodeLookupService _barcodeService;
        private readonly EthicalAiService _aiService;

        public Form1()
        {
            // This special method loads the visual design from Form1.Designer.cs
            InitializeComponent();

            // --- API KEY CONFIGURATION ---
            // 1. Get your free API key from https://www.barcodelookup.com/api
            string barcodeApiKey = ""; // <-- PASTE YOUR BARCODELOOKUP.COM API KEY HERE

            // 2. Get your free API key from Google AI Studio
            string geminiApiKey = ""; // <-- PASTE YOUR GEMINI API KEY HERE

            _barcodeService = new BarcodeLookupService(barcodeApiKey);
            _aiService = new EthicalAiService(geminiApiKey);

            // Link the button click event handler after it has been created by InitializeComponent()
            this.scanButton.Click += new System.EventHandler(this.ScanButton_Click);
        }

        /// <summary>
        /// This method is called when the "Scan" button is clicked.
        /// </summary>
        private async void ScanButton_Click(object sender, EventArgs e)
        {
            string inputBarcode = barcodeTextBox.Text;
            if (string.IsNullOrWhiteSpace(inputBarcode))
            {
                MessageBox.Show("Please enter a barcode.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            scanButton.Enabled = false;
            scanButton.Text = "Scanning...";

            try
            {
                // STEP 1: Use the live BarcodeLookupService to get factual product info.
                resultsRichTextBox.Text = $"[Step 1] Calling Barcode API for: {inputBarcode}...";
                ProductInfo productInfo = await _barcodeService.GetProductInfoFromBarcodeAsync(inputBarcode);

                if (productInfo == null)
                {
                    resultsRichTextBox.Text = "This barcode was not found in the live product database.";
                    return;
                }

                // Display the initial findings for user verification
                resultsRichTextBox.Text = $"Scanned: {productInfo.ProductName}\n";
                resultsRichTextBox.AppendText($"Parent Company: {productInfo.ParentCompany}\n\n");

                // STEP 2: Pass the verified company name to the AI for ethical analysis.
                resultsRichTextBox.AppendText($"[Step 2] Performing AI analysis on {productInfo.ParentCompany}...");
                EthicalProfile profile = await _aiService.GetEthicalProfileAsync(productInfo.ParentCompany);

                // STEP 3: Display the final, combined results.
                DisplayCompanyProfile(productInfo, profile);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                resultsRichTextBox.Text = "An error occurred. Please try again.";
            }
            finally
            {
                scanButton.Enabled = true;
                scanButton.Text = "Scan";
            }
        }

        /// <summary>
        /// Displays the formatted ethical profile of a company in the RichTextBox.
        /// </summary>
        private void DisplayCompanyProfile(ProductInfo productInfo, EthicalProfile profile)
        {
            if (profile == null)
            {
                resultsRichTextBox.Text = $"Could not generate an ethical profile for {productInfo.ParentCompany}.";
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine($"Scanned Product: {productInfo.ProductName}");
            sb.AppendLine($"Ethical Profile for: {productInfo.ParentCompany}");
            sb.AppendLine("---------------------------------\n");
            sb.AppendLine($"Summary: {profile.Summary}\n");
            sb.AppendLine("Scores (out of 10):");
            sb.AppendLine($"  - Environmental Impact:  {profile.EnvironmentalScore}/10");
            sb.AppendLine($"  - Labor Practices:       {profile.LaborPracticesScore}/10");
            sb.AppendLine($"  - Social Responsibility: {profile.SocialResponsibilityScore}/10");
            sb.AppendLine($"  - AVERAGE SCORE:         {profile.AverageScore:F1}/10\n");
            sb.AppendLine("(+) Positive Highlights:");
            if (profile.PositiveHighlights?.Count > 0)
            {
                profile.PositiveHighlights.ForEach(h => sb.AppendLine($"  - {h}"));
            }
            else { sb.AppendLine("  - None listed."); }

            sb.AppendLine("\n(-) Ethical Concerns:");
            if (profile.EthicalConcerns?.Count > 0)
            {
                profile.EthicalConcerns.ForEach(c => sb.AppendLine($"  - {c}"));
            }
            else { sb.AppendLine("  - None listed."); }

            resultsRichTextBox.Text = sb.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
