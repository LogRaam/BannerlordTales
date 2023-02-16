// Code written by Gabriel Mailhot, 11/13/2022.

#region

using TalesEnums;

#endregion

namespace TalesContract
{
    #region

    #endregion

    public interface IProvocation
    {
        public IHero Actor { get; set; }

        public ICampaignTime ProvocationTime { get; set; }

        public ProvocationType ProvocationType { get; set; }

        public IKingdom ProvocatorFaction { get; set; }
    }
}