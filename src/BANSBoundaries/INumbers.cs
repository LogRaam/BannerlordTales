// Code written by Gabriel Mailhot, 02/12/2023.  Updated by  Gabriel Mailhot on 02/19/2023.

#region

using TalesEnums;

#endregion

namespace TalesContract
{
    public interface INumbers
    {
        public Operator Operator { get; set; }
        public int RandomEnd { get; set; }
        public int RandomStart { get; set; }
        public string Value { get; set; }
        public bool ValueIsPercentage { get; set; }
        void IsEquivalentTo(INumbers numbers);
    }
}