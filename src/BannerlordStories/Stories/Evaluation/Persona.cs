// Code written by Gabriel Mailhot, 02/12/2023.

#region

using TalesContract;
using TalesEnums;

#endregion

namespace TalesBase.Stories.Evaluation
{
    public class Persona : IPersona
    {
        public Attributes? Attribute { get; set; } = Attributes.NotAssigned;
        public Characteristics? Characteristic { get; set; } = Characteristics.NotAssigned;
        public PersonalityTraits? PersonalityTrait { get; set; } = PersonalityTraits.NotAssigned;
        public Skills? Skill { get; set; } = Skills.NotAssigned;
        public Actor Subject { get; set; } = Actor.NotAssigned;
    }
}