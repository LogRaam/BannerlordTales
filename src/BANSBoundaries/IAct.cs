// Code written by Gabriel Mailhot, 02/12/2023.  Updated by  Gabriel Mailhot on 02/19/2023.

#region

using System.Collections.Generic;
using TalesEnums;

#endregion

namespace TalesContract
{
    public interface IAct
    {
        List<IChoice> Choices { get; }

        public string Id { get; }

        string Image { get; set; }

        string Intro { get; set; }

        Location Location { get; set; }

        string Name { get; set; }
        public IStory ParentStory { get; set; }

        public List<IEvaluation> Restrictions { get; set; }
        bool IsEquivalentTo(IAct act);
    }
}