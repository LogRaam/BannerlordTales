// Code written by Gabriel Mailhot, 15/09/2020.

#region

using System.Collections.Generic;
using TalesContract;
using TalesEnums;
using TaleWorlds.CampaignSystem;
using IFaction = TalesContract.IFaction;

#endregion

namespace TalesBase.TW
{
    public class BasePartyBase : IPartyBase
    {
        public BasePartyBase(PartyBase party)
        {
            if (party == null) return;

            Id = party.Id;
            Name = party.Name.ToString();
        }

        public BasePartyBase()
        {
        }

        public IBasicCultureObject BasicCulture { get; set; }
        public float CavalryStrength { get; set; }
        public ICultureObject Culture { get; set; }
        public string Id { get; set; }
        public int Index { get; set; }
        public float InfantryStrength { get; set; }
        public int InventoryCapacity { get; set; }
        public bool IsActive { get; set; }
        public bool IsMobile { get; set; }
        public bool IsSettlement { get; set; }
        public bool IsStarving { get; set; }
        public bool IsValid { get; set; }
        public bool IsVisible { get; set; }
        public ICharacterObject Leader { get; set; }
        public IHero LeaderHero { get; set; }
        public IPartyBase MainParty { get; set; }
        public IFaction MapFaction { get; set; }
        public ITroopRoster MemberRoster { get; set; }
        public IMobileParty MobileParty { get; set; }
        public string Name { get; set; }
        public int NumberOfAllMembers { get; set; }
        public int NumberOfHealthyMembers { get; set; }
        public int NumberOfMenWithHorse { get; set; }
        public int NumberOfMenWithoutHorse { get; set; }
        public int NumberOfMounts { get; set; }
        public int NumberOfPackAnimals { get; set; }
        public int NumberOfPrisoners { get; set; }
        public int NumberOfRegularMembers { get; set; }
        public int NumberOfWoundedRegularMembers { get; set; }
        public BattleSide OpponentSide { get; set; }
        public IHero Owner { get; set; }
        public int PartySizeLimit { get; set; }
        public IList<ICharacterObject> PrisonerHeroes { get; set; }
        public int PrisonerSizeLimit { get; set; }
        public ITroopRoster PrisonRoster { get; set; }
        public float RangedStrength { get; set; }
        public int RemainingFoodPercentage { get; set; }
        public ISettlement Settlement { get; set; }
        public BattleSide Side { get; set; }
        public float Strength { get; set; }
        public int TacticsSkillAmount { get; set; }
        public float TotalStrength { get; set; }
    }
}