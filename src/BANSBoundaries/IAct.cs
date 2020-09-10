// Code written by Gabriel Mailhot, 02/08/2020.

#region

using System.Collections.Generic;
using TalesEnums;

#endregion

namespace TalesContract
{
   public interface IAct
   {
      List<IChoice> Choices { get; set; }

      public string Id { get; set; }

      string Image { get; set; }

      string Intro { get; set; }

      Location Location { get; set; }

      string Name { get; set; }

      public List<IEvaluation> Restrictions { get; set; }
   }
}