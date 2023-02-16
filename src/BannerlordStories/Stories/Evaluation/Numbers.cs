// Code written by Gabriel Mailhot, 02/12/2023.

#region

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
    }
}