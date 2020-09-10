// Code written by Gabriel Mailhot, 30/08/2020.

namespace TalesEntities.TW
{
   using System.Collections.Generic;
   using TalesContract;

   public class TroopRoster : ITroopRoster
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