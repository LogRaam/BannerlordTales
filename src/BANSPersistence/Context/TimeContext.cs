// unset

#region

using _45_TalesGameState;
using TalesEnums;
using TaleWorlds.CampaignSystem;

#endregion

namespace TalesPersistence.Context
{
    public class TimeContext
    {
        private GameTime _gameTime;
        private int _hourOfDay;

        private bool _isDay;
        private bool _isNight;

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
    }
}