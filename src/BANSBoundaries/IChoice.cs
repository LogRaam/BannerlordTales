// Code written by Gabriel Mailhot, 11/09/2020.

#region

using System.Collections.Generic;

#endregion

namespace TalesContract
{
    #region

    #endregion

    public interface IChoice
    {
        public List<IEvaluation> Conditions { get; set; }

        public List<IEvaluation> Consequences { get; set; }

        public string Id { get; }

        public IAct ParentAct { get; set; }

        public string Text { get; set; }

        public List<ITrigger> Triggers { get; set; }
    }
}