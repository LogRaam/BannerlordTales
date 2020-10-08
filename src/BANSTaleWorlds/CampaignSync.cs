// Code written by Gabriel Mailhot, 11/09/2020.

#region

using TalesPersistence;
using TaleWorlds.CampaignSystem;

#endregion

namespace TalesTaleWorlds
{
    #region

    #endregion

    internal class CampaignSync : CampaignBehaviorBase
    {
        private string backgroundMeshToSwitchBack = MenuState.BackgroundMeshToSwitchBack;

        private string menuToSwitchBack = MenuState.MenuToSwitchBack;

        public override void RegisterEvents()
        {
            // Do nothing.
        }

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData(MenuState.MenuSwitchBackUid, ref MenuState.MenuToSwitchBack);
            dataStore.SyncData(MenuState.BackgroundMeshSwitchBackUid, ref MenuState.BackgroundMeshToSwitchBack);
        }
    }
}