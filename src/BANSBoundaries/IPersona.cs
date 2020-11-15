// Code written by Gabriel Mailhot, 14/11/2020.

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
    }
}