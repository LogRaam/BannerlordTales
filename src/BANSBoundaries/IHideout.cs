// Code written by Gabriel Mailhot, 02/09/2020.

namespace TalesContract
{
   public interface IHideout
   {
      public bool IsHideout { get; set; }

      public bool IsInfested { get; set; }

      public bool IsNextPossibleAttackTimeValid { get; set; }

      public bool IsSpotted { get; set; }

      public IFaction MapFaction { get; set; }

      public ICampaignTime NextPossibleAttackTime { get; set; }

      public string SceneName { get; set; }
   }
}