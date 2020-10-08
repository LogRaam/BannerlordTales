// Code written by Gabriel Mailhot, 11/09/2020.

#region

using TalesContract;

#endregion

namespace TalesEntities
{
    #region

    #endregion

    public class EventContext : IEventContext
    {
        public IHero Player { get; set; }
    }
}