// Code written by Gabriel Mailhot, 11/09/2020.

#region

using System.Collections.Generic;
using TalesContract;

#endregion

namespace TalesEntities.Stories
{
    #region

    #endregion

    public class BaseChoice : IChoice
    {
        public List<IEvaluation> Conditions { get; set; } = new List<IEvaluation>();

        public List<IEvaluation> Consequences { get; set; } = new List<IEvaluation>();

        public string Id { get; set; }

        public string Text { get; set; }

        public List<ITrigger> Triggers { get; set; } = new List<ITrigger>();
    }
}