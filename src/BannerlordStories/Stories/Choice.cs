// Code written by Gabriel Mailhot, 03/08/2020.

namespace TalesEntities.Stories
{
   using System.Collections.Generic;
   using TalesContract;

   #region

   #endregion

   public class Choice : IChoice
   {
      public List<IEvaluation> Conditions { get; } = new List<IEvaluation>();

      public List<IEvaluation> Consequences { get; } = new List<IEvaluation>();

      public string Text { get; set; }

      public List<ITrigger> Triggers { get; } = new List<ITrigger>();

      public string Id { get; set; }
   }
}