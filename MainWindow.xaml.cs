using System.Windows;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Security.Claims;
using System.Windows.Controls;

namespace CMSC_Part_2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SubmitClaim_Click(object sender, RoutedEventArgs e)
        {
            var submitClaimForm = new SubmitClaimForm();
            submitClaimForm.NavigateBack += NavigateToMainMenu;
            MainContent.Content = submitClaimForm;
        }

        private void ManageClaims_Click(object sender, RoutedEventArgs e)
        {
            var manageClaims = new ManageClaims();
            manageClaims.NavigateBack += NavigateToMainMenu;
            MainContent.Content = manageClaims;
        }

        private void NavigateToMainMenu()
        {
            MainContent.Content = null; // Clear the frame to show the main menu
        }
        private void GeneratePdf()
        {
            string filePath = "ClaimSummary.pdf";

            try
            {
                Document document = new Document();

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    PdfWriter.GetInstance(document, stream);
                    document.Open();
                    document.Add(new Paragraph("Claim Summary for November"));
                    document.Add(new Paragraph("Generated on: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                    PdfPTable table = new PdfPTable(3);
                    table.AddCell("Lecturer Name");
                    table.AddCell("Hours Worked");
                    table.AddCell("Total Payment");
                    table.AddCell("John Doe");
                    table.AddCell("35");
                    table.AddCell("$1050");

                    document.Add(table);
                    document.Close();
                }

                MessageBox.Show($"PDF generated successfully at {filePath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void GeneratePdfButton_Click(object sender, RoutedEventArgs e)
        {
            GeneratePdf();
        }
        private List<Claim> allClaims; // Assume this is populated with data

        private void ApplyFilters()
        {
            // Get selected status
            string selectedStatus = (FilterStatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Filter claims
            var filteredClaims = allClaims.Where(c =>
                (selectedStatus == "All" || c.Status == selectedStatus) &&
                (!FilterDatePicker.SelectedDate.HasValue || c.Date == FilterDatePicker.SelectedDate.Value));

            // Display filtered claims
            ClaimsReportDataGrid.ItemsSource = filteredClaims.ToList();
        }
    }
}
