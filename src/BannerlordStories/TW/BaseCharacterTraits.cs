// Code written by Gabriel Mailhot, 15/09/2020.

#region

using System;
using TalesContract;
using TaleWorlds.CampaignSystem;

#endregion

namespace TalesEntities.TW
{
    public class BaseCharacterTraits : ICharacterTraits
    {
        public BaseCharacterTraits(CharacterTraits getHeroTraits)
        {
            throw new NotImplementedException();
        }

        public BaseCharacterTraits()
        {
        }

        public int Calculating { get; set; }
        public int Generosity { get; set; }
        public int Honor { get; set; }
        public int Mercy { get; set; }
        public int Valor { get; set; }
    }
}