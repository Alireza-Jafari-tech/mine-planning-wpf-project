using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ShortTermMinePlanning
{
    public partial class PeriodAddWindow2 : Window
    {
        public TimePeriod2 NewPeriod { get; private set; }
        private TimePeriod2 editingPeriod;

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

        public PeriodAddWindow2(TimePeriod2 period = null)
        {
            InitializeComponent();
            InitializeDateComboBoxes();

            if (period != null)
            {
                editingPeriod = period;
                LoadPeriodData();
            }
        }

        private void InitializeDateComboBoxes()
        {
            for (int year = 1400; year <= 1410; year++)
            {
                cboStartYear.Items.Add(year);
                cboEndYear.Items.Add(year);
            }

            foreach (var month in persianMonths)
            {
                cboStartMonth.Items.Add(month.Value);
                cboEndMonth.Items.Add(month.Value);
            }

            for (int day = 1; day <= 31; day++)
            {
                cboStartDay.Items.Add(day);
                cboEndDay.Items.Add(day);
            }

            cboStartYear.SelectedIndex = 0;
            cboStartMonth.SelectedIndex = 0;
            cboStartDay.SelectedIndex = 0;
            cboEndYear.SelectedIndex = 0;
            cboEndMonth.SelectedIndex = 0;
            cboEndDay.SelectedIndex = 0;

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

        private void LoadPeriodData()
        {
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

                NewPeriod = new TimePeriod2
                {
                    StartDate = GetPersianDate(cboStartYear, cboStartMonth, cboStartDay),
                    EndDate = GetPersianDate(cboEndYear, cboEndMonth, cboEndDay)
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