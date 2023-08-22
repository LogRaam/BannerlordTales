// Code written by Gabriel Mailhot, 02/12/2023.  Updated by  Gabriel Mailhot on 02/19/2023.

#region

using FluentAssertions.Execution;
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

        public void IsEquivalentTo(IPersona persona)
        {
            if (persona.PersonalityTrait != PersonalityTrait) throw new AssertionFailedException("Evaluation expected persona.PersonalityTrait to be equivalent to " + persona.PersonalityTrait + ", but found that its value is " + PersonalityTrait);
            if (persona.Characteristic != Characteristic) throw new AssertionFailedException("Evaluation expected persona.Characteristic to be equivalent to " + persona.Characteristic + ", but found that its value is " + persona.Characteristic);
            if (persona.Subject != Subject) throw new AssertionFailedException("Evaluation expected persona.Subject to be equivalent to " + persona.Subject + ", but found that its value is " + persona.Subject);
            if (persona.Attribute != Attribute) throw new AssertionFailedException("Evaluation expected persona.Attribute to be equivalent to " + persona.Attribute + ", but found that its value is " + persona.Attribute);
            if (persona.Skill != Skill) throw new AssertionFailedException("Evaluation expected persona.Skill to be equivalent to " + persona.Skill + ", but found that its value is " + persona.Skill);
        }
    }
}