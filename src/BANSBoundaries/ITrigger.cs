// Code written by Gabriel Mailhot, 02/08/2020.

namespace TalesContract
{
  public interface ITrigger
    {
     int ChanceToTrigger { get; set; }

     string Link { get; set; }
  }
}