// Code written by Gabriel Mailhot, 02/12/2023.  Updated by  Gabriel Mailhot on 02/19/2023.

#region

using FluentAssertions.Execution;
using TalesContract;
using TalesEnums;

#endregion

namespace TalesBase.Stories.Evaluation
{
    public class Numbers : INumbers
    {
        public Operator Operator { get; set; } = Operator.Unknown;
        public int RandomEnd { get; set; }
        public int RandomStart { get; set; }
        public string Value { get; set; }
        public bool ValueIsPercentage { get; set; }

        public void IsEquivalentTo(INumbers numbers)
        {
            if (numbers.ValueIsPercentage != ValueIsPercentage) throw new AssertionFailedException("Evaluation expected numbers.ValueIsPercentage to be equivalent to " + numbers.ValueIsPercentage + ", but found that its value is " + ValueIsPercentage);
            if (numbers.Operator != Operator) throw new AssertionFailedException("Evaluation expected numbers.Operator to be equivalent to " + numbers.Operator + ", but found that its value is " + Operator);
            if (numbers.Value != Value) throw new AssertionFailedException("Evaluation expected numbers.Value to be equivalent to " + numbers.Value + ", but found that its value is " + Value);
            if (numbers.RandomStart != RandomStart) throw new AssertionFailedException("Evaluation expected numbers.RandomStart to be equivalent to " + numbers.RandomStart + ", but found that its value is " + RandomStart);
            if (numbers.RandomEnd != RandomEnd) throw new AssertionFailedException("Evaluation expected numbers.RandomEnd to be equivalent to " + numbers.RandomEnd + ", but found that its value is " + RandomEnd);
        }
    }
}