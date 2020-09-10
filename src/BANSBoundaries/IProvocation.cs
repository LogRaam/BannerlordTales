// Code written by Gabriel Mailhot, 02/09/2020.

namespace TalesContract
{
   #region

   using TalesEnums;

   #endregion

   public interface IProvocation
   {
      public IHero actor { get; set; }

      public ICampaignTime provocationTime { get; set; }

      public ProvocationType provocationType { get; set; }

      public IKingdom provocatorFaction { get; set; }
   }
}