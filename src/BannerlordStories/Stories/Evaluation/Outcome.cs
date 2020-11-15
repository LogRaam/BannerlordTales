// Code written by Gabriel Mailhot, 14/11/2020.

#region

using TalesContract;

#endregion

namespace TalesBase.Stories.Evaluation
{
    public class Outcome : IOutcome
    {
        public bool Escaping { get; set; }
        public bool PregnancyRisk { get; set; }
        public bool ShouldEquip { get; set; }
        public bool ShouldUndress { get; set; }
    }
}