// Code written by Gabriel Mailhot, 11/09/2020.

//BUG: I may not need DTO after all.  Persistence implement directly TaleWorlds classes.

#region

using System;
using System.Collections.Generic;
using System.Linq;
using TalesContract;
using TalesEntities.TW;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

#endregion

namespace TalesMapper
{
   #region

   #endregion

   public class DTO
   {
      public IHero Map(Hero hero)
      {
         BaseHero result = new BaseHero {
            Name = hero.Name.ToString(),
            Age = hero.Age,
            IsPrisoner = hero.IsPrisoner,
            IsPregnant = hero.IsPregnant,
            IsHumanPlayerCharacter = hero.IsHumanPlayerCharacter,
            IsDead = hero.IsDead,
            IsFertile = hero.IsFertile,
            IsFemale = hero.IsFemale,
            IsNoble = hero.IsNoble,
            IsFugitive = hero.IsFugitive,
            IsReleased = hero.IsReleased,
            IsAlive = hero.IsAlive,
            IsWounded = hero.IsWounded,
            IsWanderer = hero.IsWanderer,
            IsPlayerCompanion = hero.IsPlayerCompanion,
            IsMerchant = hero.IsMerchant,
            IsPreacher = hero.IsPreacher,
            IsHeadman = hero.IsHeadman,
            IsGangLeader = hero.IsGangLeader,
            IsArtisan = hero.IsArtisan,
            IsRuralNotable = hero.IsRuralNotable,
            IsOutlaw = hero.IsOutlaw,
            IsSpecial = hero.IsSpecial,
            IsRebel = hero.IsRebel,
            IsCommander = hero.IsCommander,
            IsPartyLeader = hero.IsPartyLeader,
            IsNotable = hero.IsNotable,
            HitPoints = hero.HitPoints,
            IsFactionLeader = hero.IsFactionLeader,
            Controversy = hero.Controversy,
            HasMet = hero.HasMet,
            Gold = hero.Gold,
            IsOccupiedByAnEvent = hero.IsOccupiedByAnEvent(),
            IsHealthFull = hero.IsHealthFull(),
            Children = hero.Children.Select(Map) as IList<IHero>
         };

         return result;
      }

      public ITroopRoster Map(TroopRoster mainPartyPrisonRoster)
      {
         BaseTroopRoster result = new BaseTroopRoster {
            Count = mainPartyPrisonRoster.Count,
            IsPrisonRoster = mainPartyPrisonRoster.IsPrisonRoster,
            TotalHealthyCount = mainPartyPrisonRoster.TotalHealthyCount,
            TotalHeroes = mainPartyPrisonRoster.TotalHeroes,
            TotalManCount = mainPartyPrisonRoster.TotalManCount,
            TotalRegulars = mainPartyPrisonRoster.TotalRegulars,
            TotalWounded = mainPartyPrisonRoster.TotalWounded,
            TotalWoundedHeroes = mainPartyPrisonRoster.TotalWoundedHeroes,
            TotalWoundedRegulars = mainPartyPrisonRoster.TotalWoundedRegulars,
            Troops = (IList<ICharacterObject>) mainPartyPrisonRoster.Troops.Select(Map)
         };

         return result;
      }


      public IBasicCharacterObject Map(BasicCharacterObject character)
      {
         throw new NotImplementedException();
      }

      public IPartyBase Map(PartyBase mainPartyPrisonRoster)
      {
         throw new NotImplementedException();
      }

      #region private

      private ICharacterObject Map(CharacterObject prisoner)
      {
         BaseCharacterObject result = new BaseCharacterObject {
            HeroObject = Map(prisoner.HeroObject)
         };

         return result;
      }

      #endregion
   }
}