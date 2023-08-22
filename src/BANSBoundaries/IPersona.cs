// Code written by Gabriel Mailhot, 02/12/2023.  Updated by  Gabriel Mailhot on 02/19/2023.

#region

using TalesEnums;

#endregion

namespace TalesContract
{
    public interface IPersona
    {
        public Attributes? Attribute { get; set; }
        public Characteristics? Characteristic { get; set; }
        public PersonalityTraits? PersonalityTrait { get; set; }
        public Skills? Skill { get; set; }
        public Actor Subject { get; set; }
        void IsEquivalentTo(IPersona persona);
    }
}