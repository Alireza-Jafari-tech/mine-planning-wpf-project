using System;
using System.Windows;
using System.Windows.Controls;

namespace ShortTermMinePlanning
{
    public partial class TargetAddWindow2 : Window
    {
        public Target2 NewTarget { get; private set; }
        private Target2 editingTarget;

        public TargetAddWindow2(Target2 target = null)
        {
            InitializeComponent();
            SetPlaceholders();

            if (target != null)
            {
                editingTarget = target;
                LoadTargetData();
            }
        }

        private void SetPlaceholders()
        {
            SetPlaceholder(txtIdealTonnage, "مثال: 100000");
            SetPlaceholder(txtIdealGrade, "مثال: 2.5");
            SetPlaceholder(txtMinTonnage, "مثال: 80000");
            SetPlaceholder(txtMaxTonnage, "مثال: 120000");
            SetPlaceholder(txtMinGrade, "مثال: 2.0");
            SetPlaceholder(txtMaxGrade, "مثال: 3.0");
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

        private void LoadTargetData()
        {
            txtIdealTonnage.Text = editingTarget.IdealTonnage.ToString();
            cboIdealUnit.SelectedIndex = (int)editingTarget.IdealTonnageUnit;
            txtIdealGrade.Text = editingTarget.IdealGrade.ToString();

            txtMinTonnage.Text = editingTarget.MinTonnage.ToString();
            txtMaxTonnage.Text = editingTarget.MaxTonnage.ToString();
            cboTonnageRangeUnit.SelectedIndex = (int)editingTarget.TonnageRangeUnit;
            cboTonnageRangeMaxUnit.SelectedIndex = (int)editingTarget.TonnageRangeUnit;

            txtMinGrade.Text = editingTarget.MinGrade.ToString();
            txtMaxGrade.Text = editingTarget.MaxGrade.ToString();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!double.TryParse(txtIdealTonnage.Text, out double idealTonnage) || idealTonnage <= 0)
                {
                    MessageBox.Show("لطفاً تناژ ایده‌آل معتبر وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(txtIdealGrade.Text, out double idealGrade) || idealGrade <= 0 || idealGrade > 100)
                {
                    MessageBox.Show("لطفاً عیار ایده‌آل معتبر بین 0 تا 100 وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(txtMinTonnage.Text, out double minTonnage) || minTonnage <= 0)
                {
                    MessageBox.Show("لطفاً حداقل تناژ معتبر وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(txtMaxTonnage.Text, out double maxTonnage) || maxTonnage <= 0)
                {
                    MessageBox.Show("لطفاً حداکثر تناژ معتبر وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (minTonnage > maxTonnage)
                {
                    MessageBox.Show("حداقل تناژ نباید از حداکثر بیشتر باشد", "خطا",
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

                if (minGrade > maxGrade)
                {
                    MessageBox.Show("حداقل عیار نباید از حداکثر بیشتر باشد", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                NewTarget = new Target2
                {
                    IdealTonnage = idealTonnage,
                    IdealTonnageUnit = (TonnageUnit)cboIdealUnit.SelectedIndex,
                    IdealGrade = idealGrade,
                    MinTonnage = minTonnage,
                    MaxTonnage = maxTonnage,
                    TonnageRangeUnit = (TonnageUnit)cboTonnageRangeUnit.SelectedIndex,
                    MinGrade = minGrade,
                    MaxGrade = maxGrade
                };

                if (editingTarget != null)
                {
                    NewTarget.Id = editingTarget.Id;
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