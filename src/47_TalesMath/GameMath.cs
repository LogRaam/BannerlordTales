// Code written by Gabriel Mailhot, 07/10/2020.

#region

using System;
using TalesContract;
using TalesEnums;

#endregion

namespace _47_TalesMath
{
    public static class GameMath
    {
        public static bool IsEvaluationConform(IEvaluation consequence, float attributeValue)
        {
            switch (consequence.Operator)
            {
                case Operator.UNKNOWN:     break;
                case Operator.GREATERTHAN: return attributeValue > float.Parse(consequence.Value);
                case Operator.LOWERTHAN:   return attributeValue < float.Parse(consequence.Value);
                case Operator.EQUALTO:     return Math.Abs(attributeValue - float.Parse(consequence.Value)) < 0.000000001;
                case Operator.NOTEQUALTO:  return Math.Abs(attributeValue - float.Parse(consequence.Value)) > 0.000000001;
                default:                   throw new ArgumentOutOfRangeException();
            }

            return false;
        }


        public static bool IsEvaluationConform(IEvaluation consequence, int attributeValue)
        {
            switch (consequence.Operator)
            {
                case Operator.UNKNOWN:     break;
                case Operator.GREATERTHAN: return attributeValue > int.Parse(consequence.Value);
                case Operator.LOWERTHAN:   return attributeValue < int.Parse(consequence.Value);
                case Operator.EQUALTO:     return attributeValue == int.Parse(consequence.Value);
                case Operator.NOTEQUALTO:  return attributeValue != int.Parse(consequence.Value);
                default:                   throw new ArgumentOutOfRangeException();
            }

            return false;
        }
    }
}