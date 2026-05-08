using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ShortTermMinePlanning
{
    public partial class MainPlanningWindow2 : Window
    {
        private DataStore2 dataStore;
        private enum DataViewMode { None, Periods, Blocks, Capacities, Targets, Fleets }
        private DataViewMode currentViewMode = DataViewMode.None;

        public MainPlanningWindow2()
        {
            InitializeComponent();
            dataStore = new DataStore2();

            lstPeriods.ItemsSource = dataStore.TimePeriods;
            lstBlocks.ItemsSource = dataStore.Blocks;
            lstCapacities.ItemsSource = dataStore.Capacities;
            lstTargets.ItemsSource = dataStore.Targets;
            lstFleets.ItemsSource = dataStore.Fleets;
        }

        private void BtnAddPeriod_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new PeriodAddWindow2();
            if (addWindow.ShowDialog() == true)
            {
                dataStore.AddTimePeriod(addWindow.NewPeriod);
                RefreshDataDisplay();
            }
        }

        private void BtnAddBlock_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new BlockAddWindow2();
            if (addWindow.ShowDialog() == true)
            {
                dataStore.AddBlock(addWindow.NewBlock);
                RefreshDataDisplay();
            }
        }

        private void BtnAddCapacity_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new CapacityAddWindow2();
            if (addWindow.ShowDialog() == true)
            {
                dataStore.AddCapacity(addWindow.NewCapacity);
                RefreshDataDisplay();
            }
        }

        private void BtnAddTarget_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new TargetAddWindow2();
            if (addWindow.ShowDialog() == true)
            {
                dataStore.AddTarget(addWindow.NewTarget);
                RefreshDataDisplay();
            }
        }

        private void BtnAddFleet_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new FleetAddWindow2();
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
                DisplayText = $"📅 دوره زمانی: {p.StartDate} تا {p.EndDate}",
                OriginalObject = p
            }).ToList();
            dataItemsControl.ItemsSource = items;
        }

        private void BtnShowBlocks_Click(object sender, RoutedEventArgs e)
        {
            currentViewMode = DataViewMode.Blocks;
            var items = dataStore.Blocks.Select(b => new RecordDisplayItem
            {
                Id = b.Id,
                DisplayText = $"⛏️ بلوک: {(b.BlockType == BlockType.سنگ_معدن ? "سنگ معدن" : "باطله")}\n" +
                             $"لایه: {b.LayerNumber}\n" +
                             $"توناز: {b.Tonnage:N0} {GetUnitText(b.TonnageUnit)}" +
                             (b.BlockType == BlockType.سنگ_معدن ? $"\nعیار: {b.Grade:F2}%" : ""),
                OriginalObject = b
            }).ToList();
            dataItemsControl.ItemsSource = items;
        }

        private void BtnShowCapacities_Click(object sender, RoutedEventArgs e)
        {
            currentViewMode = DataViewMode.Capacities;
            var items = dataStore.Capacities.Select(c => new RecordDisplayItem
            {
                Id = c.Id,
                DisplayText = $"⚙️ ظرفیت معدن: {c.MineMinCapacity:N0} تا {c.MineMaxCapacity:N0} {GetUnitText(c.MineCapacityUnit)}\n" +
                             $"ظرفیت کارخانه: {c.PlantMinCapacity:N0} تا {c.PlantMaxCapacity:N0} {GetUnitText(c.PlantCapacityUnit)}",
                OriginalObject = c
            }).ToList();
            dataItemsControl.ItemsSource = items;
        }

        private void BtnShowTargets_Click(object sender, RoutedEventArgs e)
        {
            currentViewMode = DataViewMode.Targets;
            var items = dataStore.Targets.Select(t => new RecordDisplayItem
            {
                Id = t.Id,
                DisplayText = $"🎯 تناژ ایده‌آل: {t.IdealTonnage:N0} {GetUnitText(t.IdealTonnageUnit)}\n" +
                             $"عیار ایده‌آل: {t.IdealGrade:F2}%\n" +
                             $"محدوده تناژ: {t.MinTonnage:N0} تا {t.MaxTonnage:N0} {GetUnitText(t.TonnageRangeUnit)}\n" +
                             $"محدوده عیار: {t.MinGrade:F2}% تا {t.MaxGrade:F2}%",
                OriginalObject = t
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
                             $"ظرفیت هر کامیون: {f.TruckCapacity:N0} {GetUnitText(f.TruckCapacityUnit)}\n" +
                             $"هزینه حمل: {f.TransportationCost:N0} $/تن\n" +
                             $"سرعت: {f.TruckSpeed} کیلومتر/ساعت\n" +
                             $"فاصله از بلوک: {f.DistanceFromBlock} کیلومتر\n" +
                             $"تعداد سفر در دوره: {f.TripsPerPeriod}",
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
                    var period = record.OriginalObject as TimePeriod2;
                    var editPeriodWindow = new PeriodAddWindow2(period);
                    if (editPeriodWindow.ShowDialog() == true)
                    {
                        dataStore.UpdateTimePeriod(editPeriodWindow.NewPeriod);
                        RefreshDataDisplay();
                        BtnShowPeriods_Click(null, null);
                    }
                    break;

                case DataViewMode.Blocks:
                    var block = record.OriginalObject as Block2;
                    var editBlockWindow = new BlockAddWindow2(block);
                    if (editBlockWindow.ShowDialog() == true)
                    {
                        dataStore.UpdateBlock(editBlockWindow.NewBlock);
                        RefreshDataDisplay();
                        BtnShowBlocks_Click(null, null);
                    }
                    break;

                case DataViewMode.Capacities:
                    var capacity = record.OriginalObject as Capacity2;
                    var editCapacityWindow = new CapacityAddWindow2(capacity);
                    if (editCapacityWindow.ShowDialog() == true)
                    {
                        dataStore.UpdateCapacity(editCapacityWindow.NewCapacity);
                        RefreshDataDisplay();
                        BtnShowCapacities_Click(null, null);
                    }
                    break;

                case DataViewMode.Targets:
                    var target = record.OriginalObject as Target2;
                    var editTargetWindow = new TargetAddWindow2(target);
                    if (editTargetWindow.ShowDialog() == true)
                    {
                        dataStore.UpdateTarget(editTargetWindow.NewTarget);
                        RefreshDataDisplay();
                        BtnShowTargets_Click(null, null);
                    }
                    break;

                case DataViewMode.Fleets:
                    var fleet = record.OriginalObject as Fleet2;
                    var editFleetWindow = new FleetAddWindow2(fleet);
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
                        dataStore.DeleteTimePeriod(record.OriginalObject as TimePeriod2);
                        BtnShowPeriods_Click(null, null);
                        break;
                    case DataViewMode.Blocks:
                        dataStore.DeleteBlock(record.OriginalObject as Block2);
                        BtnShowBlocks_Click(null, null);
                        break;
                    case DataViewMode.Capacities:
                        dataStore.DeleteCapacity(record.OriginalObject as Capacity2);
                        BtnShowCapacities_Click(null, null);
                        break;
                    case DataViewMode.Targets:
                        dataStore.DeleteTarget(record.OriginalObject as Target2);
                        BtnShowTargets_Click(null, null);
                        break;
                    case DataViewMode.Fleets:
                        dataStore.DeleteFleet(record.OriginalObject as Fleet2);
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
            lstCapacities.Items.Refresh();
            lstTargets.Items.Refresh();
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
}