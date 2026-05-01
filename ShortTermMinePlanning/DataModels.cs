using System;
using System.Collections.ObjectModel;

namespace ShortTermMinePlanning
{
    public enum TonnageUnit
    {
        تن,
        هزارتن,
        میلیون‌تن
    }

    public class TimePeriod
    {
        public int Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public double ExtractionCapacity { get; set; }
        public TonnageUnit ExtractionCapacityUnit { get; set; }
        public double TransportationCapacity { get; set; }
        public TonnageUnit TransportationCapacityUnit { get; set; }
        public int MaxExtractableBlocks { get; set; }

        public double ExtractionCapacityInTons
        {
            get
            {
                return ExtractionCapacityUnit switch
                {
                    TonnageUnit.تن => ExtractionCapacity,
                    TonnageUnit.هزارتن => ExtractionCapacity * 1000,
                    TonnageUnit.میلیون‌تن => ExtractionCapacity * 1000000,
                    _ => ExtractionCapacity
                };
            }
        }

        public double TransportationCapacityInTons
        {
            get
            {
                return TransportationCapacityUnit switch
                {
                    TonnageUnit.تن => TransportationCapacity,
                    TonnageUnit.هزارتن => TransportationCapacity * 1000,
                    TonnageUnit.میلیون‌تن => TransportationCapacity * 1000000,
                    _ => TransportationCapacity
                };
            }
        }
    }

    public class ExtractionBlock
    {
        public int Id { get; set; }
        public double Tonnage { get; set; }
        public TonnageUnit TonnageUnit { get; set; }
        public double ExtractionPercent { get; set; }
        public double Grade { get; set; }
        public double ExtractionCost { get; set; }

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

        public double ExtractableTonnage => TonnageInTons * (ExtractionPercent / 100);
    }

    public class Destination
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public double TransportationCost { get; set; }
        public double RecoveryRate { get; set; }
        public double MinAcceptableGrade { get; set; }
        public double MaxAcceptableGrade { get; set; }
        public double DistanceFromMine { get; set; }

        public double NetIncome => Price - TransportationCost;
    }

    public class Fleet
    {
        public int Id { get; set; }
        public int NumberOfTrucks { get; set; }
        public double TruckCapacity { get; set; }
        public TonnageUnit TruckCapacityUnit { get; set; }
        public double HaulCost { get; set; }
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

        public double TripCost => DistanceFromBlock * 2 * HaulCost;

        public double TotalHaulCost => TripCost * TripsPerPeriod * NumberOfTrucks;
    }

    public class DataStore
    {
        public ObservableCollection<TimePeriod> TimePeriods { get; set; } = new ObservableCollection<TimePeriod>();
        public ObservableCollection<ExtractionBlock> ExtractionBlocks { get; set; } = new ObservableCollection<ExtractionBlock>();
        public ObservableCollection<Destination> Destinations { get; set; } = new ObservableCollection<Destination>();
        public ObservableCollection<Fleet> Fleets { get; set; } = new ObservableCollection<Fleet>();

        private int nextPeriodId = 1;
        private int nextBlockId = 1;
        private int nextDestinationId = 1;
        private int nextFleetId = 1;

        public void AddTimePeriod(TimePeriod period)
        {
            period.Id = nextPeriodId++;
            TimePeriods.Add(period);
        }

        public void UpdateTimePeriod(TimePeriod period)
        {
            var existing = TimePeriods.FirstOrDefault(p => p.Id == period.Id);
            if (existing != null)
            {
                var index = TimePeriods.IndexOf(existing);
                TimePeriods[index] = period;
            }
        }

        public void DeleteTimePeriod(TimePeriod period)
        {
            TimePeriods.Remove(period);
        }

        public void AddExtractionBlock(ExtractionBlock block)
        {
            block.Id = nextBlockId++;
            ExtractionBlocks.Add(block);
        }

        public void UpdateExtractionBlock(ExtractionBlock block)
        {
            var existing = ExtractionBlocks.FirstOrDefault(b => b.Id == block.Id);
            if (existing != null)
            {
                var index = ExtractionBlocks.IndexOf(existing);
                ExtractionBlocks[index] = block;
            }
        }

        public void DeleteExtractionBlock(ExtractionBlock block)
        {
            ExtractionBlocks.Remove(block);
        }

        public void AddDestination(Destination destination)
        {
            destination.Id = nextDestinationId++;
            Destinations.Add(destination);
        }

        public void UpdateDestination(Destination destination)
        {
            var existing = Destinations.FirstOrDefault(d => d.Id == destination.Id);
            if (existing != null)
            {
                var index = Destinations.IndexOf(existing);
                Destinations[index] = destination;
            }
        }

        public void DeleteDestination(Destination destination)
        {
            Destinations.Remove(destination);
        }

        public void AddFleet(Fleet fleet)
        {
            fleet.Id = nextFleetId++;
            Fleets.Add(fleet);
        }

        public void UpdateFleet(Fleet fleet)
        {
            var existing = Fleets.FirstOrDefault(f => f.Id == fleet.Id);
            if (existing != null)
            {
                var index = Fleets.IndexOf(existing);
                Fleets[index] = fleet;
            }
        }

        public void DeleteFleet(Fleet fleet)
        {
            Fleets.Remove(fleet);
        }
    }
}