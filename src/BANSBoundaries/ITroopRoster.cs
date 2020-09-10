namespace TalesContract
{
    public interface ITroopRoster
    {
        int Count { get; }
        bool IsPrisonRoster { get;  }
        int TotalHealthyCount { get; }
        int TotalHeroes { get; }
        int TotalManCount { get; }
        int TotalRegulars { get;}
        int TotalWounded { get; }
        int TotalWoundedHeroes { get; }
        int TotalWoundedRegulars { get; }
    }
}