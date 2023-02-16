// Code written by Gabriel Mailhot, 02/12/2023.

#region

using System;
using TalesContract;
using TalesEnums;

#endregion

namespace TalesBase.Stories
{
    #region

    #endregion

    public class StoryHeader : IStoryHeader
    {
        public bool CanBePlayedOnlyOnce { get; set; }
        public string DependOn { get; set; }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public GameTime Time { get; set; } = GameTime.None;
        public StoryType TypeOfStory { get; set; } = StoryType.None;
    }
}