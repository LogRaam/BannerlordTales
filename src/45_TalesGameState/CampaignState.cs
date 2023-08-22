// Code written by Gabriel Mailhot, 02/12/2023.

#region

using TaleWorlds.CampaignSystem;

#endregion

namespace _45_TalesGameState
{
    public static class CampaignState
    {
        public static bool CurrentGameStarted()
        {
            return Campaign.Current != null && (Campaign.Current.GameStarted || Campaign.Current.CampaignGameLoadingType == Campaign.GameLoadingType.NewCampaign);
        }
    }
}