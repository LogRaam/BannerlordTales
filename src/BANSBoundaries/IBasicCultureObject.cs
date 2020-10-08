// Code written by Gabriel Mailhot, 11/09/2020.

#region

using TalesEnums;

#endregion

namespace TalesContract
{
   public interface IBasicCultureObject
   {
      public bool CanHaveSettlement { get; set; }

      public CultureCode CultureCode { get; set; }

      public bool IsBandit { get; set; }

      public bool IsMainCulture { get; set; }

      public string Name { get; set; }
   }
}