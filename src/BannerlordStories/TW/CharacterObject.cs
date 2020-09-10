// Code written by Gabriel Mailhot, 30/08/2020.

namespace TalesEntities.TW
{
   #region

   using System.Collections.Generic;
   using TalesContract;
   using TalesEnums;

   #endregion

   public class CharacterObject : ICharacterObject
   {
      public List<ICharacterObject> All { get; set; } = new List<ICharacterObject>();

      public IList<CharacterObject> ChildTemplates { get; set; } = new List<CharacterObject>();

      public IList<ICharacterObject> ConversationCharacters { get; set; } = new List<ICharacterObject>();

      public ICharacterObject OneToOneConversationCharacter { get; set; }

      public ICharacterObject PlayerCharacter { get; set; }

      public IList<CharacterObject> Templates { get; set; } = new List<CharacterObject>();

      public float Age { get; set; }

      public string BeardTags { get; set; }

      public int ConformityNeededToRecruitPrisoner { get; set; }

      public ICultureObject Culture { get; set; }

      public string EncyclopediaLink { get; set; }

      public string EncyclopediaLinkWithName { get; set; }

      public string HairTags { get; set; }

      public IHero HeroObject { get; set; }

      public int HitPoints { get; set; }

      public bool IsArcher { get; set; }

      public bool IsBasicTroop { get; set; }

      public bool IsChildTemplate { get; set; }

      public bool IsFemale { get; set; }

      public bool IsHero { get; set; }

      public bool IsInfantry { get; set; }

      public bool IsMounted { get; set; }

      public bool IsNotTransferable { get; set; }

      public bool IsOriginalCharacter { get; set; }

      public bool IsPlayerCharacter { get; set; }

      public bool IsRegular { get; set; }

      public bool IsTemplate { get; set; }

      public int Level { get; set; }

      public string Name { get; set; }

      public Occupation Occupation { get; set; }

      public string TattooTags { get; set; }

      public ICharacterObject TemplateCharacter { get; set; }

      public int Tier { get; set; }

      public int TroopWage { get; set; }

      public ICharacterObject[] UpgradeTargets { get; set; }

      public int UpgradeXpCost { get; set; }
   }
}