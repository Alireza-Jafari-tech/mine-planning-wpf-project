using System;
using System.Windows;
using System.Windows.Controls;

namespace ShortTermMinePlanning
{
    public partial class BlockAddWindow2 : Window
    {
        public Block2 NewBlock { get; private set; }
        private Block2 editingBlock;

        public BlockAddWindow2(Block2 block = null)
        {
            InitializeComponent();
            SetPlaceholders();
            rbOre.IsChecked = true;

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

        private void BlockTypeChanged(object sender, RoutedEventArgs e)
        {
            bool isOre = rbOre.IsChecked == true;
            lblGrade.Visibility = isOre ? Visibility.Visible : Visibility.Collapsed;
            txtGrade.Visibility = isOre ? Visibility.Visible : Visibility.Collapsed;

            if (!isOre)
            {
                txtGrade.Text = "0";
            }
        }

        private void LoadBlockData()
        {
            if (editingBlock.BlockType == BlockType.سنگ_معدن)
            {
                rbOre.IsChecked = true;
                txtGrade.Text = editingBlock.Grade.ToString();
            }
            else
            {
                rbWaste.IsChecked = true;
                txtGrade.Visibility = Visibility.Collapsed;
                lblGrade.Visibility = Visibility.Collapsed;
            }

            txtTonnage.Text = editingBlock.Tonnage.ToString();
            cboUnit.SelectedIndex = (int)editingBlock.TonnageUnit;
            sliderExtractionPercent.Value = editingBlock.ExtractionPercent;
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

                bool isOre = rbOre.IsChecked == true;
                double grade = 0;

                if (isOre)
                {
                    if (!double.TryParse(txtGrade.Text, out grade) || grade <= 0 || grade > 100)
                    {
                        MessageBox.Show("لطفاً عیار معتبر بین 0 تا 100 وارد کنید", "خطا",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                NewBlock = new Block2
                {
                    BlockType = isOre ? BlockType.سنگ_معدن : BlockType.باطله,
                    Tonnage = tonnage,
                    TonnageUnit = (TonnageUnit)cboUnit.SelectedIndex,
                    Grade = grade,
                    ExtractionPercent = sliderExtractionPercent.Value
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