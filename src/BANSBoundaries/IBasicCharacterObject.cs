// Code written by Gabriel Mailhot, 01/09/2020.

namespace TalesContract
{
   public interface IBasicCharacterObject
   {
      public float Age { get; set; }

      public IBasicCultureObject Culture { get; set; }

      public bool HasMount { get; set; }

      public int HitPoints { get; set; }

      public bool IsFemale { get; set; }

      public bool IsHero { get; set; }

      public bool IsPlayerCharacter { get; set; }

      public bool IsSoldier { get; set; }

      public int Level { get; set; }

      public string Name { get; set; }

      public ISkill Skill { get; set; }
   }
}