// Code written by Gabriel Mailhot, 02/12/2023.

#region

using _45_TalesGameState;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

#endregion

namespace TalesPersistence.Context
{
    public class LocationContext
    {
        private bool? _isCurrentlyInSettlement;
        private bool? _isCurrentlyOnMap;

        public bool? IsCurrentlyInCastle { get; set; }
        public bool? IsCurrentlyInDungeon { get; set; }
        public bool? IsCurrentlyInFortification { get; set; }
        public bool? IsCurrentlyInHideout { get; set; }


        public bool? IsCurrentlyInSettlement
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isCurrentlyInSettlement = PartyBase.MainParty.IsSettlement;

                return _isCurrentlyInSettlement;
            }

            set => _isCurrentlyInSettlement = value != null && (bool)value;
        }

        public bool? IsCurrentlyInTown { get; set; }
        public bool? IsCurrentlyInVillage { get; set; }

        public bool? IsCurrentlyOnMap
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isCurrentlyOnMap = Game.Current.GameStateManager.ActiveState is MapState;

                return _isCurrentlyOnMap;
            }

            set => _isCurrentlyOnMap = value;
        }
    }
}