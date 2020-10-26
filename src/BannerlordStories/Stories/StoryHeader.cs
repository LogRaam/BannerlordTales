// Code written by Gabriel Mailhot, 11/09/2020.

#region

using System;
using TalesContract;
using TalesEnums;

#endregion

namespace TalesEntities.Stories
{
    #region

    #endregion

    public class StoryHeader : IStoryHeader
    {
        public bool CanBePlayedOnlyOnce { get; set; }
        public string DependOn { get; set; }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public GameTime Time { get; set; } = GameTime.NONE;
        public StoryType TypeOfStory { get; set; } = StoryType.NONE;
    }
}