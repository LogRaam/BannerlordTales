// Code written by Gabriel Mailhot, 02/09/2020.

#region

using System.Collections.Generic;
using TalesContract;
using TalesEnums;

#endregion

namespace TalesEntities.Stories
{
   #region

   #endregion

   public class BaseAct : IAct
   {
      public List<IChoice> Choices { get; set; } = new List<IChoice>();

      public string Id { get; set; }

      public string Image { get; set; }

      public string Intro { get; set; }

      public Location Location { get; set; } = Location.UNKNOWN;

      public string Name { get; set; }

      public List<IEvaluation> Restrictions { get; set; } = new List<IEvaluation>();
   }
}