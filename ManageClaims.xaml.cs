using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CMSC_Part_2
{
    public partial class ManageClaims : UserControl
    {
        private List<Claim> allClaims;

        public event Action NavigateBack; // Event for navigation

        public ManageClaims()
        {
            InitializeComponent();
            LoadClaimsData();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateBack?.Invoke(); // Trigger navigation event
        }

        private void LoadClaimsData()
        {
            // Sample data for demonstration
            allClaims = new List<Claim>
            {
                new Claim { LecturerName = "John Doe", Status = "Pending", HoursWorked = 45, HourlyRate = 50, TotalAmount = 2250, Date = DateTime.Today },
                new Claim { LecturerName = "Jane Smith", Status = "Approved", HoursWorked = 30, HourlyRate = 60, TotalAmount = 1800, Date = DateTime.Today.AddDays(-1) },
                new Claim { LecturerName = "Alice Brown", Status = "Rejected", HoursWorked = 50, HourlyRate = 55, TotalAmount = 2750, Date = DateTime.Today.AddDays(-2) }
            };

            ClaimsReportDataGrid.ItemsSource = allClaims;
        }

        private void ApplyFilters_Click(object sender, RoutedEventArgs e)
        {
            var selectedStatus = (FilterStatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            var selectedDate = FilterDatePicker.SelectedDate;

            var filteredClaims = allClaims.Where(c =>
                (selectedStatus == "All" || c.Status == selectedStatus) &&
                (!selectedDate.HasValue || c.Date.Date == selectedDate.Value.Date));

            ClaimsReportDataGrid.ItemsSource = filteredClaims.ToList();
        }

        private void ApproveClaim_Click(object sender, RoutedEventArgs e)
        {
            if (ClaimsReportDataGrid.SelectedItem is Claim selectedClaim && selectedClaim.Status == "Pending")
            {
                selectedClaim.Status = "Approved";
                ClaimsReportDataGrid.Items.Refresh();
                MessageBox.Show("Claim approved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Select a valid Pending claim to approve.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RejectClaim_Click(object sender, RoutedEventArgs e)
        {
            if (ClaimsReportDataGrid.SelectedItem is Claim selectedClaim && selectedClaim.Status == "Pending")
            {
                selectedClaim.Status = "Rejected";
                ClaimsReportDataGrid.Items.Refresh();
                MessageBox.Show("Claim rejected successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Select a valid Pending claim to reject.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MassApprove_Click(object sender, RoutedEventArgs e)
        {
            var selectedClaims = ClaimsReportDataGrid.SelectedItems.Cast<Claim>().ToList();
            foreach (var claim in selectedClaims)
            {
                if (claim.Status == "Pending")
                {
                    claim.Status = "Approved";
                }
            }

            ClaimsReportDataGrid.Items.Refresh();
            MessageBox.Show("Selected claims approved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MassReject_Click(object sender, RoutedEventArgs e)
        {
            var selectedClaims = ClaimsReportDataGrid.SelectedItems.Cast<Claim>().ToList();
            foreach (var claim in selectedClaims)
            {
                if (claim.Status == "Pending")
                {
                    claim.Status = "Rejected";
                }
            }

            ClaimsReportDataGrid.Items.Refresh();
            MessageBox.Show("Selected claims rejected successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    public class Claim
    {
        public string LecturerName { get; set; }
        public string Status { get; set; }
        public int HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime Date { get; set; }
    }
}
