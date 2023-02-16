// Code written by Gabriel Mailhot, 02/12/2023.

#region

using _45_TalesGameState;
using System;
using System.Collections.Generic;
using TalesContract;
using TalesEnums;
using TaleWorlds.CampaignSystem.Settlements;

#endregion

namespace TalesBase.TW
{
    public class BaseSettlement : ISettlement
    {
        private BattleSide _battleSide;
        private int _bribePaid;
        private int _canBeClaimed;
        private IHero _claimedBy;
        private float _claimValue;
        private ICultureObject _culture;
        private ISettlement _currentSettlement;
        private string _encyclopediaLink;
        private string _encyclopediaLinkWithName;
        private string _encyclopediaText;
        private int _getNumberOfAvailableRecruits;
        private bool _hasFestival;
        private bool _hasMultipleRecruits;
        private bool _hasRecruits;
        private bool _hasVisited;
        private List<IHero> _heroesWithoutParty;
        private bool _isActive;
        private bool _isAlerted;
        private bool _isBooming;
        private bool _isCastle;
        private bool _isFortification;
        private bool _isHideout;
        private bool _isInspected;
        private bool _isMinorFactionBase;
        private bool _isQuestSettlement;
        private bool _isRaided;
        private bool _isRebelling;
        private bool _isStarving;
        private bool _isTown;
        private bool _isUnderRaid;
        private bool _isUnderRebellionAttack;
        private bool _isUnderSiege;
        private bool _isVillage;
        private bool _isVisible;
        private float _lastVisitTimeOfOwner;
        private float _maxHitPointsOfOneWallSection;
        private float _maxWallHitPoints;
        private float _militia;
        private string _name;
        private float _numberOfAlliesSpottedAround;
        private float _numberOfEnemiesSpottedAround;
        private int _numberOfLordPartiesAt;
        private int _numberOfLordPartiesTargeting;
        private int _numberOfTroopsKilledOnSide;
        private IMobileParty _oldMilitiaParty;
        private IClan _ownerClan;
        private int _passedHoursAfterLastThreat;
        private float _prosperity;
        private float _settlementHitPoints;
        private bool _settlementTaken;
        private float _settlementTotalWallHitPoints;
        private ITown _town;
        private IVillage _village;
        private int _wallSectionCount;

        public BaseSettlement() { }

        public BaseSettlement(Settlement settlement)
        {
            if (settlement == null) return;

            _name = settlement.Name.ToString();
        }

        public BattleSide BattleSide
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _battleSide = (BattleSide)Enum.Parse(typeof(BattleSide), Origin().BattleSide.ToString().ToUpper());

