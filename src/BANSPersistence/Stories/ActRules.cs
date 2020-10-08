// Code written by Gabriel Mailhot, 27/09/2020.

#region

using TalesContract;

#endregion

namespace TalesPersistence.Stories
{
    public class ActRules
    {
        public ActRules(Act act)
        {
            Act = act;
        }


        private IAct Act { get; }
    }
}