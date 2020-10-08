// Code written by Gabriel Mailhot, 14/09/2020.

#region

using TalesContract;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

#endregion

namespace TalesEntities.TW
{
    public class BaseBasicCharacterObject : IBasicCharacterObject
    {
        public BaseBasicCharacterObject()
        {
        }

        public BaseBasicCharacterObject(BasicCharacterObject character)
        {
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
            Vigor = h.GetAttributeValue(CharacterAttributesEnum.Vigor);
            Control = h.GetAttributeValue(CharacterAttributesEnum.Control);
            Endurance = h.GetAttributeValue(CharacterAttributesEnum.Endurance);
            Cunning = h.GetAttributeValue(CharacterAttributesEnum.Cunning);
            Social = h.GetAttributeValue(CharacterAttributesEnum.Social);
            Intelligence = h.GetAttributeValue(CharacterAttributesEnum.Intelligence);
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