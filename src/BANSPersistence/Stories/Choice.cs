// Code written by Gabriel Mailhot, 03/10/2020.

#region

using TalesBase.Stories;
using TalesContract;

#endregion

namespace TalesPersistence.Stories
{
    public class Choice : BaseChoice
    {
        public Choice(IChoice baseChoice)
        {
            Text = baseChoice.Text;
            Conditions = baseChoice.Conditions;
            Consequences = baseChoice.Consequences;
            Triggers = baseChoice.Triggers;
        }

        public Choice()
        {
        }
    }
}