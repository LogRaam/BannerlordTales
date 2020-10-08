// Code written by Gabriel Mailhot, 14/09/2020.

#region

using System;
using TalesContract;
using TaleWorlds.Core;
using CultureCode = TalesEnums.CultureCode;

#endregion

namespace TalesEntities.TW
{
    public class BaseBasicCultureObject : IBasicCultureObject
    {
        public BaseBasicCultureObject()
        {
        }

        public BaseBasicCultureObject(BasicCultureObject culture)
        {
            CanHaveSettlement = culture.CanHaveSettlement;

            Enum.TryParse(culture.GetCultureCode().ToString(), true, out CultureCode p);
            CultureCode = p;

            IsBandit = culture.IsBandit;
            IsMainCulture = culture.IsMainCulture;
            Name = culture.Name.ToString();
        }

        public bool CanHaveSettlement { get; set; }
        public CultureCode CultureCode { get; set; }
        public bool IsBandit { get; set; }
        public bool IsMainCulture { get; set; }
        public string Name { get; set; }
    }
}