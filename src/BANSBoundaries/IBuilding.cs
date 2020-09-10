// Code written by Gabriel Mailhot, 01/09/2020.

namespace TalesContract
{
   public interface IBuilding
   {
      public float BuildingProgress { get; set; }

      public IBuildingType BuildingType { get; set; }

      public int CurrentLevel { get; set; }

      public string Explanation { get; }

      public bool IsCurrentlyDefault { get; set; }

      public ITown Town { get; set; }
   }
}