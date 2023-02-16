// Code written by Gabriel Mailhot, 02/12/2023.

#region

using System.Collections.Generic;
using TalesContract;
using TalesEnums;

#endregion

namespace TalesBase.Stories
{
    #region

    #endregion

    public class BaseAct : IAct
    {
        public List<IChoice> Choices { get; set; } = new List<IChoice>();

        public string Id => ParentStory.Header.Name.Replace(" ", "") + "_" + Name.Replace(" ", "");

        public string Image { get; set; }

        public string Intro { get; set; }

        public Location Location { get; set; } = Location.Unknown;

        public string Name { get; set; }

        public IStory ParentStory { get; set; }

        public List<IEvaluation> Restrictions { get; set; } = new List<IEvaluation>();
    }
}