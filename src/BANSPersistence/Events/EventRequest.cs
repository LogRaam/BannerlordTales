// Code written by Gabriel Mailhot, 27/09/2020.

#region

using TalesContract;
using TalesEnums;

#endregion

namespace TalesPersistence.Events
{
    #region

    #endregion

    public class EventRequest : IEventRequest
    {
        public StoryAction Action { get; set; }

        public StoryType StoryType { get; set; }
    }
}