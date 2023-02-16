// Code written by Gabriel Mailhot, 02/12/2023.

#region

using TalesContract;
using TalesEnums;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

#endregion

namespace TalesBase.TW
{
    public class BaseBasicCharacterObject : IBasicCharacterObject
    {
        public BaseBasicCharacterObject() { }

        public BaseBasicCharacterObject(BasicCharacterObject character)
        {
            if (character == null) return;

            Age = character.Age;
            Culture = new BaseBasicCultureObject(character.Culture);
            HasMount = character.HasMount();
            HitPoints = character.HitPoints;
            IsFemale = character.IsFemale;
            IsHero = character.IsHero;
            IsPlayerCharacter = character.IsPlayerCharacter;
            IsSoldier = character.IsSoldier;
            Level = character.Level;
            Name = character.Name.ToString();
            var h = Hero.FindFirst(n => n.Name == character.Name && n.Culture == character.Culture && n.IsHumanPlayerCharacter == character.IsPlayerCharacter);
            Vigor = h.GetAttributeValue(new CharacterAttribute(CharacterAttributesEnum.Vigor.ToString()));
            Control = h.GetAttributeValue(new CharacterAttribute(CharacterAttributesEnum.Control.ToString()));
            Endurance = h.GetAttributeValue(new CharacterAttribute(CharacterAttributesEnum.Endurance.ToString()));
            Cunning = h.GetAttributeValue(new CharacterAttribute(CharacterAttributesEnum.Cunning.ToString()));
            Social = h.GetAttributeValue(new CharacterAttribute(CharacterAttributesEnum.Social.ToString()));
            Intelligence = h.GetAttributeValue(new CharacterAttribute(CharacterAttributesEnum.Intelligence.ToString()));
        }

        public float Age { get; set; }
        public int Control { get; set; }
        public IBasicCultureObject Culture { get; set; }
        public int Cunning { get; set; }
        public int Endurance { get; set; }
        public bool HasMount { get; set; }
        public int HitPoints { get; set; }
        public int Intelligence { get; set; }
        public bool IsFemale { get; set; }
        public bool IsHero { get; set; }
        public bool IsPlayerCharacter { get; set; }
        public bool IsSoldier { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public int Social { get; set; }
        public int Vigor { get; set; }
    }
}