// Code written by Gabriel Mailhot, 14/11/2020.

namespace TalesContract
{
    public interface IOutcome
    {
        public bool Escaping { get; set; }
        public bool PregnancyRisk { get; set; }
        public bool ShouldEquip { get; set; }
        public bool ShouldUndress { get; set; }
    }
}