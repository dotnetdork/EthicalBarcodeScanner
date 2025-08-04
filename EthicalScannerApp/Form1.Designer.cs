namespace EthicalScannerFormsApp
{
    // The class name should match your form's file name (e.g., Form1)
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            barcodeTextBox = new TextBox();
            scanButton = new Button();
            resultsRichTextBox = new RichTextBox();
            infoLabel = new Label();
            SuspendLayout();
            // 
            // barcodeTextBox
            // 
            barcodeTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            barcodeTextBox.Font = new Font("Segoe UI", 9F);
            barcodeTextBox.Location = new Point(10, 30);
            barcodeTextBox.Name = "barcodeTextBox";
            barcodeTextBox.Size = new Size(375, 23);
            barcodeTextBox.TabIndex = 0;
            // 
            // scanButton
            // 
            scanButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            scanButton.BackColor = Color.DodgerBlue;
            scanButton.FlatAppearance.BorderSize = 0;
            scanButton.FlatStyle = FlatStyle.Flat;
            scanButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            scanButton.ForeColor = Color.White;
            scanButton.Location = new Point(390, 30);
            scanButton.Name = "scanButton";
            scanButton.Size = new Size(108, 23);
            scanButton.TabIndex = 1;
            scanButton.Text = "Scan";
            scanButton.UseVisualStyleBackColor = false;
            // 
            // resultsRichTextBox
            // 
            resultsRichTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            resultsRichTextBox.BorderStyle = BorderStyle.FixedSingle;
            resultsRichTextBox.Font = new Font("Consolas", 9.5F);
            resultsRichTextBox.Location = new Point(10, 70);
            resultsRichTextBox.Name = "resultsRichTextBox";
            resultsRichTextBox.ReadOnly = true;
            resultsRichTextBox.Size = new Size(489, 531);
            resultsRichTextBox.TabIndex = 2;
            resultsRichTextBox.Text = "";
            // 
            // infoLabel
            // 
            infoLabel.AutoSize = true;
            infoLabel.Font = new Font("Segoe UI", 9F);
            infoLabel.Location = new Point(8, 8);
            infoLabel.Name = "infoLabel";
            infoLabel.Size = new Size(221, 15);
            infoLabel.TabIndex = 3;
            infoLabel.Text = "Enter a barcode number and click 'Scan'.";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(509, 612);
            Controls.Add(infoLabel);
            Controls.Add(resultsRichTextBox);
            Controls.Add(scanButton);
            Controls.Add(barcodeTextBox);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(352, 377);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Ethical Scanner (Live API + AI)";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox barcodeTextBox;
        private System.Windows.Forms.Button scanButton;
        private System.Windows.Forms.RichTextBox resultsRichTextBox;
        private System.Windows.Forms.Label infoLabel;
    }
}