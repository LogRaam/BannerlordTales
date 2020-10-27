// Code written by Gabriel Mailhot, 26/10/2020.

#region

using System;
using TalesBase.TW;
using TalesContract;
using TalesEnums;

#endregion

namespace TalesEntities
{
    public class BasicCharacterObject : BaseBasicCharacterObject
    {
        public bool IsConsequenceConformFor(IEvaluation consequence)
        {
            if (consequence.PersonalityTrait != null) return IsPersonalityTraitConformFor(consequence);

            if (consequence.Attribute != null) return IsAttributeConformFor(consequence);

            if (consequence.Skill != null) return IsSkillConformFor(consequence);

            if (consequence.Characteristic != null) return IsCharacteristicConformFor(consequence);

            return true;
        }

        #region private

        private bool IsAttributeConformFor(IEvaluation consequence)
        {
            switch (consequence.Attribute)
            {
                case Attributes.VIGOR:        return IsVigorConformFor(consequence);
                case Attributes.CONTROL:      return IsControlConformFor(consequence);
                case Attributes.ENDURANCE:    return IsEnduranceConformFor(consequence);
                case Attributes.CUNNING:      return IsCunningConformFor(consequence);
                case Attributes.SOCIAL:       return IsSocialConformFor(consequence);
                case Attributes.INTELLIGENCE: return IsIntelligenceConformFor(consequence);
                case Attributes.UNKNOWN:      return true;
                case null:                    return true;

                default: return true;
            }
        }

        private bool IsCharacteristicConformFor(IEvaluation consequence)
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
                    return IsRenownConformFor(consequence);

                case null:
                    throw new NotImplementedException();

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool IsControlConformFor(IEvaluation consequence)
        {
            throw new NotImplementedException();
        }

        private bool IsCunningConformFor(IEvaluation consequence)
        {
            throw new NotImplementedException();
        }

        private bool IsEnduranceConformFor(IEvaluation consequence)
        {
            throw new NotImplementedException();
        }

        private bool IsIntelligenceConformFor(IEvaluation consequence)
        {
            throw new NotImplementedException();
        }

        private bool IsPersonalityTraitConformFor(IEvaluation consequence)
        {
            throw new NotImplementedException();
        }

        private bool IsRenownConformFor(IEvaluation consequence)
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

        private bool IsSkillConformFor(IEvaluation consequence)
        {
            throw new NotImplementedException();
        }

        private bool IsSocialConformFor(IEvaluation consequence)
        {
            throw new NotImplementedException();
        }

        private bool IsVigorConformFor(IEvaluation consequence)
        {
            switch (consequence.Operator)
            {
                case Operator.UNKNOWN:     throw new ApplicationException("Operator unknown when trying to evaluate Vigor.");
                case Operator.GREATERTHAN: return VigorGreaterThanConformFrom(consequence);
                case Operator.LOWERTHAN:   return VigorLowerThanConformFrom(consequence);
                case Operator.EQUALTO:     return VigorEqualToConformFrom(consequence);
                case Operator.NOTEQUALTO:  return VigorNotEqualToConformFrom(consequence);
                default:                   throw new ArgumentOutOfRangeException();
            }
        }


        private bool VigorEqualToConformFrom(IEvaluation consequence)
        {
            return Vigor == int.Parse(consequence.Value);
        }

        private bool VigorGreaterThanConformFrom(IEvaluation consequence)
        {
            return Vigor > int.Parse(consequence.Value);
        }

        private bool VigorLowerThanConformFrom(IEvaluation consequence)
        {
            return Vigor < int.Parse(consequence.Value);
        }

        private bool VigorNotEqualToConformFrom(IEvaluation consequence)
        {
            return Vigor != int.Parse(consequence.Value);
        }

        #endregion
    }
}