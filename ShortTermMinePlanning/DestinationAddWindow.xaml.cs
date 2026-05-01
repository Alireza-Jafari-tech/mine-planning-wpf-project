using System;
using System.Windows;
using System.Windows.Controls;

namespace ShortTermMinePlanning
{
    public partial class DestinationAddWindow : Window
    {
        public Destination NewDestination { get; private set; }
        private Destination editingDestination;

        public DestinationAddWindow(Destination destination = null)
        {
            InitializeComponent();
            SetAllPlaceholders();

            if (destination != null)
            {
                editingDestination = destination;
                LoadDestinationData();
            }
        }

        private void SetAllPlaceholders()
        {
            SetPlaceholder(txtPrice, "مثال: 50");
            SetPlaceholder(txtTransportationCost, "مثال: 10");
            SetPlaceholder(txtDistance, "مثال: 25.5");
            SetPlaceholder(txtMinGrade, "مثال: 1.5");
            SetPlaceholder(txtMaxGrade, "مثال: 3.5");
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

        private void LoadDestinationData()
        {
            txtPrice.Text = editingDestination.Price.ToString();
            txtTransportationCost.Text = editingDestination.TransportationCost.ToString();
            txtDistance.Text = editingDestination.DistanceFromMine.ToString();
            sliderRecovery.Value = editingDestination.RecoveryRate;
            txtMinGrade.Text = editingDestination.MinAcceptableGrade.ToString();
            txtMaxGrade.Text = editingDestination.MaxAcceptableGrade.ToString();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!double.TryParse(txtPrice.Text, out double price) || price <= 0)
                {
                    MessageBox.Show("لطفاً قیمت معتبر وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(txtTransportationCost.Text, out double transportCost) || transportCost < 0)
                {
                    MessageBox.Show("لطفاً هزینه حمل معتبر وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(txtDistance.Text, out double distance) || distance < 0)
                {
                    MessageBox.Show("لطفاً فاصله معتبر وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(txtMinGrade.Text, out double minGrade) || minGrade < 0 || minGrade > 100)
                {
                    MessageBox.Show("لطفاً حداقل عیار معتبر بین 0 تا 100 وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(txtMaxGrade.Text, out double maxGrade) || maxGrade < 0 || maxGrade > 100)
                {
                    MessageBox.Show("لطفاً حداکثر عیار معتبر بین 0 تا 100 وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (minGrade >= maxGrade)
                {
                    MessageBox.Show("حداقل عیار باید کمتر از حداکثر عیار باشد", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                NewDestination = new Destination
                {
                    Price = price,
                    TransportationCost = transportCost,
                    DistanceFromMine = distance,
                    RecoveryRate = sliderRecovery.Value,
                    MinAcceptableGrade = minGrade,
                    MaxAcceptableGrade = maxGrade
                };

                if (editingDestination != null)
                {
                    NewDestination.Id = editingDestination.Id;
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