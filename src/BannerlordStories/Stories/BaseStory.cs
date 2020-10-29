// Code written by Gabriel Mailhot, 11/09/2020.

#region

using System.Collections.Generic;
using TalesContract;

#endregion

namespace TalesBase.Stories
{
    #region

    #endregion

    public class BaseStory : IStory
    {
        public List<IAct> Acts { get; set; } = new List<IAct>();

        public IStoryHeader Header { get; set; } = new StoryHeader();

        public string Id => Header.Name.Replace(" ", "");


        public List<IEvaluation> Restrictions { get; set; } = new List<IEvaluation>();

        public List<ISequence> Sequences { get; set; } = new List<ISequence>();
    }
}