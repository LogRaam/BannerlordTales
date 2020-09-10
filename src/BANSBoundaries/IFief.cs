// Code written by Gabriel Mailhot, 01/09/2020.

namespace TalesContract
{
   public interface IFief
   {
      public float FoodStocks { get; set; }

      public IMobileParty GarrisonParty { get; set; }

      public float Militia { get; }

      public float Prosperity { get; }
   }
}