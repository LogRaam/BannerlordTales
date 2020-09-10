// Code written by Gabriel Mailhot, 01/09/2020.

namespace TalesContract
{
   #region

   using System.Collections.Generic;

   #endregion

   public interface IFaction
   {
      float Aggressiveness { get; }

      IEnumerable<IMobileParty> AllParties { get; }

      ICharacterObject BasicTroop { get; }

      ICultureObject Culture { get; }

      float DailyCrimeRatingChange { get; }

      string EncyclopediaLink { get; }

      string EncyclopediaLinkWithName { get; }

      string EncyclopediaText { get; }

      IEnumerable<ITown> Fiefs { get; }

      IEnumerable<IHero> Heroes { get; }

      string InformalName { get; }

      bool IsBanditFaction { get; }

      bool IsClan { get; }

      bool IsEliminated { get; }

      bool IsKingdomFaction { get; }

      bool IsMapFaction { get; }

      bool IsMinorFaction { get; }

      bool IsOutlaw { get; }

      IHero Leader { get; }

      IEnumerable<IHero> Lords { get; }

      float MainHeroCrimeRating { get; set; }

      IFaction MapFaction { get; }

      string Name { get; }

      ICampaignTime NotAttackableByPlayerUntilTime { get; set; }

      IEnumerable<ISettlement> Settlements { get; }

      string StringId { get; }

      float TotalStrength { get; }

      int TributeWallet { get; set; }

      IEnumerable<IMobileParty> WarParties { get; }

      bool IsAtWarWith(IFaction other);
   }
}