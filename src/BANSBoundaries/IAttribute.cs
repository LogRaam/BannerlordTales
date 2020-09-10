// Code written by Gabriel Mailhot, 01/09/2020.

namespace TalesContract
{
   public interface IAttribute
   {
      public int Control { get; set; }

      public int Cunning { get; set; }

      public int Endurance { get; set; }

      public int Intelligence { get; set; }

      public int Social { get; set; }

      public int Vigor { get; set; }
   }
}