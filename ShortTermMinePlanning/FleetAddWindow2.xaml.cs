using System;
using System.Windows;
using System.Windows.Controls;

namespace ShortTermMinePlanning
{
    public partial class FleetAddWindow2 : Window
    {
        public Fleet2 NewFleet { get; private set; }
        private Fleet2 editingFleet;

        public FleetAddWindow2(Fleet2 fleet = null)
        {
            InitializeComponent();
            SetAllPlaceholders();

            if (fleet != null)
            {
                editingFleet = fleet;
                LoadFleetData();
            }
        }

        private void SetAllPlaceholders()
        {
            SetPlaceholder(txtNumberOfTrucks, "مثال: 10");
            SetPlaceholder(txtTruckCapacity, "مثال: 50");
            SetPlaceholder(txtTransportationCost, "مثال: 2.5");
            SetPlaceholder(txtTruckSpeed, "مثال: 40");
            SetPlaceholder(txtLoadTime, "مثال: 0.5");
            SetPlaceholder(txtUnloadTime, "مثال: 0.25");
            SetPlaceholder(txtDistanceFromBlock, "مثال: 15");
            SetPlaceholder(txtTripsPerPeriod, "مثال: 20");
        }

        private void SetPlaceholder(TextBox textBox, string placeholder)
        {
            textBox.Text = placeholder;
            textBox.Foreground = System.Windows.Media.Brushes.Gray;
            textBox.GotFocus += (s, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.Foreground = System.Windows.Media.Brushes.Black;
                }
            };
            textBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.Foreground = System.Windows.Media.Brushes.Gray;
                }
            };
        }

        private void LoadFleetData()
        {
            txtNumberOfTrucks.Text = editingFleet.NumberOfTrucks.ToString();
            txtTruckCapacity.Text = editingFleet.TruckCapacity.ToString();
            cboCapacityUnit.SelectedIndex = (int)editingFleet.TruckCapacityUnit;
            txtTransportationCost.Text = editingFleet.TransportationCost.ToString();
            txtTruckSpeed.Text = editingFleet.TruckSpeed.ToString();
            txtLoadTime.Text = editingFleet.LoadTime.ToString();
            txtUnloadTime.Text = editingFleet.UnloadTime.ToString();
            txtDistanceFromBlock.Text = editingFleet.DistanceFromBlock.ToString();
            txtTripsPerPeriod.Text = editingFleet.TripsPerPeriod.ToString();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(txtNumberOfTrucks.Text, out int numberOfTrucks) || numberOfTrucks <= 0)
                {
                    MessageBox.Show("لطفاً تعداد کامیون معتبر وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(txtTruckCapacity.Text, out double truckCapacity) || truckCapacity <= 0)
                {
                    MessageBox.Show("لطفاً ظرفیت کامیون معتبر وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(txtTransportationCost.Text, out double transportationCost) || transportationCost <= 0)
                {
                    MessageBox.Show("لطفاً هزینه حمل معتبر وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(txtTruckSpeed.Text, out double truckSpeed) || truckSpeed <= 0)
                {
                    MessageBox.Show("لطفاً سرعت کامیون معتبر وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(txtLoadTime.Text, out double loadTime) || loadTime < 0)
                {
                    MessageBox.Show("لطفاً زمان بارگیری معتبر وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(txtUnloadTime.Text, out double unloadTime) || unloadTime < 0)
                {
                    MessageBox.Show("لطفاً زمان تخلیه معتبر وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(txtDistanceFromBlock.Text, out double distance) || distance < 0)
                {
                    MessageBox.Show("لطفاً فاصله معتبر وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!int.TryParse(txtTripsPerPeriod.Text, out int trips) || trips <= 0)
                {
                    MessageBox.Show("لطفاً تعداد سفر در دوره معتبر وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                NewFleet = new Fleet2
                {
                    NumberOfTrucks = numberOfTrucks,
                    TruckCapacity = truckCapacity,
                    TruckCapacityUnit = (TonnageUnit)cboCapacityUnit.SelectedIndex,
                    TransportationCost = transportationCost,
                    TruckSpeed = truckSpeed,
                    LoadTime = loadTime,
                    UnloadTime = unloadTime,
                    DistanceFromBlock = distance,
                    TripsPerPeriod = trips
                };

                if (editingFleet != null)
                {
                    NewFleet.Id = editingFleet.Id;
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا: {ex.Message}", "خطا",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}