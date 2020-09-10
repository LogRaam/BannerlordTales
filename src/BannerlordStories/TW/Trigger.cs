// Code written by Gabriel Mailhot, 30/08/2020.

#region

using TalesContract;

#endregion

namespace TalesEntities.TW
{
   #region

   #endregion

   public class Trigger : ITrigger
   {
      public int ChanceToTrigger { get; set; }

      public string Link { get; set; }
   }
}