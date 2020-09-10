// Code written by Gabriel Mailhot, 02/09/2020.

#region

using System;
using System.Collections.Generic;
using TalesContract;

#endregion

namespace TalesEntities.Stories
{
   #region

   #endregion

   public class BaseStory : IStory
   {
      public List<IAct> Acts { get; set; } = new List<IAct>();

      public IStoryHeader Header { get; set; } = new StoryHeader();

      public string Id { get; set; } = Guid.NewGuid().ToString();


      public List<IEvaluation> Restrictions { get; set; } = new List<IEvaluation>();
   }
}