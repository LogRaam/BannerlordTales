// Code written by Gabriel Mailhot, 29/08/2020.

namespace BannerlordTales.Events
{
   #region

   using TalesContract;
   using TalesEnums;

   #endregion

   public class EventRequest : IEventRequest
   {
      public StoryAction Action { get; set; }

      public StoryType StoryType { get; set; }
   }
}