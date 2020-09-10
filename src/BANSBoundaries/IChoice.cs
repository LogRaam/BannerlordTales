// Code written by Gabriel Mailhot, 03/08/2020.

namespace TalesContract
{
   #region

   using System.Collections.Generic;

   #endregion

   public interface IChoice
   {
      List<IEvaluation> Conditions { get; }

      List<IEvaluation> Consequences { get; }

      string Text { get; set; }

      List<ITrigger> Triggers { get; }

      public string Id { get; set; }
   }
}