                return _battleSide;
            }
            set => _battleSide = value;
        }


        public IList<IMobileParty> BoundParties { get; set; }
        public IList<IVillage> BoundVillages { get; set; }

        public int BribePaid
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _bribePaid = Origin().BribePaid;

                return _bribePaid;
            }
            set => _bribePaid = value;
        }

        public int CanBeClaimed
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _canBeClaimed = Origin().CanBeClaimed;

                return _canBeClaimed;
            }
            set => _canBeClaimed = value;
        }

        public IHero ClaimedBy
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _claimedBy = new BaseHero(Origin().ClaimedBy);

                return _claimedBy;
            }
            set => _claimedBy = value;
        }

        public float ClaimValue
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _claimValue = Origin().ClaimValue;

                return _claimValue;
            }
            set => _claimValue = value;
        }

        public ICultureObject Culture
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _culture = new BaseCultureObject(Origin().Culture);

                return _culture;
            }
            set => _culture = value;
        }

        public ISettlement CurrentSettlement
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _currentSettlement = new BaseSettlement(Settlement.CurrentSettlement);

                return _currentSettlement;
            }
            set => _currentSettlement = value;
        }

        public string EncyclopediaLink
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _encyclopediaLink = Origin().EncyclopediaLink;

                return _encyclopediaLink;
            }
            set => _encyclopediaLink = value;
        }

        public string EncyclopediaLinkWithName
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _encyclopediaLinkWithName = Origin().EncyclopediaLinkWithName.ToString();

                return _encyclopediaLinkWithName;
            }
            set => _encyclopediaLinkWithName = value;
        }

        public string EncyclopediaText
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _encyclopediaText = Origin().EncyclopediaText.ToString();

                return _encyclopediaText;
            }
            set => _encyclopediaText = value;
        }


        public bool HasVisited
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _hasVisited = Origin().HasVisited;

                return _hasVisited;
            }
            set => _hasVisited = value;
        }

        public List<IHero> HeroesWithoutParty
        {
            get
            {
                if (CampaignState.CurrentGameStarted())
                {
                    var r = new List<IHero>();
                    foreach (var hero in Origin().HeroesWithoutParty) r.Add(new BaseHero(hero));
                    _heroesWithoutParty = r;
                }

                return _heroesWithoutParty;
            }
            set => _heroesWithoutParty = value;
        }

        public IHideout Hideout { get; set; }

        public bool IsActive
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isActive = Origin().IsActive;

                return _isActive;
            }
            set => _isActive = value;
        }

        public bool IsAlerted
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isAlerted = Origin().IsAlerted;

                return _isAlerted;
            }
            set => _isAlerted = value;
        }

        public bool IsBooming
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isBooming = Origin().IsBooming;

                return _isBooming;
            }
            set => _isBooming = value;
        }

        public bool IsCastle
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isCastle = Origin().IsCastle;

                return _isCastle;
            }
            set => _isCastle = value;
        }

        public bool IsFortification
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isFortification = Origin().IsFortification;

                return _isFortification;
            }
            set => _isFortification = value;
        }

        public bool IsHideout
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isHideout = Origin().IsHideout;

                return _isHideout;
            }
            set => _isHideout = value;
        }

        public bool IsInspected
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isInspected = Origin().IsInspected;

                return _isInspected;
            }
            set => _isInspected = value;
        }


        public bool IsRaided
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isRaided = Origin().IsRaided;

                return _isRaided;
            }
            set => _isRaided = value;
        }


        public bool IsStarving
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isStarving = Origin().IsStarving;

                return _isStarving;
            }
            set => _isStarving = value;
        }

        public bool IsTown
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isTown = Origin().IsTown;

                return _isTown;
            }
            set => _isTown = value;
        }

        public bool IsUnderRaid
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isUnderRaid = Origin().IsUnderRaid;

                return _isUnderRaid;
            }
            set => _isUnderRaid = value;
        }

        public bool IsUnderRebellionAttack
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isUnderRebellionAttack = Origin().IsUnderRebellionAttack();

                return _isUnderRebellionAttack;
            }
            set => _isUnderRebellionAttack = value;
        }

        public bool IsUnderSiege
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isUnderSiege = Origin().IsUnderSiege;

                return _isUnderSiege;
            }
            set => _isUnderSiege = value;
        }

        public bool IsVillage
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isVillage = Origin().IsVillage;

                return _isVillage;
            }
            set => _isVillage = value;
        }

        public bool IsVisible
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isVisible = Origin().IsVisible;

                return _isVisible;
            }
            set => _isVisible = value;
        }

        public IMobileParty LastAttackerParty { get; set; }

        public float LastVisitTimeOfOwner
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _lastVisitTimeOfOwner = Origin().LastVisitTimeOfOwner;

                return _lastVisitTimeOfOwner;
            }
            set => _lastVisitTimeOfOwner = value;
        }

        public IFaction MapFaction { get; set; }

        public float MaxHitPointsOfOneWallSection
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _maxHitPointsOfOneWallSection = Origin().MaxHitPointsOfOneWallSection;

                return _maxHitPointsOfOneWallSection;
            }
            set => _maxHitPointsOfOneWallSection = value;
        }

        public float MaxWallHitPoints
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _maxWallHitPoints = Origin().MaxWallHitPoints;

                return _maxWallHitPoints;
            }
            set => _maxWallHitPoints = value;
        }

        public IMobileParty MilitaParty { get; set; }

        public float Militia
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _militia = Origin().Militia;

                return _militia;
            }
            set => _militia = value;
        }

        public string Name
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _name = Origin().Name.ToString();

                return _name;
            }
            set => _name = value;
        }

        public IList<IHero> Notables { get; set; }

        public float NumberOfAlliesSpottedAround
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _numberOfAlliesSpottedAround = Origin().NumberOfAlliesSpottedAround;

                return _numberOfAlliesSpottedAround;
            }
            set => _numberOfAlliesSpottedAround = value;
        }

        public float NumberOfEnemiesSpottedAround
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _numberOfEnemiesSpottedAround = Origin().NumberOfEnemiesSpottedAround;

                return _numberOfEnemiesSpottedAround;
            }
            set => _numberOfEnemiesSpottedAround = value;
        }

        public int NumberOfLordPartiesAt
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _numberOfLordPartiesAt = Origin().NumberOfLordPartiesAt;

                return _numberOfLordPartiesAt;
            }
            set => _numberOfLordPartiesAt = value;
        }

        public int NumberOfLordPartiesTargeting
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _numberOfLordPartiesTargeting = Origin().NumberOfLordPartiesTargeting;

                return _numberOfLordPartiesTargeting;
            }
            set => _numberOfLordPartiesTargeting = value;
        }

        public int NumberOfTroopsKilledOnSide
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _numberOfTroopsKilledOnSide = Origin().NumberOfTroopsKilledOnSide;

                return _numberOfTroopsKilledOnSide;
            }
            set => _numberOfTroopsKilledOnSide = value;
        }


        public IClan OwnerClan
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _ownerClan = new BaseClan(Origin().OwnerClan);

                return _ownerClan;
            }
            set => _ownerClan = value;
        }

        public IList<IMobileParty> Parties { get; set; }
        public IPartyBase Party { get; set; }

        public int PassedHoursAfterLastThreat
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _passedHoursAfterLastThreat = Origin().PassedHoursAfterLastThreat;

                return _passedHoursAfterLastThreat;
            }
            set => _passedHoursAfterLastThreat = value;
        }

        public float Prosperity
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _prosperity = Origin().Prosperity;

                return _prosperity;
            }
            set => _prosperity = value;
        }

        public float SettlementHitPoints
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _settlementHitPoints = Origin().Prosperity;

                return _settlementHitPoints;
            }
            set => _settlementHitPoints = value;
        }


        public float SettlementTotalWallHitPoints
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _settlementTotalWallHitPoints = Origin().SettlementTotalWallHitPoints;

                return _settlementTotalWallHitPoints;
            }
            set => _settlementTotalWallHitPoints = value;
        }

        public IList<float> SettlementWallSectionHitPointsRatioList { get; set; }
        public IList<IPartyBase> SiegeParties { get; set; }


        public int WallSectionCount
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _wallSectionCount = Origin().WallSectionCount;

                return _wallSectionCount;
            }
            set => _wallSectionCount = value;
        }

        #region private

        private Settlement Origin()
        {
            return Settlement.FindFirst(n => n.Name.ToString() == _name);
        }

        #endregion
    }
}