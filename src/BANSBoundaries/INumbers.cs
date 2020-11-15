// Code written by Gabriel Mailhot, 14/11/2020.

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
    }
}