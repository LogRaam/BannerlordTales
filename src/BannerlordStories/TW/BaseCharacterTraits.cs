// Code written by Gabriel Mailhot, 02/12/2023.

#region

using TalesContract;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

#endregion

namespace TalesBase.TW
{
    public class BaseCharacterTraits : ICharacterTraits
    {
        public BaseCharacterTraits(CharacterTraits traits)
        {
            if (traits == null) return;

            Calculating = traits.Calculating;
            Generosity = traits.Generosity;
            Honor = traits.Honor;
            Mercy = traits.Mercy;
            Valor = traits.Valor;
        }

        public BaseCharacterTraits() { }

        public int Calculating { get; set; }
        public int Generosity { get; set; }
        public int Honor { get; set; }
        public int Mercy { get; set; }
        public int Valor { get; set; }
    }
}