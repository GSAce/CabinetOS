namespace CabinetOS.Core.Arcade.Models
{
    public class DatGameInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Parent { get; set; }
        public string? CloneOf { get; set; }
        public string? Year { get; set; }
        public string? Manufacturer { get; set; }

        public List<DatRomInfo> Roms { get; set; } = new();
        public List<DatDiskInfo> Disks { get; set; } = new();
        public List<string> Categories { get; set; } = new();

        // Filtering flags
        public bool IsMechanical { get; set; }
        public bool IsPrototype { get; set; }
        public bool IsGambling { get; set; }
        public bool IsFruitMachine { get; set; }
        public bool IsTicketRedemption { get; set; }
        public bool IsQuiz { get; set; }
        public bool IsBios { get; set; }
        public bool IsAdult { get; set; }
        public bool IsCasino { get; set; }
        public bool IsMahjong { get; set; }
        public bool IsNonWorking { get; set; }
        public bool IsWorking { get; set; }
    }
}