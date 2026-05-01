using System;
using System.Windows;
using System.Windows.Controls;

namespace ShortTermMinePlanning
{
    public partial class FleetAddWindow : Window
    {
        public Fleet NewFleet { get; private set; }
        private Fleet editingFleet;

        public FleetAddWindow(Fleet fleet = null)
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
            SetPlaceholder(txtHaulCost, "مثال: 0.5");
            SetPlaceholder(txtTruckSpeed, "مثال: 40");
            SetPlaceholder(txtLoadTime, "مثال: 0.5");
            SetPlaceholder(txtUnloadTime, "مثال: 0.25");
            SetPlaceholder(txtDistanceFromBlock, "مثال: 15");
            SetPlaceholder(txtTripsPerPeriod, "مثال: 20");
        }

        private void SetPlaceholder(Control textBox, string placeholder)
        {
            if (textBox is TextBox txtBox)
            {
                txtBox.Text = placeholder;
                txtBox.Foreground = System.Windows.Media.Brushes.Gray;
                txtBox.GotFocus += (s, e) =>
                {
                    if (txtBox.Text == placeholder)
                    {
                        txtBox.Text = "";
                        txtBox.Foreground = System.Windows.Media.Brushes.Black;
                    }
                };
                txtBox.LostFocus += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(txtBox.Text))
                    {
                        txtBox.Text = placeholder;
                        txtBox.Foreground = System.Windows.Media.Brushes.Gray;
                    }
                };
            }
        }

        private void LoadFleetData()
        {
            txtNumberOfTrucks.Text = editingFleet.NumberOfTrucks.ToString();
            txtTruckCapacity.Text = editingFleet.TruckCapacity.ToString();
            cboCapacityUnit.SelectedIndex = (int)editingFleet.TruckCapacityUnit;
            txtHaulCost.Text = editingFleet.HaulCost.ToString();
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

                if (!double.TryParse(txtHaulCost.Text, out double haulCost) || haulCost <= 0)
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

                NewFleet = new Fleet
                {
                    NumberOfTrucks = numberOfTrucks,
                    TruckCapacity = truckCapacity,
                    TruckCapacityUnit = (TonnageUnit)cboCapacityUnit.SelectedIndex,
                    HaulCost = haulCost,
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