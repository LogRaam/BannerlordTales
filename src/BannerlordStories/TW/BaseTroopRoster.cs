// Code written by Gabriel Mailhot, 11/09/2020.

#region

using System.Collections.Generic;
using TalesContract;

#endregion

namespace TalesEntities.TW
{
    public class BaseTroopRoster : ITroopRoster
    {
        public int Count { get; set; }

        public bool IsPrisonRoster { get; set; }

        public int TotalHealthyCount { get; set; }

        public int TotalHeroes { get; set; }

        public int TotalManCount { get; set; }

        public int TotalRegulars { get; set; }

        public int TotalWounded { get; set; }

        public int TotalWoundedHeroes { get; set; }

        public int TotalWoundedRegulars { get; set; }

        public IList<ICharacterObject> Troops { get; set; } = new List<ICharacterObject>();
    }
}