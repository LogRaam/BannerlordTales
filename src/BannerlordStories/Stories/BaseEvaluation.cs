// Code written by Gabriel Mailhot, 11/09/2020.

#region

using TalesContract;
using TalesEnums;

#endregion

namespace TalesBase.Stories
{
    #region

    #endregion

    public class BaseEvaluation : IEvaluation
    {
        public Attributes? Attribute { get; set; } = Attributes.UNKNOWN;
        public Characteristics? Characteristic { get; set; } = Characteristics.UNKNOWN;
        public bool Escaping { get; set; }
        public Operator Operator { get; set; } = Operator.UNKNOWN;
        public PartyType PartyType { get; set; } = PartyType.UNKNOWN;
        public PersonalityTraits? PersonalityTrait { get; set; } = PersonalityTraits.UNKNOWN;
        public bool PregnancyRisk { get; set; }
        public int RandomEnd { get; set; }
        public int RandomStart { get; set; }
        public Skills? Skill { get; set; } = Skills.UNKNOWN;
        public Actor Subject { get; set; } = Actor.UNKNOWN;
        public GameTime Time { get; set; }
        public string Value { get; set; }
        public bool ValueIsPercentage { get; set; }
    }
}