using System;
using System.Windows;
using System.Windows.Controls;

namespace ShortTermMinePlanning
{
    public partial class CapacityAddWindow2 : Window
    {
        public Capacity2 NewCapacity { get; private set; }
        private Capacity2 editingCapacity;

        public CapacityAddWindow2(Capacity2 capacity = null)
        {
            InitializeComponent();
            SetPlaceholders();

            if (capacity != null)
            {
                editingCapacity = capacity;
                LoadCapacityData();
            }
        }

        private void SetPlaceholders()
        {
            SetPlaceholder(txtMineMinCapacity, "مثال: 5000");
            SetPlaceholder(txtMineMaxCapacity, "مثال: 10000");
            SetPlaceholder(txtPlantMinCapacity, "مثال: 4500");
            SetPlaceholder(txtPlantMaxCapacity, "مثال: 9000");
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

        private void LoadCapacityData()
        {
            txtMineMinCapacity.Text = editingCapacity.MineMinCapacity.ToString();
            txtMineMaxCapacity.Text = editingCapacity.MineMaxCapacity.ToString();
            cboMineUnit.SelectedIndex = (int)editingCapacity.MineCapacityUnit;
            cboMineMaxUnit.SelectedIndex = (int)editingCapacity.MineCapacityUnit;

            txtPlantMinCapacity.Text = editingCapacity.PlantMinCapacity.ToString();
            txtPlantMaxCapacity.Text = editingCapacity.PlantMaxCapacity.ToString();
            cboPlantUnit.SelectedIndex = (int)editingCapacity.PlantCapacityUnit;
            cboPlantMaxUnit.SelectedIndex = (int)editingCapacity.PlantCapacityUnit;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!double.TryParse(txtMineMinCapacity.Text, out double mineMin) || mineMin <= 0)
                {
                    MessageBox.Show("لطفاً حداقل ظرفیت معدن معتبر وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(txtMineMaxCapacity.Text, out double mineMax) || mineMax <= 0)
                {
                    MessageBox.Show("لطفاً حداکثر ظرفیت معدن معتبر وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (mineMin > mineMax)
                {
                    MessageBox.Show("حداقل ظرفیت معدن نباید از حداکثر بیشتر باشد", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(txtPlantMinCapacity.Text, out double plantMin) || plantMin <= 0)
                {
                    MessageBox.Show("لطفاً حداقل ظرفیت کارخانه معتبر وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(txtPlantMaxCapacity.Text, out double plantMax) || plantMax <= 0)
                {
                    MessageBox.Show("لطفاً حداکثر ظرفیت کارخانه معتبر وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (plantMin > plantMax)
                {
                    MessageBox.Show("حداقل ظرفیت کارخانه نباید از حداکثر بیشتر باشد", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                NewCapacity = new Capacity2
                {
                    MineMinCapacity = mineMin,
                    MineMaxCapacity = mineMax,
                    MineCapacityUnit = (TonnageUnit)cboMineUnit.SelectedIndex,
                    PlantMinCapacity = plantMin,
                    PlantMaxCapacity = plantMax,
                    PlantCapacityUnit = (TonnageUnit)cboPlantUnit.SelectedIndex
                };

                if (editingCapacity != null)
                {
                    NewCapacity.Id = editingCapacity.Id;
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