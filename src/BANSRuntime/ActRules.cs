// Code written by Gabriel Mailhot, 06/09/2020.

#region

using System;
using TalesContract;
using TalesEntities.Stories;
using TalesEnums;
using TalesPersistence;

#endregion

namespace BannerlordTales
{
   public class ActRules
   {
      public ActRules(Act act)
      {
         Act = act;
      }

      public ActRules(Sequence sequence)
      {
         Act = sequence;
      }

      private IAct Act { get; }

      public bool AllConditionsConformToEvent()
      {
         foreach (IChoice choice in Act.Choices)
         {
            foreach (IEvaluation condition in choice.Conditions)
            {
               switch (condition.Subject)
               {
                  case Actor.PLAYER:
                     return IsConsequenceConformFor(GameData.Instance.GameContext.Player, condition);

                  case Actor.NPC:
                     return IsConsequenceConformFor(GameData.Instance.GameContext.Npc, condition);


                  case Actor.UNKNOWN:
                     throw new ApplicationException("Act's Consequence evaluation failed: actor unknown.");

                  default:
                     throw new ArgumentOutOfRangeException();
               }
            }
         }

         return true;
      }


      public bool CorrespondingLocationValidated()
      {
         switch (Act.Location)
         {
            case Location.UNKNOWN:
               return true;

            case Location.MAP when GameData.Instance.GameContext.IsCurrentlyOnMap != null:                     return (bool) GameData.Instance.GameContext.IsCurrentlyOnMap;
            case Location.SETTLEMENT when GameData.Instance.GameContext.IsCurrentlyInSettlement != null:       return (bool) GameData.Instance.GameContext.IsCurrentlyInSettlement;
            case Location.VILLAGE when GameData.Instance.GameContext.IsCurrentlyInVillage != null:             return (bool) GameData.Instance.GameContext.IsCurrentlyInVillage;
            case Location.DUNGEON when GameData.Instance.GameContext.IsCurrentlyInDungeon != null:             return (bool) GameData.Instance.GameContext.IsCurrentlyInDungeon;
            case Location.CASTLE when GameData.Instance.GameContext.IsCurrentlyInCastle != null:               return (bool) GameData.Instance.GameContext.IsCurrentlyInCastle;
            case Location.FORTIFICATION when GameData.Instance.GameContext.IsCurrentlyInFortification != null: return (bool) GameData.Instance.GameContext.IsCurrentlyInFortification;
            case Location.TOWN when GameData.Instance.GameContext.IsCurrentlyInTown != null:                   return (bool) GameData.Instance.GameContext.IsCurrentlyInTown;
            case Location.HIDEOUT when GameData.Instance.GameContext.IsCurrentlyInHideout != null:             return (bool) GameData.Instance.GameContext.IsCurrentlyInHideout;

            default: return true;
         }
      }

      public bool LinkedSequencesExists()
      {
         foreach (IChoice choice in Act.Choices)
            foreach (ITrigger trigger in choice.Triggers)
               foreach (IStory story in GameData.Instance.StoryContext.Stories)
                  foreach (IAct act in story.Acts)
                     if (act.Name == trigger.Link)
                        return true;

         return false;
      }

      #region private

      private bool IsAttributeConformFor(IBasicCharacterObject actor, IEvaluation consequence)
      {
         throw new NotImplementedException();
      }

      private bool IsCharacteristicConformFor(IBasicCharacterObject actor, IEvaluation consequence)
      {
         switch (consequence.Characteristic)
         {
            case Characteristics.UNKNOWN:
               throw new NotImplementedException();


            case Characteristics.AGE:
               throw new NotImplementedException();

            case Characteristics.GENDER:
               throw new NotImplementedException();

            case Characteristics.HEALTH:
               throw new NotImplementedException();

            case Characteristics.GOLD:
               throw new NotImplementedException();

            case Characteristics.RENOWN:
               return IsRenownConformFor(actor, consequence);

            case null:
               throw new NotImplementedException();

            default:
               throw new ArgumentOutOfRangeException();
         }
      }

      private bool IsConsequenceConformFor(IBasicCharacterObject actor, IEvaluation consequence)
      {
         if (consequence.PersonalityTrait != null) return IsPersonalityTraitConformFor(actor, consequence);

         if (consequence.Attribute != null) return IsAttributeConformFor(actor, consequence);

         if (consequence.Skill != null) return IsSkillConformFor(actor, consequence);

         if (consequence.Characteristic != null) return IsCharacteristicConformFor(actor, consequence);

         return true;
      }

      private bool IsPersonalityTraitConformFor(IBasicCharacterObject actor, IEvaluation consequence)
      {
         throw new NotImplementedException();
      }

      private bool IsRenownConformFor(IBasicCharacterObject actor, IEvaluation consequence)
      {
         switch (consequence.Operator)
         {
            case Operator.UNKNOWN:
               break;

            case Operator.GREATERTHAN:
               break;

            case Operator.LOWERTHAN:
               break;

            case Operator.EQUALTO:
            {
               //TODO: must find how to recover renown
               break;
            }

            default:
               throw new ArgumentOutOfRangeException();
         }

         return false;
      }

      private bool IsSkillConformFor(IBasicCharacterObject actor, IEvaluation consequence)
      {
         throw new NotImplementedException();
      }

      #endregion
   }
}