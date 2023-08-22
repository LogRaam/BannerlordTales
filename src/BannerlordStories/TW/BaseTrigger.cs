// Code written by Gabriel Mailhot, 02/12/2023.  Updated by  Gabriel Mailhot on 02/19/2023.

#region

using FluentAssertions.Execution;
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

        public bool IsEquivalentTo(ITrigger trigger)
        {
            if (trigger.Link != Link) throw new AssertionFailedException("Evaluation expected trigger.Link to be equivalent to " + trigger.Link + ", but found that its value is " + Link);
            if (trigger.ChanceToTrigger != ChanceToTrigger) throw new AssertionFailedException("Evaluation expected trigger.ChanceToTrigger to be equivalent to " + trigger.ChanceToTrigger + ", but found that its value is " + ChanceToTrigger);

            return true;
        }
    }
}