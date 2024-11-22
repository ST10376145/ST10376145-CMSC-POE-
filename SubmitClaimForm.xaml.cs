using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace CMSC_Part_2
{
    /// <summary>
    /// Interaction logic for SubmitClaimForm.xaml
    /// </summary>
    public partial class SubmitClaimForm : UserControl
    {
        private string uploadedFilePath;
        public event Action NavigateBack;
        public SubmitClaimForm()
        {
            InitializeComponent();
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf|Word Documents (*.docx)|*.docx|Excel Files (*.xlsx)|*.xlsx",
                Title = "Select Supporting Document"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);

                // File size limit set to 5 MB
                if (fileInfo.Length > 5 * 1024 * 1024)
                {
                    MessageBox.Show("File size exceeds 5 MB. Please select a smaller file.", "File Size Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                uploadedFilePath = openFileDialog.FileName;
                FileNameTextBlock.Text = $"Uploaded File: {System.IO.Path.GetFileName(uploadedFilePath)}";
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateBack?.Invoke(); // Trigger the event to navigate back
        }

        private void SubmitClaim_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(HoursWorkedTextBox.Text) ||
                string.IsNullOrWhiteSpace(HourlyRateTextBox.Text) ||
                uploadedFilePath == null)
            {
                MessageBox.Show("Please fill in all required fields and upload a document.", "Form Incomplete", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show("Claim Submitted Successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            ResetForm();
        }

        private void ResetForm_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            HoursWorkedTextBox.Clear();
            HourlyRateTextBox.Clear();
            NotesTextBox.Clear();
            FileNameTextBlock.Text = "";
            uploadedFilePath = null;
        }
        private void HoursWorkedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Validate and parse Hours Worked
            bool isHoursValid = decimal.TryParse(HoursWorkedTextBox.Text, out decimal hours) && hours > 0;

            // Validate and parse Hourly Rate
            bool isRateValid = decimal.TryParse(HourlyRateTextBox.Text, out decimal rate) && rate >= 0;

            // Check validity of inputs
            if (isHoursValid && isRateValid)
            {
                // Perform calculation and format as currency
                decimal totalPayment = hours * rate;
                FinalPaymentTextBlock.Text = $"Total Payment: {totalPayment:C}";

                // Clear any previous error message
                FinalPaymentTextBlock.ToolTip = null;
            }
            else
            {
                // Display error message
                FinalPaymentTextBlock.Text = "Invalid input";
                FinalPaymentTextBlock.ToolTip = "Ensure Hours Worked is positive and Hourly Rate is non-negative.";
            }
        }

        // Restrict input to numeric-only for Hours Worked
        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {
            // Allow only integers
            e.Handled = !int.TryParse(e.Text, out _);
        }

        // Restrict input to decimal values for Hourly Rate
        private void DecimalOnly(object sender, TextCompositionEventArgs e)
        {
            // Allow only decimals
            e.Handled = !decimal.TryParse(e.Text, out _);
        }
    }
}

