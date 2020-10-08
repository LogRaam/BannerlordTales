// Code written by Gabriel Mailhot, 27/09/2020.

#region

using TalesContract;
using TalesEnums;

#endregion

namespace TalesPersistence.Events
{
    #region

    #endregion

    public class DailyEvent
    {
        public IEventRequest Execute()
        {
            //// TODO: If pregnant, send pregnancy stage messages and manage toon body slide accordingly.
            //var hero = GamePersistence.Instance.PlayerState;

            if (GameData.Instance.GameContext.Player.IsPrisoner) return StartCaptiveStory();

            return null;
        }

        #region private

        private IEventRequest StartCaptiveStory()
        {
            return new EventRequest {StoryType = StoryType.PLAYER_IS_CAPTIVE, Action = StoryAction.START};
        }

        #endregion
    }
}