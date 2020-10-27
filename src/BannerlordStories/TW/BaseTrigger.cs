// Code written by Gabriel Mailhot, 11/09/2020.

#region

using TalesContract;

#endregion

namespace TalesBase.TW
{
    #region

    #endregion

    public class BaseTrigger : ITrigger
    {
        public int ChanceToTrigger { get; set; }

        public string Link { get; set; }
    }
}