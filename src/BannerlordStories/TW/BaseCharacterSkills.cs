// Code written by Gabriel Mailhot, 16/09/2020.

#region

using System;
using TalesContract;
using TaleWorlds.Core;

#endregion

namespace TalesEntities.TW
{
    public class BaseCharacterSkills : ICharacterSkills
    {
        public BaseCharacterSkills(CharacterSkills getHeroSkills)
        {
            throw new NotImplementedException();
        }

        public BaseCharacterSkills()
        {
        }

        public int ATHLETICS { get; set; }

        public int BOW { get; set; }

        public int CHARM { get; set; }

        public int CRAFTING { get; set; }

        public int CROSSBOW { get; set; }

        public int ENGINEERING { get; set; }

        public int LEADERSHIP { get; set; }

        public int MEDICINE { get; set; }

        public int ONEHANDED { get; set; }

        public int POLEARM { get; set; }

        public int RIDING { get; set; }

        public int ROGUERY { get; set; }

        public int SCOUTING { get; set; }

        public int STEWARD { get; set; }

        public int TACTICS { get; set; }

        public int THROWING { get; set; }

        public int TRADE { get; set; }

        public int TWOHANDED { get; set; }
    }
}