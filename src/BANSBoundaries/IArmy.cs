// Code written by Gabriel Mailhot, 02/09/2020.

namespace TalesContract
{
   #region

   using System.Collections.Generic;
   using TalesEnums;

   #endregion

   public interface IArmy
   {
      public IHero ArmyOwner { get; set; }

      public ArmyTypes ArmyType { get; set; }

      public float Cohesion { get; set; }

      public float CohesionChange { get; }

      public IKingdom Kingdom { get; set; }

      public IMobileParty LeaderParty { get; set; }

      public IList<IMobileParty> LeaderPartyAndAttachedParties { get; }

      public IKingdom MapFaction { get; }

      public float Morale { get; set; }

      public string Name { get; set; }

      public IList<IMobileParty> Parties { get; set; }

      public float TotalHealthyMembers { get; }

      public float TotalManCount { get; }

      public int TotalRegularCount { get; }

      public float TotalStrength { get; }
   }
}