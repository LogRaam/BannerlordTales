// Code written by Gabriel Mailhot, 11/09/2020.

#region

using System.Collections.Generic;

#endregion

namespace TalesContract
{
    #region

    #endregion

    public interface IVillage
    {
        public IList<IVillage> All { get; }

        public ISettlement Bound { get; set; }

        public float Hearth { get; set; }

        public float HearthChange { get; }

        public int HearthLevel { get; set; }

        public bool IsDeserted { get; }

        public float LastDemandSatisfiedTime { get; set; }

        public ITown MarketTown { get; }

        public float Militia { get; }

        public float MilitiaChange { get; }

        public int TradeTaxAccumulated { get; set; }

        public IList<IMobileParty> VillagerPartiesAll { get; set; }

        public IMobileParty VillagerParty { get; set; }

        public IVillageType VillageType { get; set; }
    }
}