// Code written by Gabriel Mailhot, 11/09/2020.

#region

using TalesContract;

#endregion

namespace TalesEntities
{
    #region

    #endregion

    public class PregnancyEvent
    {
        public void StartPregnancy(IHero hero)
        {
            // TODO: Find father
            // TODO: Set pregnancy duration to RND{260, ..., 285} days
            // TODO: Start pregnancy
        }

        public void StopPregnancy(IHero hero)
        {
            // TODO: In case hero is pregnant, we must abort pregnancy
        }
    }
}