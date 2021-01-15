// unset

#region

using _45_TalesGameState;
using TalesBase.TW;
using TalesContract;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using Hero = TalesPersistence.Entities.Hero;

#endregion

namespace TalesPersistence.Context
{
    public class HeroContext
    {
        private IHero _captor;

        private IHero _player;
        private bool? _playerIsCaptor;

        public IHero Captor
        {
            get
            {
                if (CampaignState.CurrentGameStarted())
                    if (TaleWorlds.CampaignSystem.Hero.MainHero.IsPrisoner)
                        _captor = new BaseHero(Campaign.Current.MainParty.LeaderHero);

                return _captor;
            }

            set => _captor = value;
        }

        public IHero Player
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _player = new BaseHero(Game.Current.PlayerTroop);

                return _player;
            }
            set => _player = value;
        }


        public bool? PlayerIsCaptor
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _playerIsCaptor = Campaign.Current.MainParty.LeaderHero.IsHumanPlayerCharacter && Campaign.Current.MainParty.PrisonRoster.Count > 0;

                return _playerIsCaptor;
            }

            set => _playerIsCaptor = value;
        }

        public void MakePregnant(Hero actor)
        {
            if (CampaignState.CurrentGameStarted())
            {
                MakePregnantAction.Apply(actor.ToTwHero());

                return;
            }

            Player.IsPregnant = true;
        }
    }
}