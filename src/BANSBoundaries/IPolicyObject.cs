// Code written by Gabriel Mailhot, 02/09/2020.

namespace TalesContract
{
   public interface IPolicyObject
   {
      public float AuthoritarianWeight { get; set; }

      public float EgalitarianWeight { get; set; }

      public string LogEntryDescription { get; set; }

      public float OligarchicWeight { get; set; }

      public string SecondaryEffects { get; set; }
   }
}