// Code written by Gabriel Mailhot, 02/09/2020.

namespace TalesContract
{
   #region

   using System.Collections.Generic;

   #endregion

   public interface IKingdom
   {
      public IList<IPolicyObject> ActivePolicies { get; set; }

      public float Aggressiveness { get; set; }

      public IList<IKingdom> All { get; set; }

      public IList<IMobileParty> AllParties { get; set; }

      public IReadOnlyList<IArmy> Armies { get; set; }

      public ICharacterObject BasicTroop { get; set; }

      public IReadOnlyList<IClan> Clans { get; set; }

      public ICultureObject Culture { get; set; }

      public float DailyCrimeRatingChange { get; set; }

      public string EncyclopediaLink { get; set; }

      public string EncyclopediaLinkWithName { get; set; }

      public string EncyclopediaRulerTitle { get; set; }

      public string EncyclopediaText { get; set; }

      public string EncyclopediaTitle { get; set; }

      public IList<ITown> Fiefs { get; set; }

      public IList<IHero> Heroes { get; set; }

      public string InformalName { get; set; }

      public bool IsBanditFaction { get; set; }

      public bool IsClan { get; set; }

      public bool IsEliminated { get; set; }

      public bool IsKingdomFaction { get; set; }

      public bool IsMapFaction { get; set; }

      public bool IsMinorFaction { get; set; }

      public bool IsOutlaw { get; set; }

      public int LastArmyCreationDay { get; set; }

      public ICampaignTime LastKingdomDecisionConclusionDate { get; set; }

      public ICampaignTime LastMercenaryOfferTime { get; set; }

      public IHero Leader { get; set; }

      public IList<IHero> Lords { get; set; }

      public float MainHeroCrimeRating { get; set; }

      public IFaction MapFaction { get; set; }

      public int MercenaryWallet { get; set; }

      public string Name { get; set; }

      public ICampaignTime NotAttackableByPlayerUntilTime { get; set; }

      public int PoliticalStagnation { get; set; }

      public IList<IProvocation> Provocations { get; set; }

      public IHero Ruler { get; set; }

      public IClan RulingClan { get; set; }

      public IList<ISettlement> Settlements { get; set; }

      public float TotalStrength { get; set; }

      public int TributeWallet { get; set; }

      public IList<IVillage> Villages { get; set; }

      public IList<IMobileParty> WarParties { get; set; }
   }
}