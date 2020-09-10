// Code written by Gabriel Mailhot, 12/08/2020.

namespace TalesEntities
{
   #region

   using TalesContract;

   #endregion

   public class EventContext : IEventContext
   {
      public IHero Player { get; set; }
   }
}