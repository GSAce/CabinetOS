namespace CabinetOS.Core.Arcade.DatDiscovery
{
    public class ArcadeDatFamilyInfo
    {
        public string Family { get; }
        public IReadOnlyList<string> DatPaths { get; }

        public ArcadeDatFamilyInfo(string family, IReadOnlyList<string> datPaths)
        {
            Family = family;
            DatPaths = datPaths;
        }
    }
}