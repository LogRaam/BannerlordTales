// Code written by Gabriel Mailhot, 11/13/2022.  Updated by  Gabriel Mailhot on 02/19/2023.

namespace TalesContract
{
    public interface ITrigger
    {
        int ChanceToTrigger { get; set; }

        string Link { get; set; }
        bool IsEquivalentTo(ITrigger trigger);
    }
}