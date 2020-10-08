// Code written by Gabriel Mailhot, 27/09/2020.

#region

using TalesContract;

#endregion

namespace TalesPersistence.Events
{
    public class HourlyEvent
    {
        public IAct Execute()
        {
            //TODO: Qualify act to play
            var act = GameData.Instance.GameContext.RetrieveActToPlay();
            GameData.Instance.GameContext.EventChanceBonus++;

            return act;
        }
    }
}