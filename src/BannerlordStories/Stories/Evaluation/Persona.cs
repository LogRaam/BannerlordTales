// Code written by Gabriel Mailhot, 14/11/2020.

#region

using TalesContract;
using TalesEnums;

#endregion

namespace TalesBase.Stories.Evaluation
{
    public class Persona : IPersona
    {
        public Attributes? Attribute { get; set; } = Attributes.UNKNOWN;
        public Characteristics? Characteristic { get; set; } = Characteristics.UNKNOWN;
        public PersonalityTraits? PersonalityTrait { get; set; } = PersonalityTraits.UNKNOWN;
        public Skills? Skill { get; set; } = Skills.UNKNOWN;
        public Actor Subject { get; set; } = Actor.UNKNOWN;
    }
}