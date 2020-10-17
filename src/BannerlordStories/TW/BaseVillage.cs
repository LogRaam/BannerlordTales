// Code written by Gabriel Mailhot, 11/10/2020.

#region

using System.Collections.Generic;
using TalesContract;
using TaleWorlds.CampaignSystem;

#endregion

namespace TalesEntities.TW
{
    public class BaseVillage : IVillage
    {
        public BaseVillage(Village village)
        {
            //TODO
        }

        public BaseVillage() { }
        public IList<IVillage> All { get; set; }
        public ISettlement Bound { get; set; }
        public float Hearth { get; set; }
        public float HearthChange { get; set; }
        public int HearthLevel { get; set; }
        public bool IsDeserted { get; set; }
        public float LastDemandSatisfiedTime { get; set; }
        public ITown MarketTown { get; set; }
        public float Militia { get; set; }
        public float MilitiaChange { get; set; }
        public int TradeTaxAccumulated { get; set; }
        public IList<IMobileParty> VillagerPartiesAll { get; set; }
        public IMobileParty VillagerParty { get; set; }
        public IVillageType VillageType { get; set; }
    }
}