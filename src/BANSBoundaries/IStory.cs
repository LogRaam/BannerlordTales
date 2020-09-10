// Code written by Gabriel Mailhot, 03/08/2020.

#region

using System.Collections.Generic;

#endregion

namespace TalesContract
{
   #region

   #endregion

   public interface IStory
   {
      List<IAct> Acts { get; set; }

      IStoryHeader Header { get; set; }

      string Id { get; set; }

      List<IEvaluation> Restrictions { get; set; }
   }
}