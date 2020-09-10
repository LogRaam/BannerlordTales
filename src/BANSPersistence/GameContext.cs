// Code written by Gabriel Mailhot, 02/09/2020.

#region

using TalesContract;
using TalesMapper;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

#endregion

namespace TalesPersistence
{
   #region

   #endregion

   public class GameContext
   {
      private IBasicCharacterObject _player;
      private bool? isCurrentlyInSettlement;
      private bool? isCurrentlyOnMap;
      private bool isDay;
      private bool isNight;
      private bool? playerIsCaptive;
      private bool? playerIsCaptor;

      public bool? IsCurrentlyInCastle { get; set; }
      public bool? IsCurrentlyInDungeon { get; set; }
      public bool? IsCurrentlyInFortification { get; set; }
      public bool? IsCurrentlyInHideout { get; set; }

      public bool? IsCurrentlyInSettlement
      {
         get
         {
            if (CurrentGameStarted())
            {
               isCurrentlyInSettlement = PartyBase.MainParty.IsSettlement;
            }

            return isCurrentlyInSettlement;
         }

         set => isCurrentlyInSettlement = value != null && (bool) value;
      }

      public bool? IsCurrentlyInTown { get; set; }
      public bool? IsCurrentlyInVillage { get; set; }

      public bool? IsCurrentlyOnMap
      {
         get
         {
            if (CurrentGameStarted())
            {
               isCurrentlyOnMap = Game.Current.GameStateManager.ActiveState is MapState;
            }

            return isCurrentlyOnMap;
         }

         set => isCurrentlyOnMap = value;
      }

      public bool IsDay
      {
         get
         {
            if (CurrentGameStarted())
            {
               isDay = Campaign.Current.IsDay;
               isNight = !isDay;
            }

            return isDay;
         }

         set => isDay = value;
      }

      public bool IsNight
      {
         get
         {
            if (CurrentGameStarted())
            {
               isNight = Campaign.Current.IsDay;
               isDay = !isNight;
            }

            return isDay;
         }

         set => isDay = value;
      }

      public IBasicCharacterObject Npc { get; set; } //TODO: must return Npc in Event (Captor, Captive, Dialogue?)

      public IBasicCharacterObject Player
      {
         get
         {
            if (CurrentGameStarted())
            {
               _player = new DTO().Map(Game.Current.PlayerTroop);
            }

            return _player;
         }
         set => _player = value;
      }


      public bool? PlayerIsCaptive
      {
         get
         {
            if (CurrentGameStarted())
            {
               playerIsCaptive = Hero.MainHero.IsPrisoner;
            }

            return playerIsCaptive;
         }

         set => playerIsCaptive = value;
      }

      public bool? PlayerIsCaptor
      {
         get
         {
            if (CurrentGameStarted())
            {
               playerIsCaptor = Campaign.Current.MainParty.LeaderHero.IsHumanPlayerCharacter && Campaign.Current.MainParty.PrisonRoster.Count > 0;
            }

            return playerIsCaptor;
         }

         set => playerIsCaptor = value;
      }

      internal bool CurrentGameStarted()
      {
         return Campaign.Current != null && Campaign.Current.GameStarted;
      }
   }
}