// Code written by Gabriel Mailhot, 11/09/2020.

#region

using System;
using System.Collections.Generic;
using System.Linq;
using _45_TalesGameState;
using TalesContract;
using TalesDAL;
using TalesEntities.TW;
using TalesEnums;
using TalesPersistence.Stories;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using Location = TalesEnums.Location;

#endregion

namespace TalesPersistence
{
    #region

    #endregion

    public class GameContext
    {
        private IHero _captor;
        private GameTime _gameTime;
        private int _hourOfDay;
        private int _hoursInDay;
        private bool? _isCurrentlyInSettlement;
        private bool? _isCurrentlyOnMap;
        private bool _isDay;
        private bool _isNight;
        private IHero _player;
        private bool? _playerIsCaptor;


        public IHero Captor
        {
            get
            {
                if (CampaignState.CurrentGameStarted())
                {
                    if (Hero.MainHero.IsPrisoner) _captor = new BaseHero(Campaign.Current.MainParty.LeaderHero);
                }

                return _captor;
            }

            set => _captor = value;
        }

        public int EventChanceBonus { get; set; }

        public GameTime GameTime
        {
            get
            {
                _gameTime = GameTime.ANYTIME;
                if (IsDay) _gameTime = GameTime.DAYTIME;
                if (IsNight) _gameTime = GameTime.NIGHTTIME;

                return _gameTime;
            }
            set => _gameTime = value;
        }

        public int HourOfDay
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _hourOfDay = CampaignTime.Now.GetHourOfDay;

                return _hourOfDay;
            }
            set => _hourOfDay = value;
        }

        public int HoursInDay
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _hoursInDay = CampaignTime.HoursInDay;

                return _hoursInDay;
            }
            set => _hoursInDay = value;
        }

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

        public bool IsDay
        {
            get
            {
                if (CampaignState.CurrentGameStarted())
                {
                    _isDay = Campaign.Current.IsDay;
                    _isNight = !_isDay;
                }

                return _isDay;
            }

            set => _isDay = value;
        }

        public bool IsNight
        {
            get
            {
                if (CampaignState.CurrentGameStarted())
                {
                    _isNight = Campaign.Current.IsDay;
                    _isDay = !_isNight;
                }

                return _isDay;
            }

            set => _isDay = value;
        }


        public string LastGameMenuOpened { get; set; }

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


        public bool IsActLocationValidInContext(IAct act)
        {
            switch (act.Location)
            {
                case Location.UNKNOWN:                                               return true;
                case Location.MAP when IsCurrentlyOnMap != null:                     return (bool)IsCurrentlyOnMap;
                case Location.SETTLEMENT when IsCurrentlyInSettlement != null:       return (bool)IsCurrentlyInSettlement;
                case Location.VILLAGE when IsCurrentlyInVillage != null:             return (bool)IsCurrentlyInVillage;
                case Location.DUNGEON when IsCurrentlyInDungeon != null:             return (bool)IsCurrentlyInDungeon;
                case Location.CASTLE when IsCurrentlyInCastle != null:               return (bool)IsCurrentlyInCastle;
                case Location.FORTIFICATION when IsCurrentlyInFortification != null: return (bool)IsCurrentlyInFortification;
                case Location.TOWN when IsCurrentlyInTown != null:                   return (bool)IsCurrentlyInTown;
                case Location.HIDEOUT when IsCurrentlyInHideout != null:             return (bool)IsCurrentlyInHideout;

                default: return true;
            }
        }


        public bool ReadyToShowNewEvent()
        {
            var f = EventChanceBonus + HourOfDay;
            TalesRandom.InitRandomNumber(Guid.NewGuid().GetHashCode());

            var diceRoll = TalesRandom.GenerateRandomNumber(100 + f);
            var result = diceRoll > 100;

            if (result) ResetEventChanceBonus();
            GameData.Instance.GameContext.EventChanceBonus++;

            return result;
        }

        public void ResetEventChanceBonus()
        {
            EventChanceBonus = 0;
        }

        public IAct RetrieveActToPlay()
        {
            var qualifiedActs = GetAllQualifiedActsAndSequences();

            if (qualifiedActs.Count == 0) return null;

            return ChooseOneToPlay(qualifiedActs);
        }

        #region private

        private static bool StoryAlreadyPlayed(IStory story)
        {
            if (!story.Header.CanBePlayedOnlyOnce) return false;

            foreach (var playedStory in GameData.Instance.StoryContext.PlayedStories)
                if (playedStory.Id == story.Id)
                    return true;

            return false;
        }

        private IAct ChooseOneToPlay(List<IAct> qualifiedActs)
        {
            if (qualifiedActs.Count == 0) return null;

            var index = new Random().Next(0, qualifiedActs.Count);
            GameData.Instance.StoryContext.PlayedActs.Add(qualifiedActs[index]);

            return qualifiedActs[index];
        }

        private List<IAct> GetAllQualifiedActsAndSequences()
        {
            var result = new List<IAct>();
            foreach (var s in GameData.Instance.StoryContext.Stories.Where(n => n.Header.TypeOfStory != StoryType.WAITING && n.Header.Name.ToUpper() != "TEST"))
            {
                var story = new Story(s);

                if (story.AlreadyPlayed()) continue;

                result.AddRange(GetQualifiedActs(story)); //BUG: got not qualified
            }

            return result;
        }


        private List<IAct> GetQualifiedActs(IStory story)
        {
            var result = new List<IAct>();
            foreach (var a in story.Acts)
            {
                var act = new Act(a);

                if (act.AlreadyPlayed()) continue;

                if (act.IsQualifiedRightNow()) result.Add(act);
            }

            return result;
        }

        private List<IAct> GetQualifiedSequences(IStory story)
        {
            var result = new List<IAct>();
            foreach (var s in story.Sequences)
            {
                var sequence = new Sequence(s);
                if (sequence.IsQualifiedRightNow()) result.Add(sequence);
            }

            return result;
        }

        #endregion
    }
}