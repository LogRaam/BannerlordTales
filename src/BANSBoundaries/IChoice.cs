// Code written by Gabriel Mailhot, 02/12/2023.  Updated by  Gabriel Mailhot on 02/19/2023.

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
        bool IsEquivalentTo(IChoice choice);
    }
}