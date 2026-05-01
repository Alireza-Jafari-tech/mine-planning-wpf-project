using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ShortTermMinePlanning
{
    public partial class MainPlanningWindow : Window
    {
        private DataStore dataStore;
        private enum DataViewMode { None, Periods, Blocks, Destinations, Fleets }
        private DataViewMode currentViewMode = DataViewMode.None;

        public MainPlanningWindow()
        {
            InitializeComponent();
            dataStore = new DataStore();

            lstPeriods.ItemsSource = dataStore.TimePeriods;
            lstBlocks.ItemsSource = dataStore.ExtractionBlocks;
            lstDestinations.ItemsSource = dataStore.Destinations;
            lstFleets.ItemsSource = dataStore.Fleets;
        }

        private void BtnAddPeriod_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new PeriodAddWindow();
            if (addWindow.ShowDialog() == true)
            {
                dataStore.AddTimePeriod(addWindow.NewPeriod);
                RefreshDataDisplay();
            }
        }

        private void BtnAddBlock_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new BlockAddWindow();
            if (addWindow.ShowDialog() == true)
            {
                dataStore.AddExtractionBlock(addWindow.NewBlock);
                RefreshDataDisplay();
            }
        }

        private void BtnAddDestination_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new DestinationAddWindow();
            if (addWindow.ShowDialog() == true)
            {
                dataStore.AddDestination(addWindow.NewDestination);
                RefreshDataDisplay();
            }
        }

        private void BtnAddFleet_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new FleetAddWindow();
            if (addWindow.ShowDialog() == true)
            {
                dataStore.AddFleet(addWindow.NewFleet);
                RefreshDataDisplay();
            }
        }

        private void BtnShowPeriods_Click(object sender, RoutedEventArgs e)
        {
            currentViewMode = DataViewMode.Periods;
            var items = dataStore.TimePeriods.Select(p => new RecordDisplayItem
            {
                Id = p.Id,
                DisplayText = $"📅 دوره زمانی: {p.StartDate} تا {p.EndDate}\n" +
                             $"ظرفیت استخراج: {p.ExtractionCapacity:N0} {GetUnitText(p.ExtractionCapacityUnit)} | " +
                             $"ظرفیت حمل: {p.TransportationCapacity:N0} {GetUnitText(p.TransportationCapacityUnit)} | " +
                             $"حداکثر بلوک: {p.MaxExtractableBlocks}",
                OriginalObject = p
            }).ToList();
            dataItemsControl.ItemsSource = items;
        }

        private void BtnShowBlocks_Click(object sender, RoutedEventArgs e)
        {
            currentViewMode = DataViewMode.Blocks;
            var items = dataStore.ExtractionBlocks.Select(b => new RecordDisplayItem
            {
                Id = b.Id,
                DisplayText = $"⛏️ بلوک استخراج: {b.Tonnage:N0} {GetUnitText(b.TonnageUnit)}\n" +
                             $"عیار: {b.Grade:F2}% | درصد استخراج: {b.ExtractionPercent:F1}% | " +
                             $"هزینه استخراج: {b.ExtractionCost:N0} $/تن\n" +
                             $"تن قابل استخراج: {b.ExtractableTonnage:N0} تن",
                OriginalObject = b
            }).ToList();
            dataItemsControl.ItemsSource = items;
        }

        private void BtnShowDestinations_Click(object sender, RoutedEventArgs e)
        {
            currentViewMode = DataViewMode.Destinations;
            var items = dataStore.Destinations.Select(d => new RecordDisplayItem
            {
                Id = d.Id,
                DisplayText = $"🏭 مقصد: قیمت فروش {d.Price:N0} $/تن | " +
                             $"هزینه حمل: {d.TransportationCost:N0} $/تن\n" +
                             $"درآمد خالص: {(d.Price - d.TransportationCost):N0} $/تن | " +
                             $"نرخ بازیابی: {d.RecoveryRate:F1}%\n" +
                             $"عیار قابل قبول: {d.MinAcceptableGrade:F1}% تا {d.MaxAcceptableGrade:F1}% | " +
                             $"فاصله از معدن: {d.DistanceFromMine:F1} کیلومتر",
                OriginalObject = d
            }).ToList();
            dataItemsControl.ItemsSource = items;
        }

        private void BtnShowFleets_Click(object sender, RoutedEventArgs e)
        {
            currentViewMode = DataViewMode.Fleets;
            var items = dataStore.Fleets.Select(f => new RecordDisplayItem
            {
                Id = f.Id,
                DisplayText = $"🚛 ناوگان: {f.NumberOfTrucks} دستگاه کامیون\n" +
                             $"ظرفیت هر کامیون: {f.TruckCapacity:N0} {GetUnitText(f.TruckCapacityUnit)} | " +
                             $"هزینه حمل: {f.HaulCost:N0} $/کیلومتر\n" +
                             $"سرعت: {f.TruckSpeed} کیلومتر/ساعت | " +
                             $"زمان بارگیری: {f.LoadTime} ساعت | زمان تخلیه: {f.UnloadTime} ساعت\n" +
                             $"فاصله از بلوک: {f.DistanceFromBlock} کیلومتر | " +
                             $"تعداد سفر در دوره: {f.TripsPerPeriod}\n" +
                             $"ظرفیت کل در دوره: {f.TotalCapacityPerPeriod:N0} تن",
                OriginalObject = f
            }).ToList();
            dataItemsControl.ItemsSource = items;
        }

        private void BtnEditRecord_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var record = button?.Tag as RecordDisplayItem;
            if (record == null) return;

            switch (currentViewMode)
            {
                case DataViewMode.Periods:
                    var period = record.OriginalObject as TimePeriod;
                    var editPeriodWindow = new PeriodAddWindow(period);
                    if (editPeriodWindow.ShowDialog() == true)
                    {
                        dataStore.UpdateTimePeriod(editPeriodWindow.NewPeriod);
                        RefreshDataDisplay();
                        BtnShowPeriods_Click(null, null);
                    }
                    break;

                case DataViewMode.Blocks:
                    var block = record.OriginalObject as ExtractionBlock;
                    var editBlockWindow = new BlockAddWindow(block);
                    if (editBlockWindow.ShowDialog() == true)
                    {
                        dataStore.UpdateExtractionBlock(editBlockWindow.NewBlock);
                        RefreshDataDisplay();
                        BtnShowBlocks_Click(null, null);
                    }
                    break;

                case DataViewMode.Destinations:
                    var dest = record.OriginalObject as Destination;
                    var editDestWindow = new DestinationAddWindow(dest);
                    if (editDestWindow.ShowDialog() == true)
                    {
                        dataStore.UpdateDestination(editDestWindow.NewDestination);
                        RefreshDataDisplay();
                        BtnShowDestinations_Click(null, null);
                    }
                    break;

                case DataViewMode.Fleets:
                    var fleet = record.OriginalObject as Fleet;
                    var editFleetWindow = new FleetAddWindow(fleet);
                    if (editFleetWindow.ShowDialog() == true)
                    {
                        dataStore.UpdateFleet(editFleetWindow.NewFleet);
                        RefreshDataDisplay();
                        BtnShowFleets_Click(null, null);
                    }
                    break;
            }
        }

        private void BtnDeleteRecord_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var record = button?.Tag as RecordDisplayItem;
            if (record == null) return;

            var result = MessageBox.Show("آیا از حذف این رکورد اطمینان دارید؟",
                                        "تأیید حذف",
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                switch (currentViewMode)
                {
                    case DataViewMode.Periods:
                        dataStore.DeleteTimePeriod(record.OriginalObject as TimePeriod);
                        BtnShowPeriods_Click(null, null);
                        break;
                    case DataViewMode.Blocks:
                        dataStore.DeleteExtractionBlock(record.OriginalObject as ExtractionBlock);
                        BtnShowBlocks_Click(null, null);
                        break;
                    case DataViewMode.Destinations:
                        dataStore.DeleteDestination(record.OriginalObject as Destination);
                        BtnShowDestinations_Click(null, null);
                        break;
                    case DataViewMode.Fleets:
                        dataStore.DeleteFleet(record.OriginalObject as Fleet);
                        BtnShowFleets_Click(null, null);
                        break;
                }
                RefreshDataDisplay();
            }
        }

        private void RefreshDataDisplay()
        {
            lstPeriods.Items.Refresh();
            lstBlocks.Items.Refresh();
            lstDestinations.Items.Refresh();
            lstFleets.Items.Refresh();
        }

        private string GetUnitText(TonnageUnit unit)
        {
            return unit switch
            {
                TonnageUnit.تن => "تن",
                TonnageUnit.هزارتن => "هزار تن",
                TonnageUnit.میلیون‌تن => "میلیون تن",
                _ => "تن"
            };
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("آیا به صفحه انتخاب مدل بازمی‌گردید؟ داده‌های ذخیره شده پاک خواهند شد.",
                                        "بازگشت",
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var selectionWindow = new ModelSelectionWindow();
                selectionWindow.Show();
                this.Close();
            }
        }
    }

    public class RecordDisplayItem
    {
        public int Id { get; set; }
        public string DisplayText { get; set; }
        public object OriginalObject { get; set; }
    }
}