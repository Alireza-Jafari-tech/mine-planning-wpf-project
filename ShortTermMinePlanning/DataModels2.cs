using System;
using System.Collections.ObjectModel;

namespace ShortTermMinePlanning
{
    //public enum TonnageUnit
    //{
    //    تن,
    //    هزارتن,
    //    میلیون‌تن
    //}

    public enum BlockType
    {
        سنگ_معدن,
        باطله
    }

    // Time Period for Model 2
    public class TimePeriod2
    {
        public int Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    // Block for Model 2
    public class Block2
    {
        public int Id { get; set; }
        public BlockType BlockType { get; set; }
        public double Tonnage { get; set; }
        public TonnageUnit TonnageUnit { get; set; }
        public double Grade { get; set; } // Only for Ore, 0 for Waste
        public double ExtractionPercent { get; set; } = 100;

        public double TonnageInTons
        {
            get
            {
                return TonnageUnit switch
                {
                    TonnageUnit.تن => Tonnage,
                    TonnageUnit.هزارتن => Tonnage * 1000,
                    TonnageUnit.میلیون‌تن => Tonnage * 1000000,
                    _ => Tonnage
                };
            }
        }
    }

    // Capacity for Model 2
    public class Capacity2
    {
        public int Id { get; set; }
        public double MineMinCapacity { get; set; }
        public double MineMaxCapacity { get; set; }
        public TonnageUnit MineCapacityUnit { get; set; }
        public double PlantMinCapacity { get; set; }
        public double PlantMaxCapacity { get; set; }
        public TonnageUnit PlantCapacityUnit { get; set; }

        public double MineMinCapacityInTons
        {
            get
            {
                return MineCapacityUnit switch
                {
                    TonnageUnit.تن => MineMinCapacity,
                    TonnageUnit.هزارتن => MineMinCapacity * 1000,
                    TonnageUnit.میلیون‌تن => MineMinCapacity * 1000000,
                    _ => MineMinCapacity
                };
            }
        }

        public double MineMaxCapacityInTons
        {
            get
            {
                return MineCapacityUnit switch
                {
                    TonnageUnit.تن => MineMaxCapacity,
                    TonnageUnit.هزارتن => MineMaxCapacity * 1000,
                    TonnageUnit.میلیون‌تن => MineMaxCapacity * 1000000,
                    _ => MineMaxCapacity
                };
            }
        }

        public double PlantMinCapacityInTons
        {
            get
            {
                return PlantCapacityUnit switch
                {
                    TonnageUnit.تن => PlantMinCapacity,
                    TonnageUnit.هزارتن => PlantMinCapacity * 1000,
                    TonnageUnit.میلیون‌تن => PlantMinCapacity * 1000000,
                    _ => PlantMinCapacity
                };
            }
        }

        public double PlantMaxCapacityInTons
        {
            get
            {
                return PlantCapacityUnit switch
                {
                    TonnageUnit.تن => PlantMaxCapacity,
                    TonnageUnit.هزارتن => PlantMaxCapacity * 1000,
                    TonnageUnit.میلیون‌تن => PlantMaxCapacity * 1000000,
                    _ => PlantMaxCapacity
                };
            }
        }
    }

    // Target for Model 2
    public class Target2
    {
        public int Id { get; set; }
        public double IdealTonnage { get; set; }
        public TonnageUnit IdealTonnageUnit { get; set; }
        public double IdealGrade { get; set; }
        public double MinTonnage { get; set; }
        public double MaxTonnage { get; set; }
        public TonnageUnit TonnageRangeUnit { get; set; }
        public double MinGrade { get; set; }
        public double MaxGrade { get; set; }

        public double IdealTonnageInTons
        {
            get
            {
                return IdealTonnageUnit switch
                {
                    TonnageUnit.تن => IdealTonnage,
                    TonnageUnit.هزارتن => IdealTonnage * 1000,
                    TonnageUnit.میلیون‌تن => IdealTonnage * 1000000,
                    _ => IdealTonnage
                };
            }
        }

        public double MinTonnageInTons
        {
            get
            {
                return TonnageRangeUnit switch
                {
                    TonnageUnit.تن => MinTonnage,
                    TonnageUnit.هزارتن => MinTonnage * 1000,
                    TonnageUnit.میلیون‌تن => MinTonnage * 1000000,
                    _ => MinTonnage
                };
            }
        }

        public double MaxTonnageInTons
        {
            get
            {
                return TonnageRangeUnit switch
                {
                    TonnageUnit.تن => MaxTonnage,
                    TonnageUnit.هزارتن => MaxTonnage * 1000,
                    TonnageUnit.میلیون‌تن => MaxTonnage * 1000000,
                    _ => MaxTonnage
                };
            }
        }
    }

