// Code written by Gabriel Mailhot, 02/12/2023.

#region

using TalesContract;
using CharacterSkills = TaleWorlds.Core.CharacterSkills;

#endregion

namespace TalesBase.TW
{
    #region

    #endregion

    public class BaseSkill : ISkill
    {
        public BaseSkill(CharacterSkills skills)
        {
            //TODO
        }

        public BaseSkill() { }

        public int Athletics { get; set; }

        public int Bow { get; set; }

        public int Charm { get; set; }

        public int Crafting { get; set; }

        public int Crossbow { get; set; }

        public int Engineering { get; set; }

        public int Leadership { get; set; }

        public int Medicine { get; set; }

        public int OneHanded { get; set; }

        public int Polearm { get; set; }

        public int Riding { get; set; }

        public int Roguery { get; set; }

        public int Scouting { get; set; }

        public int Steward { get; set; }

        public int Tactics { get; set; }

        public int Throwing { get; set; }

        public int Trade { get; set; }

        public int TwoHanded { get; set; }
    }
}