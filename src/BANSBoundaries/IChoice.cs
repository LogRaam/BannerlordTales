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
        List<IEvaluation> Conditions { get; }

        List<IEvaluation> Consequences { get; }

        public string Id { get; }

        string Text { get; set; }

        List<ITrigger> Triggers { get; }
    }
}