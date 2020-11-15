// Code written by Gabriel Mailhot, 26/10/2020.

#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using _45_TalesGameState;
using _47_TalesMath;
using TalesBase.Items;
using TalesBase.TW;
using TalesContract;
using TalesDAL;
using TalesEnums;
using TalesPersistence.Stories;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.TwoDimension;
using Location = TalesEnums.Location;

#endregion

namespace TalesPersistence.Context
{
    #region

    #endregion

    public class GameContext
    {
        private FileInfo _bodyArmorsFolder;
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

        public List<BaseBodyArmor> BodyArmors { get; set; }

        public FileInfo BodyArmorsFile
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _bodyArmorsFolder = new FileInfo(GameData.Instance.StoryContext.ModuleFolder.FullName + "\\Modules\\SandBoxCore\\ModuleData\\spitems\\body_armors.xml");

                return _bodyArmorsFolder;
            }
            set => _bodyArmorsFolder = value;
        }


        public IHero Captor
        {
            get
            {
                if (CampaignState.CurrentGameStarted())
                    if (Hero.MainHero.IsPrisoner)
                        _captor = new BaseHero(Campaign.Current.MainParty.LeaderHero);

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


        public string LastGameMenuOpened { get; set; } = "Unknown";

        public List<Texture> OriginalBackgroundSpriteSheets { get; set; }

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

        public Act ChooseQualifiedActFrom(List<Story> stories)
        {
            var acts = new List<Act>();
            foreach (var s in stories)
            {
                if (!s.IsQualifiedRightNow()) continue;

                foreach (var a in s.Acts)
                {
                    var act = new Act(a);

                    if (!act.IsQualifiedRightNow()) continue;

                    acts.Add(act);
                }
            }

            return acts[TalesRandom.GenerateRandomNumber(acts.Count)];
        }


        public List<Story> GetAlreadyPlayedStories()
        {
            var result = new List<Story>();

            foreach (var s in GameData.Instance.StoryContext.Stories)
            {
                var story = new Story(s);

                if (story.CanBePlayedOnceAndAlreadyPlayed()) continue;
                if (!story.AlreadyPlayed()) continue;

                if (story.IsQualifiedRightNow()) result.Add(story);
            }

            return result;
        }

        public List<Story> GetNewStories()
        {
            var result = new List<Story>();

            foreach (var s in GameData.Instance.StoryContext.Stories)
            {
                var story = new Story(s);

                if (story.CanBePlayedOnceAndAlreadyPlayed()) continue;
                if (story.AlreadyPlayed()) continue;

                if (story.IsQualifiedRightNow()) result.Add(story);
            }

            return result;
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


        public IAct RetrieveActToPlay(StoryType storyType)
        {
            var qualifiedActs = GetAllPrisonerQualifiedActs().Where(n => n.ParentStory.Header.TypeOfStory == storyType).ToList();

            return qualifiedActs.Count > 0
                ? ChooseOneToPlay(qualifiedActs)
                : null;
        }

        #region private

        private IAct ChooseOneToPlay(List<IAct> qualifiedActs)
        {
            if (qualifiedActs.Count == 0) return null;

            var index = new Random().Next(0, qualifiedActs.Count);


            GameData.Instance.StoryContext.AddToPlayedActs(qualifiedActs[index]);

            return qualifiedActs[index];
        }


        private List<IAct> GetAllPrisonerQualifiedActs()
        {
            GameFunction.Log("GetAllPrisonerQualifiedActs()");

            return GetAllQualifiedActs(GameData.Instance.StoryContext.Stories);
        }


        private List<IAct> GetAllQualifiedActs(List<IStory> stories)
        {
            var result = new List<IAct>();
            foreach (var s in stories)
            {
                if (s.Header.Name.ToUpper() == "TEST") continue;

                var story = new Story(s);

                result.AddRange(GetQualifiedActs(story));
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

        #endregion
    }
}