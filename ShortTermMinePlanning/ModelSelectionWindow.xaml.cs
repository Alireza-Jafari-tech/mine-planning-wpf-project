using System.Windows;

namespace ShortTermMinePlanning
{
    public partial class ModelSelectionWindow : Window
    {
        public ModelSelectionWindow()
        {
            InitializeComponent();
            txtModelDescription.Text = "لطفاً یکی از مدل‌های بالا را انتخاب کنید";
        }

        private void ModelSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (rbProfitModel.IsChecked == true)
            {
                txtModelDescription.Text = "📈 مدل حداکثر سود: این مدل بهترین برنامه استخراج را برای رسیدن به حداکثر سود بر اساس تن، عیار، هزینه‌ها و قیمت فروش پیدا می‌کند.";
                btnContinue.IsEnabled = true;
            }
            else if (rbLossModel.IsChecked == true)
            {
                txtModelDescription.Text = "📉 مدل حداقل ضرر: این مدل ضررهای ناشی از عدم تطابق با تن و عیار هدف را به حداقل می‌رساند.";
                btnContinue.IsEnabled = true;
            }
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            string helpText = @"راهنمای انتخاب مدل برنامه‌ریزی
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

✅ مدل حداکثر سود:
• زمان استفاده: زمانی که هدف اصلی، حداکثر کردن سود است
• کاربرد: معادنی که کنترل کامل روی برنامه استخراج دارند
• خروجی: برنامه استخراج با بالاترین سودآوری

⚠️ مدل حداقل ضرر:
• زمان استفاده: زمانی که محدودیت‌های تولید وجود دارد
• کاربرد: معادنی که باید به تعهدات تناژی و عیاری پایبند باشند
• خروجی: برنامه‌ای با کمترین میزان جریمه و ضرر

💡 نکته: برای شروع، مدل حداکثر سود را انتخاب کنید.";

            MessageBox.Show(helpText, "راهنمای انتخاب مدل",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnContinue_Click(object sender, RoutedEventArgs e)
        {
            if (rbProfitModel.IsChecked == true)
            {
                var mainWindow = new MainPlanningWindow();
                mainWindow.Show();
                this.Close();
            }
            else if (rbLossModel.IsChecked == true)
            {
                var mainWindow2 = new MainPlanningWindow2();
                mainWindow2.Show();
                this.Close();
            }
        }
    }
}