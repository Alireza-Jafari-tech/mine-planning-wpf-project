using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ShortTermMinePlanning
{
    public partial class PeriodAddWindow : Window
    {
        public TimePeriod NewPeriod { get; private set; }
        private TimePeriod editingPeriod;

        private Dictionary<int, string> persianMonths = new Dictionary<int, string>
        {
            {1, "فروردین"}, {2, "اردیبهشت"}, {3, "خرداد"}, {4, "تیر"},
            {5, "مرداد"}, {6, "شهریور"}, {7, "مهر"}, {8, "آبان"},
            {9, "آذر"}, {10, "دی"}, {11, "بهمن"}, {12, "اسفند"}
        };

        private Dictionary<int, int> monthDays = new Dictionary<int, int>
        {
            {1, 31}, {2, 31}, {3, 31}, {4, 31}, {5, 31}, {6, 31},
            {7, 30}, {8, 30}, {9, 30}, {10, 30}, {11, 30}, {12, 29}
        };

        public PeriodAddWindow(TimePeriod period = null)
        {
            InitializeComponent();

            InitializeDateComboBoxes();
            SetPlaceholders();

            if (period != null)
            {
                editingPeriod = period;
                LoadPeriodData();
            }
        }

        private void InitializeDateComboBoxes()
        {
            // Fill years (1400-1410)
            for (int year = 1400; year <= 1410; year++)
            {
                cboStartYear.Items.Add(year);
                cboEndYear.Items.Add(year);
            }

            // Fill months
            foreach (var month in persianMonths)
            {
                cboStartMonth.Items.Add(month.Value);
                cboEndMonth.Items.Add(month.Value);
            }

            // Fill days (1-31 initially)
            for (int day = 1; day <= 31; day++)
            {
                cboStartDay.Items.Add(day);
                cboEndDay.Items.Add(day);
            }

            // Set default selections
            cboStartYear.SelectedIndex = 0;
            cboStartMonth.SelectedIndex = 0;
            cboStartDay.SelectedIndex = 0;
            cboEndYear.SelectedIndex = 0;
            cboEndMonth.SelectedIndex = 0;
            cboEndDay.SelectedIndex = 0;

            // Handle month change to update days
            cboStartMonth.SelectionChanged += (s, e) => UpdateDays(cboStartMonth, cboStartDay);
            cboEndMonth.SelectionChanged += (s, e) => UpdateDays(cboEndMonth, cboEndDay);
        }

        private void UpdateDays(ComboBox monthCombo, ComboBox dayCombo)
        {
            int selectedMonth = monthCombo.SelectedIndex + 1;
            int maxDays = monthDays[selectedMonth];

            dayCombo.Items.Clear();
            for (int day = 1; day <= maxDays; day++)
            {
                dayCombo.Items.Add(day);
            }
            dayCombo.SelectedIndex = 0;
        }

        private string GetPersianDate(ComboBox yearCombo, ComboBox monthCombo, ComboBox dayCombo)
        {
            int year = (int)yearCombo.SelectedItem;
            int month = monthCombo.SelectedIndex + 1;
            int day = (int)dayCombo.SelectedItem;
            return $"{year}/{month:D2}/{day:D2}";
        }

        private void SetPlaceholders()
        {
            SetPlaceholder(txtExtractionCapacity, "مثال: 5000");
            SetPlaceholder(txtTransportationCapacity, "مثال: 5000");
            SetPlaceholder(txtMaxBlocks, "مثال: 10");
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

        private void LoadPeriodData()
        {
            // Parse Persian date string
            var startParts = editingPeriod.StartDate.Split('/');
            var endParts = editingPeriod.EndDate.Split('/');

            if (startParts.Length == 3)
            {
                cboStartYear.SelectedItem = int.Parse(startParts[0]);
                cboStartMonth.SelectedIndex = int.Parse(startParts[1]) - 1;
                UpdateDays(cboStartMonth, cboStartDay);
                cboStartDay.SelectedItem = int.Parse(startParts[2]);
            }

            if (endParts.Length == 3)
            {
                cboEndYear.SelectedItem = int.Parse(endParts[0]);
                cboEndMonth.SelectedIndex = int.Parse(endParts[1]) - 1;
                UpdateDays(cboEndMonth, cboEndDay);
                cboEndDay.SelectedItem = int.Parse(endParts[2]);
            }

            txtExtractionCapacity.Text = editingPeriod.ExtractionCapacity.ToString();
            cboExtractionUnit.SelectedIndex = (int)editingPeriod.ExtractionCapacityUnit;
            txtTransportationCapacity.Text = editingPeriod.TransportationCapacity.ToString();
            cboTransportUnit.SelectedIndex = (int)editingPeriod.TransportationCapacityUnit;
            txtMaxBlocks.Text = editingPeriod.MaxExtractableBlocks.ToString();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cboStartYear.SelectedItem == null || cboStartMonth.SelectedItem == null || cboStartDay.SelectedItem == null ||
                    cboEndYear.SelectedItem == null || cboEndMonth.SelectedItem == null || cboEndDay.SelectedItem == null)
                {
                    MessageBox.Show("لطفاً تاریخ شروع و پایان را کامل انتخاب کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(txtExtractionCapacity.Text, out double extractionCap) || extractionCap <= 0)
                {
                    MessageBox.Show("لطفاً ظرفیت استخراج معتبر وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(txtTransportationCapacity.Text, out double transportCap) || transportCap <= 0)
                {
                    MessageBox.Show("لطفاً ظرفیت حمل معتبر وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!int.TryParse(txtMaxBlocks.Text, out int maxBlocks) || maxBlocks <= 0)
                {
                    MessageBox.Show("لطفاً حداکثر بلوک‌های قابل استخراج معتبر وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                NewPeriod = new TimePeriod
                {
                    StartDate = GetPersianDate(cboStartYear, cboStartMonth, cboStartDay),
                    EndDate = GetPersianDate(cboEndYear, cboEndMonth, cboEndDay),
                    ExtractionCapacity = extractionCap,
                    ExtractionCapacityUnit = (TonnageUnit)cboExtractionUnit.SelectedIndex,
                    TransportationCapacity = transportCap,
                    TransportationCapacityUnit = (TonnageUnit)cboTransportUnit.SelectedIndex,
                    MaxExtractableBlocks = maxBlocks
                };

                if (editingPeriod != null)
                {
                    NewPeriod.Id = editingPeriod.Id;
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