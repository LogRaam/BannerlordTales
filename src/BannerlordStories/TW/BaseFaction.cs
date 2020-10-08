// Code written by Gabriel Mailhot, 15/09/2020.

#region

using System;
using System.Collections.Generic;
using TalesContract;

#endregion

namespace TalesEntities.TW
{
    public class BaseFaction : IFaction
    {
        public BaseFaction(TaleWorlds.CampaignSystem.IFaction originMapFaction)
        {
            throw new NotImplementedException();
        }

        public BaseFaction()
        {
        }

        public float Aggressiveness { get; }
        public IEnumerable<IMobileParty> AllParties { get; }
        public ICharacterObject BasicTroop { get; }
        public ICultureObject Culture { get; }
        public float DailyCrimeRatingChange { get; }
        public string EncyclopediaLink { get; }
        public string EncyclopediaLinkWithName { get; }
        public string EncyclopediaText { get; }
        public IEnumerable<ITown> Fiefs { get; }
        public IEnumerable<IHero> Heroes { get; }
        public string InformalName { get; }
        public bool IsBanditFaction { get; }
        public bool IsClan { get; }
        public bool IsEliminated { get; }
        public bool IsKingdomFaction { get; }
        public bool IsMapFaction { get; }
        public bool IsMinorFaction { get; }
        public bool IsOutlaw { get; }
        public IHero Leader { get; }
        public IEnumerable<IHero> Lords { get; }
        public float MainHeroCrimeRating { get; set; }
        public IFaction MapFaction { get; }
        public string Name { get; }
        public ICampaignTime NotAttackableByPlayerUntilTime { get; set; }
        public IEnumerable<ISettlement> Settlements { get; }
        public string StringId { get; }
        public float TotalStrength { get; }
        public int TributeWallet { get; set; }
        public IEnumerable<IMobileParty> WarParties { get; }

        public bool IsAtWarWith(IFaction other)
        {
            throw new NotImplementedException();
        }
    }
}