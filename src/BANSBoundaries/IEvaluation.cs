// Code written by Gabriel Mailhot, 11/09/2020.

#region

using TalesEnums;

#endregion

namespace TalesContract
{
   public interface IEvaluation
   {
      public Attributes? Attribute { get; set; }
      public Characteristics? Characteristic { get; set; }

      public Operator Operator { get; set; }

      public PersonalityTraits? PersonalityTrait { get; set; }
      public bool PregnancyRisk { get; set; }

      public int RandomEnd { get; set; }

      public int RandomStart { get; set; }

      public Skills? Skill { get; set; }
      public Actor Subject { get; set; }

      public GameTime Time { get; set; }

      public string Value { get; set; }

      public bool ValueIsPercentage { get; set; }
   }
}