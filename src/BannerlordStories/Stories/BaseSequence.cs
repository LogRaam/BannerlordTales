// Code written by Gabriel Mailhot, 11/09/2020.

#region

using System.Collections.Generic;
using TalesContract;
using TalesEnums;

#endregion

namespace TalesEntities.Stories
{
    public class BaseSequence : ISequence, IAct
    {
        public List<IChoice> Choices { get; set; } = new List<IChoice>();

        public string Id => ParentStory.Header.Name.Replace(" ", "") + "_" + Name.Replace(" ", "");

        public string Image { get; set; }
        public string Intro { get; set; }
        public Location Location { get; set; }
        public string Name { get; set; }
        public IStory ParentStory { get; set; }
        public List<IEvaluation> Restrictions { get; set; } = new List<IEvaluation>();
    }
}