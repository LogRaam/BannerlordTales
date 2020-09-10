// Code written by Gabriel Mailhot, 31/08/2020.

#region

using TalesEnums;

#endregion

namespace TalesContract
{
   #region

   #endregion

   public interface IStoryHeader
   {
      bool CanBePlayedOnlyOnce { get; set; }

      string DependOn { get; set; }

      string Id { get; set; }
      string Name { get; set; }

      public GameTime Time { get; set; }

      StoryType TypeOfStory { get; set; }
   }
}