    // Fleet for Model 2 (similar to Model 1 but transportation cost is $/ton)
    public class Fleet2
    {
        public int Id { get; set; }
        public int NumberOfTrucks { get; set; }
        public double TruckCapacity { get; set; }
        public TonnageUnit TruckCapacityUnit { get; set; }
        public double TransportationCost { get; set; } // $/ton
        public double TruckSpeed { get; set; }
        public double LoadTime { get; set; }
        public double UnloadTime { get; set; }
        public double DistanceFromBlock { get; set; }
        public int TripsPerPeriod { get; set; }

        public double TruckCapacityInTons
        {
            get
            {
                return TruckCapacityUnit switch
                {
                    TonnageUnit.تن => TruckCapacity,
                    TonnageUnit.هزارتن => TruckCapacity * 1000,
                    TonnageUnit.میلیون‌تن => TruckCapacity * 1000000,
                    _ => TruckCapacity
                };
            }
        }

        public double TotalCapacityPerTrip => NumberOfTrucks * TruckCapacityInTons;
        public double TotalCapacityPerPeriod => TotalCapacityPerTrip * TripsPerPeriod;
        public double CycleTime => LoadTime + UnloadTime + (DistanceFromBlock / TruckSpeed) * 2;
    }

    // DataStore for Model 2
    public class DataStore2
    {
        public ObservableCollection<TimePeriod2> TimePeriods { get; set; } = new ObservableCollection<TimePeriod2>();
        public ObservableCollection<Block2> Blocks { get; set; } = new ObservableCollection<Block2>();
        public ObservableCollection<Capacity2> Capacities { get; set; } = new ObservableCollection<Capacity2>();
        public ObservableCollection<Target2> Targets { get; set; } = new ObservableCollection<Target2>();
        public ObservableCollection<Fleet2> Fleets { get; set; } = new ObservableCollection<Fleet2>();

        private int nextPeriodId = 1;
        private int nextBlockId = 1;
        private int nextCapacityId = 1;
        private int nextTargetId = 1;
        private int nextFleetId = 1;

        public void AddTimePeriod(TimePeriod2 period)
        {
            period.Id = nextPeriodId++;
            TimePeriods.Add(period);
        }

        public void UpdateTimePeriod(TimePeriod2 period)
        {
            var existing = TimePeriods.FirstOrDefault(p => p.Id == period.Id);
            if (existing != null)
            {
                var index = TimePeriods.IndexOf(existing);
                TimePeriods[index] = period;
            }
        }

        public void DeleteTimePeriod(TimePeriod2 period)
        {
            TimePeriods.Remove(period);
        }

        public void AddBlock(Block2 block)
        {
            block.Id = nextBlockId++;
            Blocks.Add(block);
        }

        public void UpdateBlock(Block2 block)
        {
            var existing = Blocks.FirstOrDefault(b => b.Id == block.Id);
            if (existing != null)
            {
                var index = Blocks.IndexOf(existing);
                Blocks[index] = block;
            }
        }

        public void DeleteBlock(Block2 block)
        {
            Blocks.Remove(block);
        }

        public void AddCapacity(Capacity2 capacity)
        {
            capacity.Id = nextCapacityId++;
            Capacities.Add(capacity);
        }

        public void UpdateCapacity(Capacity2 capacity)
        {
            var existing = Capacities.FirstOrDefault(c => c.Id == capacity.Id);
            if (existing != null)
            {
                var index = Capacities.IndexOf(existing);
                Capacities[index] = capacity;
            }
        }

        public void DeleteCapacity(Capacity2 capacity)
        {
            Capacities.Remove(capacity);
        }

        public void AddTarget(Target2 target)
        {
            target.Id = nextTargetId++;
            Targets.Add(target);
        }

        public void UpdateTarget(Target2 target)
        {
            var existing = Targets.FirstOrDefault(t => t.Id == target.Id);
            if (existing != null)
            {
                var index = Targets.IndexOf(existing);
                Targets[index] = target;
            }
        }

        public void DeleteTarget(Target2 target)
        {
            Targets.Remove(target);
        }

        public void AddFleet(Fleet2 fleet)
        {
            fleet.Id = nextFleetId++;
            Fleets.Add(fleet);
        }

        public void UpdateFleet(Fleet2 fleet)
        {
            var existing = Fleets.FirstOrDefault(f => f.Id == fleet.Id);
            if (existing != null)
            {
                var index = Fleets.IndexOf(existing);
                Fleets[index] = fleet;
            }
        }

        public void DeleteFleet(Fleet2 fleet)
        {
            Fleets.Remove(fleet);
        }
    }
}