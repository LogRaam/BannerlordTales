// Code written by Gabriel Mailhot, 02/12/2023.

#region

using System;
using TalesBase.TW;
using TalesContract;
using TalesEnums;

#endregion

namespace TalesPersistence.Entities
{
    public class BasicCharacterObject : BaseBasicCharacterObject
    {
        public bool IsConsequenceConformFor(IEvaluation consequence)
        {
            if (consequence.Persona.PersonalityTrait != null) return IsPersonalityTraitConformFor(consequence);

            if (consequence.Persona.Attribute != null) return IsAttributeConformFor(consequence);

            if (consequence.Persona.Skill != null) return IsSkillConformFor(consequence);

            if (consequence.Persona.Characteristic != null) return IsCharacteristicConformFor(consequence);

            return true;
        }

        #region private

        private bool IsAttributeConformFor(IEvaluation consequence)
        {
            switch (consequence.Persona.Attribute)
            {
                case Attributes.Vigor: return IsVigorConformFor(consequence);
                case Attributes.Control: return IsControlConformFor(consequence);
                case Attributes.Endurance: return IsEnduranceConformFor(consequence);
                case Attributes.Cunning: return IsCunningConformFor(consequence);
                case Attributes.Social: return IsSocialConformFor(consequence);
                case Attributes.Intelligence: return IsIntelligenceConformFor(consequence);
                case Attributes.NotAssigned: return true;
                case null: return true;

                default: return true;
            }
        }

        private bool IsCharacteristicConformFor(IEvaluation consequence)
        {
            switch (consequence.Persona.Characteristic)
            {
                case Characteristics.NotAssigned:
                    throw new NotImplementedException();


                case Characteristics.Age:
                    throw new NotImplementedException();

                case Characteristics.Gender:
                    throw new NotImplementedException();

                case Characteristics.Health:
                    throw new NotImplementedException();

                case Characteristics.Gold:
                    throw new NotImplementedException();

                case Characteristics.Renown:
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
            switch (consequence.Numbers.Operator)
            {
                case Operator.Unknown:
                    break;

                case Operator.Greaterthan:
                    break;

                case Operator.Lowerthan:
                    break;

                case Operator.Equalto:
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
            switch (consequence.Numbers.Operator)
            {
                case Operator.Unknown: throw new ApplicationException("Operator unknown when trying to evaluate Vigor.");
                case Operator.Greaterthan: return VigorGreaterThanConformFrom(consequence);
                case Operator.Lowerthan: return VigorLowerThanConformFrom(consequence);
                case Operator.Equalto: return VigorEqualToConformFrom(consequence);
                case Operator.Notequalto: return VigorNotEqualToConformFrom(consequence);
                default: throw new ArgumentOutOfRangeException();
            }
        }


        private bool VigorEqualToConformFrom(IEvaluation consequence)
        {
            return Vigor == int.Parse(consequence.Numbers.Value);
        }

        private bool VigorGreaterThanConformFrom(IEvaluation consequence)
        {
            return Vigor > int.Parse(consequence.Numbers.Value);
        }

        private bool VigorLowerThanConformFrom(IEvaluation consequence)
        {
            return Vigor < int.Parse(consequence.Numbers.Value);
        }

        private bool VigorNotEqualToConformFrom(IEvaluation consequence)
        {
            return Vigor != int.Parse(consequence.Numbers.Value);
        }

        #endregion
    }
}