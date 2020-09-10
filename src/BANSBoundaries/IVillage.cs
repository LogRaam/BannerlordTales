// Code written by Gabriel Mailhot, 01/09/2020.

namespace TalesContract
{
   #region

   using System.Collections.Generic;

   #endregion

   public interface IVillage
   {
      public IList<IVillage> All { get; }

      public ISettlement Bound { get; set; }

      public float Hearth { get; set; }

      public float HearthChange { get; }

      public bool IsDeserted { get; }

      public float LastDemandSatisfiedTime { get; set; }

      public ITown MarketTown { get; }

      public float Militia { get; }

      public float MilitiaChange { get; }

      public int TradeTaxAccumulated { get; set; }

      public IList<IMobileParty> VillagerPartiesAll { get; set; }

      public IMobileParty VillagerParty { get; set; }

      public IVillageType VillageType { get; set; }

      public int HearthLevel();
   }
}