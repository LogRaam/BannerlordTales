// Code written by Gabriel Mailhot, 05/09/2020.

#region

using System;
using System.Collections.Generic;
using TalesContract;
using TalesEnums;

#endregion

namespace TalesEntities.Stories
{
   public class Sequence : IAct
   {
      public List<IChoice> Choices { get; set; } = new List<IChoice>();
      public string Id { get; set; }
      public string Image { get; set; }
      public string Intro { get; set; }
      public Location Location { get; set; }

      public string Name { get; set; }
      public List<IEvaluation> Restrictions { get; set; } = new List<IEvaluation>();

      public bool IsQualified()
      {
         throw new NotImplementedException();
      }
   }
}