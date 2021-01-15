// unset

#region

using System;
using TalesDAL;

#endregion

namespace TalesPersistence.Context
{
    public class EventContext
    {
        private int EventChanceBonus { get; set; }

        public bool ReadyToShowNewEvent()
        {
            var f = EventChanceBonus + new TimeContext().HourOfDay;
            TalesRandom.InitRandomNumber(Guid.NewGuid().GetHashCode());

            var diceRoll = TalesRandom.GenerateRandomNumber(100 + f);
            var result = diceRoll > 100;

            if (result) ResetEventChanceBonus();
            GameData.Instance.GameContext.Events.EventChanceBonus++;

            return result;
        }

        public void ResetEventChanceBonus()
        {
            EventChanceBonus = 0;
        }
    }
}