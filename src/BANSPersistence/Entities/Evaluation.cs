// Code written by Gabriel Mailhot, 12/10/2020.

#region

using System;
using TalesContract;
using TalesEntities.Stories;
using TalesEnums;

#endregion

namespace TalesPersistence.Entities
{
    public class Evaluation : BaseEvaluation
    {
        public Evaluation(IEvaluation condition)
        {
            Subject = condition.Subject;

            Attribute = condition.Attribute;
            Characteristic = condition.Characteristic;
            PartyType = condition.PartyType;
            PersonalityTrait = condition.PersonalityTrait;
            Skill = condition.Skill;
            Time = condition.Time;

            PregnancyRisk = condition.PregnancyRisk;

            Operator = condition.Operator;

            RandomStart = condition.RandomStart;
            RandomEnd = condition.RandomEnd;

            Value = condition.Value;
            ValueIsPercentage = condition.ValueIsPercentage;
        }

        public Evaluation() { }

        public bool IsAccepted()
        {
            if (!AttributeAccepted()) return false;
            if (!CharacteristicAccepted()) return false;
            if (!PartyTypeAccepted()) return false;
            if (!PersonalityTraitAccepted()) return false;
            if (!SkillAccepted()) return false;
            if (!TimeAccepted()) return false;

            return true;
        }

        #region private

        private bool AttributeAccepted()
        {
            switch (Attribute)
            {
                case Attributes.UNKNOWN:      throw new ApplicationException("Attribute unknown: " + Attribute);
                case Attributes.VIGOR:        return EvalOperation(IdentifySubject().Vigor);
                case Attributes.CONTROL:      return EvalOperation(IdentifySubject().Control);
                case Attributes.ENDURANCE:    return EvalOperation(IdentifySubject().Endurance);
                case Attributes.CUNNING:      return EvalOperation(IdentifySubject().Cunning);
                case Attributes.SOCIAL:       return EvalOperation(IdentifySubject().Social);
                case Attributes.INTELLIGENCE: return EvalOperation(IdentifySubject().Intelligence);
                case null:                    throw new NullReferenceException("Attribute is null");
                default:                      throw new ArgumentOutOfRangeException();
            }
        }

        private bool CharacteristicAccepted()
        {
            switch (Characteristic)
            {
                case Characteristics.UNKNOWN:    throw new ApplicationException("Characteristic Unknown");
                case Characteristics.AGE:        return EvalOperation((int)IdentifySubject().Age);
                case Characteristics.GENDER:     return EvalGender(IdentifySubject());
                case Characteristics.HEALTH:     return EvalOperation(IdentifySubject().HitPoints);
                case Characteristics.GOLD:       return EvalOperation(IdentifySubject().Gold);
                case Characteristics.RENOWN:     return EvalOperation((int)IdentifySubject().Clan.Renown);
                case Characteristics.CULTURE:    return Value.ToUpper() == IdentifySubject().Culture.CultureCode.ToString();
                case Characteristics.OCCUPATION: return EvalOccupation(IdentifySubject());
                case null:                       throw new NullReferenceException("Characteristic is null");
                default:                         throw new ArgumentOutOfRangeException();
            }

            return false;
        }


        private bool EvalGender(Hero subject)
        {
            var t = Value.ToUpper();

            if (t == "ANY") return true;
            if (subject.IsFemale) return t == "FEMALE";

            return t == "MALE";
        }

        private bool EvalOccupation(Hero subject)
        {
            switch (Value.ToUpper())
            {
                case "ARTISAN":          return subject.IsArtisan;
                case "CHILD":            return subject.IsChild;
                case "COMMANDER":        return subject.IsCommander;
                case "FACTIONLEADER":    return subject.IsFactionLeader;
                case "FUGITIVE":         return subject.IsFugitive;
                case "GANGLEADER":       return subject.IsGangLeader;
                case "HEADMAN":          return subject.IsHeadman;
                case "MERCENARY":        return subject.IsMercenary;
                case "MERCHANT":         return subject.IsMerchant;
                case "MINORFACTIONHERO": return subject.IsMinorFactionHero;
                case "NOBLE":            return subject.IsNoble;
                case "NOTABLE":          return subject.IsNotable;
                case "OUTLAW":           return subject.IsOutlaw;
                case "PARTYLEADER":      return subject.IsPartyLeader;
                case "PLAYERCOMPANION":  return subject.IsPlayerCompanion;
                case "PREACHER":         return subject.IsPreacher;
                case "REBEL":            return subject.IsRebel;
                case "RURALNOTABLE":     return subject.IsRuralNotable;
                case "SPECIAL":          return subject.IsSpecial;
                case "WANDERER":         return subject.IsWanderer;
                default:                 throw new ArgumentOutOfRangeException();
            }
        }


        private bool EvalOperation(int n)
        {
            var value = int.Parse(Value);

            switch (Operator)
            {
                case Operator.UNKNOWN:     throw new ApplicationException("Operator unknown");
                case Operator.GREATERTHAN: return n > value;
                case Operator.LOWERTHAN:   return n < value;
                case Operator.EQUALTO:     return n == value;
                case Operator.NOTEQUALTO:  return n != value;
                default:                   throw new ArgumentOutOfRangeException();
            }
        }

        private Hero IdentifySubject()
        {
            return Subject == Actor.NPC
                ? new Hero(GameData.Instance.GameContext.Captor)
                : new Hero(GameData.Instance.GameContext.Player);
        }


        private bool PartyTypeAccepted()
        {
            //TODO: must find a way to evaluate party type from encounters
            return true;
        }

        private bool PersonalityTraitAccepted()
        {
            throw new NotImplementedException();
        }

        private bool SkillAccepted()
        {
            throw new NotImplementedException();
        }


        private bool TimeAccepted()
        {
            switch (Time)
            {
                case GameTime.ANYTIME:
                case GameTime.NONE:
                case GameTime.UNKNOWN:
                    return true;
            }

            return Time == GameData.Instance.GameContext.GameTime;
        }

        #endregion
    }
}