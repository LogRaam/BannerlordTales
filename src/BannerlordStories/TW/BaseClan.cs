// Code written by Gabriel Mailhot, 15/09/2020.

#region

using System;
using System.Collections.Generic;
using TalesContract;
using TaleWorlds.CampaignSystem;

#endregion

namespace TalesEntities.TW
{
    public class BaseClan : IClan
    {
        public BaseClan(Clan originClan)
        {
            throw new NotImplementedException();
        }

        public BaseClan()
        {
        }

        public float Aggressiveness { get; set; }
        public IList<IClan> All { get; set; }
        public IList<IMobileParty> AllParties { get; set; }
        public IList<IClan> BanditFactions { get; set; }
        public ICharacterObject BasicTroop { get; set; }
        public int CommanderLimit { get; set; }
        public int CompanionLimit { get; set; }
        public IList<IHero> Companions { get; set; }
        public ICultureObject Culture { get; set; }
        public float DailyCrimeRatingChange { get; set; }
        public int DebtToKingdom { get; set; }
        public IList<ITown> Fiefs { get; set; }
        public IList<ITown> Fortifications { get; set; }
        public string FullName { get; set; }
        public int Gold { get; set; }
        public IList<IHero> Heroes { get; set; }
        public ISettlement HomeSettlement { get; set; }
        public float Influence { get; set; }
        public float InfluenceChange { get; set; }
        public string InformalName { get; set; }
        public bool IsBanditFaction { get; set; }
        public bool IsClan { get; set; }
        public bool IsClanTypeMercenary { get; set; }
        public bool IsEliminated { get; set; }
        public bool IsKingdomFaction { get; set; }
        public bool IsMafia { get; set; }
        public bool IsMapFaction { get; set; }
        public bool IsMinorFaction { get; set; }
        public bool IsNomad { get; set; }
        public bool IsOutlaw { get; set; }
        public bool IsRebelFaction { get; set; }
        public bool IsSect { get; set; }
        public bool IsUnderMercenaryService { get; set; }
        public IKingdom Kingdom { get; set; }
        public ICampaignTime LastFactionChangeTime { get; set; }
        public IHero Leader { get; set; }
        public IList<IHero> Lords { get; set; }
        public float MainHeroCrimeRating { get; set; }
        public int MercenaryAwardMultiplier { get; set; }
        public string Name { get; set; }
        public ICampaignTime NotAttackableByPlayerUntilTime { get; set; }
        public int NumFiefs { get; set; }
        public IClan PlayerClan { get; set; }
        public float Renown { get; set; }
        public int RenownRequirementForNextTier { get; set; }
        public IList<ISettlement> Settlements { get; set; }
        public IList<IHero> SupporterNotables { get; set; }
        public int Tier { get; set; }
        public float TotalStrength { get; set; }
        public int TributeWallet { get; set; }
        public IList<IVillage> Villages { get; set; }
        public IList<IMobileParty> WarParties { get; set; }
    }
}