// Code written by Gabriel Mailhot, 02/12/2023.  Updated by  Gabriel Mailhot on 02/19/2023.

namespace TalesContract
{
    public interface IOutcome
    {
        public bool Escaping { get; set; }
        public bool PregnancyRisk { get; set; }
        public bool ShouldEquip { get; set; }
        public bool ShouldUndress { get; set; }
        void IsEquivalentTo(IOutcome outcome);
    }
}