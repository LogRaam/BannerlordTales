// unset

#region

using System;
using TalesEnums;
using TalesPersistence.Context;
using TalesPersistence.Entities;

#endregion

namespace TalesPersistence.Stories
{
    public class StoryRules
    {
        public StoryRules(Story story)
        {
            Story = story;
        }

        private Story Story { get; }

        public bool IsNotRestricted()
        {
            if (Story.Restrictions.Count == 0) return true;

            foreach (var restriction in Story.Restrictions)
            {
                var r = new Evaluation(restriction);

                if (!r.CanBePlayedInContext()) return false;
            }

            return true;
        }

        public bool IsOneTimeStoryNeverPlayed()
        {
            return !(Story.Header.CanBePlayedOnlyOnce && GameData.Instance.StoryContext.PlayedStories.Exists(n => n.Id == Story.Id));
        }


        public bool IsTheRightStoryType(StoryType storyType)
        {
            switch (storyType)
            {
                case StoryType.PLAYER_IS_CAPTIVE:      return GameData.Instance.GameContext.Heroes.Player.IsPrisoner;
                case StoryType.PLAYER_IS_CAPTOR:       return GameData.Instance.GameContext.Heroes.PlayerIsCaptor != null && (bool)GameData.Instance.GameContext.Heroes.PlayerIsCaptor;
                case StoryType.PLAYER_ON_CAMPAIGN_MAP: return GameData.Instance.GameContext.Tracking.IsCurrentlyOnMap != null && (bool)GameData.Instance.GameContext.Tracking.IsCurrentlyOnMap;
                case StoryType.PLAYER_IN_SETTLEMENT:   return GameData.Instance.GameContext.Tracking.IsCurrentlyInSettlement != null && (bool)GameData.Instance.GameContext.Tracking.IsCurrentlyInSettlement;
                case StoryType.PLAYER_SURRENDER:       return GameData.Instance.GameContext.Heroes.Player.IsPrisoner;
                case StoryType.NONE:                   return true;
                case StoryType.UNKNOWN:                throw new ApplicationException("Story type undefined.");
                case StoryType.WAITING:                break;
                default:                               throw new ApplicationException("Story type undefined.");
            }

            return true;
        }

        public bool IsTheRightTime()
        {
            switch (Story.Header.Time)
            {
                case GameTime.DAYTIME:   return GameData.Instance.GameContext.Time.IsDay;
                case GameTime.NIGHTTIME: return GameData.Instance.GameContext.Time.IsNight;
                case GameTime.ANYTIME:   return true;
                case GameTime.NONE:      return true;
                case GameTime.UNKNOWN:   return true;
                default:                 return true;
            }
        }

        public bool ItsDependenciesAreCleared()
        {
            if (string.IsNullOrEmpty(Story.Header.DependOn)) return true;
            if (Story.Header.DependOn.ToUpper() == "NONE") return true;

            return GameData.Instance.StoryContext.PlayedStories.Exists(n => n.Id == Story.Header.DependOn);
        }
    }
}