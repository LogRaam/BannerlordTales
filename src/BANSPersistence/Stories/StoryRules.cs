// Code written by Gabriel Mailhot, 27/09/2020.

#region

using System;
using TalesContract;
using TalesEnums;
using TalesPersistence.Context;

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
                if (restriction.Attribute != null)
                    if (IsAttributeRestricted(restriction))
                        return false;

                if (restriction.PersonalityTrait != null)
                    if (IsPersonalityTraitRestricted(restriction))
                        return false;

                if (restriction.Skill != null)
                    if (IsSkillRestricted(restriction))
                        return false;
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
                case StoryType.PLAYER_IS_CAPTIVE:      return GameData.Instance.GameContext.Player.IsPrisoner;
                case StoryType.PLAYER_IS_CAPTOR:       return GameData.Instance.GameContext.PlayerIsCaptor != null && (bool)GameData.Instance.GameContext.PlayerIsCaptor;
                case StoryType.PLAYER_ON_CAMPAIGN_MAP: return GameData.Instance.GameContext.IsCurrentlyOnMap != null && (bool)GameData.Instance.GameContext.IsCurrentlyOnMap;
                case StoryType.PLAYER_IN_SETTLEMENT:   return GameData.Instance.GameContext.IsCurrentlyInSettlement != null && (bool)GameData.Instance.GameContext.IsCurrentlyInSettlement;
                case StoryType.PLAYER_SURRENDER:       return GameData.Instance.GameContext.Player.IsPrisoner;
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
                case GameTime.DAYTIME:   return GameData.Instance.GameContext.IsDay;
                case GameTime.NIGHTTIME: return GameData.Instance.GameContext.IsNight;
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

        #region private

        private static bool IsNpcVigorEqualTo(string value)
        {
            throw new NotImplementedException();
        }

        private static bool IsNpcVigorGreaterThan(string value)
        {
            throw new NotImplementedException();
        }

        private static bool IsNpcVigorLowerThan(string value)
        {
            throw new NotImplementedException();
        }

        private static bool IsNpcVigorPercentageEqualTo(string value)
        {
            throw new NotImplementedException();
        }

        private static bool IsNpcVigorPercentageGreaterThan(string value)
        {
            throw new NotImplementedException();
        }

        private static bool IsNpcVigorPercentageLowerThan(string value)
        {
            throw new NotImplementedException();
        }

        private static bool IsNpcVigorRestricted(IEvaluation restriction)
        {
            switch (restriction.Operator)
            {
                case Operator.EQUALTO when restriction.ValueIsPercentage:     return IsNpcVigorPercentageEqualTo(restriction.Value);
                case Operator.EQUALTO:                                        return IsNpcVigorEqualTo(restriction.Value);
                case Operator.GREATERTHAN when restriction.ValueIsPercentage: return IsNpcVigorPercentageGreaterThan(restriction.Value);
                case Operator.GREATERTHAN:                                    return IsNpcVigorGreaterThan(restriction.Value);
                case Operator.LOWERTHAN when restriction.ValueIsPercentage:   return IsNpcVigorPercentageLowerThan(restriction.Value);
                case Operator.LOWERTHAN:                                      return IsNpcVigorLowerThan(restriction.Value);

                case Operator.UNKNOWN:
                    // todo: implement custom attribute
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return false;
        }

        private static bool IsPlayerVigorEqualTo(string value)
        {
            throw new NotImplementedException();
        }

        private static bool IsPlayerVigorGreaterThan(string value)
        {
            throw new NotImplementedException();
        }

        private static bool IsPlayerVigorLowerThan(string value)
        {
            throw new NotImplementedException();
        }

        private static bool IsPlayerVigorPercentageEqualTo(string value)
        {
            throw new NotImplementedException();
        }

        private static bool IsPlayerVigorPercentageGreaterThan(string value)
        {
            throw new NotImplementedException();
        }

        private static bool IsPlayerVigorPercentageLowerThan(string value)
        {
            throw new NotImplementedException();
        }

        private static bool IsPlayerVigorRestricted(IEvaluation restriction)
        {
            switch (restriction.Operator)
            {
                case Operator.EQUALTO when restriction.ValueIsPercentage:     return IsPlayerVigorPercentageEqualTo(restriction.Value);
                case Operator.EQUALTO:                                        return IsPlayerVigorEqualTo(restriction.Value);
                case Operator.GREATERTHAN when restriction.ValueIsPercentage: return IsPlayerVigorPercentageGreaterThan(restriction.Value);
                case Operator.GREATERTHAN:                                    return IsPlayerVigorGreaterThan(restriction.Value);
                case Operator.LOWERTHAN when restriction.ValueIsPercentage:   return IsPlayerVigorPercentageLowerThan(restriction.Value);
                case Operator.LOWERTHAN:                                      return IsPlayerVigorLowerThan(restriction.Value);
                case Operator.UNKNOWN:                                        return false; // todo: implement custom attribute
                default:                                                      throw new ArgumentOutOfRangeException();
            }
        }

        private bool IsAttributeRestricted(IEvaluation restriction)
        {
            switch (restriction.Attribute)
            {
                case Attributes.VIGOR:        return IsVigorRestricted(restriction);
                case Attributes.CONTROL:      return IsControlRestricted(restriction);
                case Attributes.ENDURANCE:    return IsEnduranceRestricted(restriction);
                case Attributes.CUNNING:      return IsCunningRestricted(restriction);
                case Attributes.SOCIAL:       return IsSocialRestricted(restriction);
                case Attributes.INTELLIGENCE: return IsIntelligenceRestricted(restriction);
                case null:                    return false;
                default:                      return false;
            }
        }

        private bool IsControlRestricted(IEvaluation restriction)
        {
            return IsPlayerControlRestricted(restriction);
        }

        private bool IsCunningRestricted(IEvaluation restriction)
        {
            return IsPlayerCunningRestricted(restriction);
        }

        private bool IsEnduranceRestricted(IEvaluation restriction)
        {
            return IsPlayerEnduranceRestricted(restriction);
        }

        private bool IsIntelligenceRestricted(IEvaluation restriction)
        {
            return IsPlayerIntelligenceRestricted(restriction);
        }

        private bool IsNpcControlRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsNpcCunningRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsNpcEnduranceRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsNpcGenerosityTraitRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsNpcHonorTraitRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsNpcIntelligenceRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsNpcMercyTraitRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsNpcPersonalityTraitsRestricted(IEvaluation restriction)
        {
            switch (restriction.PersonalityTrait)
            {
                case PersonalityTraits.UNKNOWN:    return false;
                case PersonalityTraits.MERCY:      return IsNpcMercyTraitRestricted(restriction);
                case PersonalityTraits.GENEROSITY: return IsNpcGenerosityTraitRestricted(restriction);
                case PersonalityTraits.HONOR:      return IsNpcHonorTraitRestricted(restriction);
                case PersonalityTraits.VALOR:      return IsNpcValorTraitRestricted(restriction);
                case null:                         return false;
                default:                           return false;
            }
        }

        private bool IsNpcSkillRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsNpcSocialRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsNpcValorTraitRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPersonalityTraitRestricted(IEvaluation restriction)
        {
            switch (restriction.Subject)
            {
                case Actor.UNKNOWN: return false;
                case Actor.PLAYER:  return IsPlayerPersonalityTraitsRestricted(restriction);
                case Actor.NPC:     return IsNpcPersonalityTraitsRestricted(restriction);
                default:            return false;
            }
        }

        private bool IsPlayerAthleticSkillRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerBowSkillRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerCharmSkillRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerControlRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerCraftingSkillRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerCrossbowSkillRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerCunningRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerEnduranceRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerEngineeringSkillRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerGenerosityTraitRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerHonorTraitRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerIntelligenceRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerLeadershipSkillRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerMedicineSkillRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerMercyTraitRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerOneHandedSkillRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerPersonalityTraitsRestricted(IEvaluation restriction)
        {
            switch (restriction.PersonalityTrait)
            {
                case PersonalityTraits.UNKNOWN:    return false;
                case PersonalityTraits.MERCY:      return IsPlayerMercyTraitRestricted(restriction);
                case PersonalityTraits.GENEROSITY: return IsPlayerGenerosityTraitRestricted(restriction);
                case PersonalityTraits.HONOR:      return IsPlayerHonorTraitRestricted(restriction);
                case PersonalityTraits.VALOR:      return IsPlayerValorTraitRestricted(restriction);
                case null:                         return false;
                default:                           return false;
            }
        }

        private bool IsPlayerPolearmSkillRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerRidingSkillRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerRoguerySkillRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerScoutingSkillRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerSkillRestricted(IEvaluation restriction)
        {
            switch (restriction.Skill)
            {
                case Skills.UNKNOWN:     return false;
                case Skills.ONEHANDED:   return IsPlayerOneHandedSkillRestricted(restriction);
                case Skills.TWOHANDED:   return IsPlayerTwoHandedSkillRestricted(restriction);
                case Skills.POLEARM:     return IsPlayerPolearmSkillRestricted(restriction);
                case Skills.BOW:         return IsPlayerBowSkillRestricted(restriction);
                case Skills.CROSSBOW:    return IsPlayerCrossbowSkillRestricted(restriction);
                case Skills.THROWING:    return IsPlayerThrowingSkillRestricted(restriction);
                case Skills.RIDING:      return IsPlayerRidingSkillRestricted(restriction);
                case Skills.ATHLETICS:   return IsPlayerAthleticSkillRestricted(restriction);
                case Skills.CRAFTING:    return IsPlayerCraftingSkillRestricted(restriction);
                case Skills.SCOUTING:    return IsPlayerScoutingSkillRestricted(restriction);
                case Skills.TACTICS:     return IsPlayerTacticsSkillRestricted(restriction);
                case Skills.ROGUERY:     return IsPlayerRoguerySkillRestricted(restriction);
                case Skills.CHARM:       return IsPlayerCharmSkillRestricted(restriction);
                case Skills.LEADERSHIP:  return IsPlayerLeadershipSkillRestricted(restriction);
                case Skills.TRADE:       return IsPlayerTradeSkillRestricted(restriction);
                case Skills.STEWARD:     return IsPlayerStewardSkillRestricted(restriction);
                case Skills.MEDICINE:    return IsPlayerMedicineSkillRestricted(restriction);
                case Skills.ENGINEERING: return IsPlayerEngineeringSkillRestricted(restriction);
                case null:               return false;
                default:                 return false;
            }
        }

        private bool IsPlayerSocialRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerStewardSkillRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerTacticsSkillRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerThrowingSkillRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerTradeSkillRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerTwoHandedSkillRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerValorTraitRestricted(IEvaluation restriction)
        {
            throw new NotImplementedException();
        }

        private bool IsSkillRestricted(IEvaluation restriction)
        {
            switch (restriction.Subject)
            {
                case Actor.UNKNOWN: return false;
                case Actor.PLAYER:  return IsPlayerSkillRestricted(restriction);
                case Actor.NPC:     return IsNpcSkillRestricted(restriction);
                default:            return false;
            }
        }

        private bool IsSocialRestricted(IEvaluation restriction)
        {
            return IsPlayerSocialRestricted(restriction);
        }

        private bool IsVigorRestricted(IEvaluation restriction)
        {
            return IsPlayerVigorRestricted(restriction);
        }

        #endregion
    }
}