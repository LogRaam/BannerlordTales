// Code written by Gabriel Mailhot, 01/09/2020.

namespace TalesContract
{
   using TalesEnums;

   public interface IBasicCultureObject
   {
      public bool CanHaveSettlement { get; }

      public bool IsBandit { get; }

      public bool IsMainCulture { get; }

      public string Name { get; }

      public CultureCode GetCultureCode();
   }
}