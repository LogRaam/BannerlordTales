// Code written by Gabriel Mailhot, 02/12/2023.

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
            switch (consequence.Numbers.Operator)
            {
                case Operator.Unknown: break;
                case Operator.Greaterthan: return attributeValue > float.Parse(consequence.Numbers.Value);
                case Operator.Lowerthan: return attributeValue < float.Parse(consequence.Numbers.Value);
                case Operator.Equalto: return Math.Abs(attributeValue - float.Parse(consequence.Numbers.Value)) < 0.000000001;
                case Operator.Notequalto: return Math.Abs(attributeValue - float.Parse(consequence.Numbers.Value)) > 0.000000001;
                default: throw new ArgumentOutOfRangeException();
            }

            return false;
        }


        public static bool IsEvaluationConform(IEvaluation consequence, int attributeValue)
        {
            switch (consequence.Numbers.Operator)
            {
                case Operator.Unknown: break;
                case Operator.Greaterthan: return attributeValue > int.Parse(consequence.Numbers.Value);
                case Operator.Lowerthan: return attributeValue < int.Parse(consequence.Numbers.Value);
                case Operator.Equalto: return attributeValue == int.Parse(consequence.Numbers.Value);
                case Operator.Notequalto: return attributeValue != int.Parse(consequence.Numbers.Value);
                default: throw new ArgumentOutOfRangeException();
            }

            return false;
        }
    }
}