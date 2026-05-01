using System;
using System.Windows;
using System.Windows.Controls;

namespace ShortTermMinePlanning
{
    public partial class BlockAddWindow : Window
    {
        public ExtractionBlock NewBlock { get; private set; }
        private ExtractionBlock editingBlock;

        public BlockAddWindow(ExtractionBlock block = null)
        {
            InitializeComponent();
            SetPlaceholders();

            if (block != null)
            {
                editingBlock = block;
                LoadBlockData();
            }
        }

        private void SetPlaceholders()
        {
            SetPlaceholder(txtTonnage, "مثال: 100");
            SetPlaceholder(txtGrade, "مثال: 2.5");
            SetPlaceholder(txtExtractionCost, "مثال: 15");
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

        private void LoadBlockData()
        {
            txtTonnage.Text = editingBlock.Tonnage.ToString();
            cboUnit.SelectedIndex = (int)editingBlock.TonnageUnit;
            sliderExtractionPercent.Value = editingBlock.ExtractionPercent;
            txtGrade.Text = editingBlock.Grade.ToString();
            txtExtractionCost.Text = editingBlock.ExtractionCost.ToString();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!double.TryParse(txtTonnage.Text, out double tonnage) || tonnage <= 0)
                {
                    MessageBox.Show("لطفاً مقدار تناژ معتبر وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(txtGrade.Text, out double grade) || grade <= 0 || grade > 100)
                {
                    MessageBox.Show("لطفاً عیار معتبر بین 0 تا 100 وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(txtExtractionCost.Text, out double cost) || cost <= 0)
                {
                    MessageBox.Show("لطفاً هزینه استخراج معتبر وارد کنید", "خطا",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                TonnageUnit selectedUnit = (TonnageUnit)cboUnit.SelectedIndex;

                NewBlock = new ExtractionBlock
                {
                    Tonnage = tonnage,
                    TonnageUnit = selectedUnit,
                    ExtractionPercent = sliderExtractionPercent.Value,
                    Grade = grade,
                    ExtractionCost = cost
                };

                if (editingBlock != null)
                {
                    NewBlock.Id = editingBlock.Id;
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