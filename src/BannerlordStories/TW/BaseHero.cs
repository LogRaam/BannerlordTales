// Code written by Gabriel Mailhot, 02/12/2023.

#region

using _45_TalesGameState;
using System;
using System.Collections.Generic;
using System.Linq;
using TalesContract;
using TalesEnums;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;
using CharacterSkills = TalesContract.CharacterSkills;
using IFaction = TalesContract.IFaction;

#endregion

namespace TalesBase.TW
{
    #region

    #endregion

    public class BaseHero : IHero
    {
        private float _age;
        private bool _alwaysUnconscious;
        private int _athletics;
        private bool _awaitingTrial;
        private ICampaignTime _birthDay;
        private ISettlement _bornSettlement;
        private int _bow;
        private bool _canBecomePrisoner;
        private bool _canBeCompanion;
        private bool _canHaveRecruits;
        private ICampaignTime _captivityStartTime;
        private ICharacterObject _characterObject;
        private int _charm;
        private IList<IHero> _children = new List<IHero>();
        private IClan _clan;
        private IClan _companionOf;
        private IList<IHero> _companionsInParty;
        private int _control;
        private float _controversy;
        private int _crafting;
        private int _crossbow;
        private IBasicCultureObject _culture;
        private int _cunning;
        private ISettlement _currentSettlement;
        private ICampaignTime _deathDay;
        private IHero _deathMarkKillerHero;
        private bool _detected;
        private int _endurance;
        private int _engineering;
        private IList<IHero> _exSpouses;
        private IHero _father;
        private string _firstName;
        private int _gold;
        private ITown _governorOf;
        private bool _hasMet;
        private CharacterSkills _heroSkills;
        private CharacterStates _heroState;
        private ICharacterTraits _heroTraits;
        private int _hitPoints;
        private ISettlement _homeSettlement;
        private int _intelligence;
        private bool _isActive;
        private bool _isAlive;
        private bool _isArtisan;
        private bool _isChild;
        private bool _isCommander;
        private bool _isDead;
        private bool _isDisabled;
        private bool _isFactionLeader;
        private bool _isFemale;
        private bool _isFertile;
        private bool _isFugitive;
        private bool _isGangLeader;
        private bool _isHeadman;
        private bool _isHealthFull;
        private bool _isHumanPlayerCharacter;
        private bool _isMainHeroIll;
        private bool _isMercenary;
        private bool _isMerchant;
        private bool _isMinorFactionHero;
        private bool _isNoble;
        private bool _isNotable;
        private bool _isNotSpawned;
        private bool _isOccupiedByAnEvent;
        private bool _isOutlaw;
        private bool _isPartyLeader;
        private bool _isPlayerCompanion;
        private bool _isPreacher;
        private bool _isPregnant;
        private bool _isPrisoner;
        private bool _isRebel;
        private bool _isReleased;
        private bool _isRuralNotable;
        private bool _isSpecial;
        private bool _isWanderer;
        private bool _isWounded;
        private ICampaignTime _lastMeetingTimeWithPlayer;
        private bool _lastSeenInSettlement;
        private ISettlement _lastSeenPlace;
        private ICampaignTime _lastSeenTime;
        private int _leadership;
        private int _level;
        private IFaction _mapFaction;
        private int _maxHitPoints;
        private int _medecine;
        private int _medicine;
        private IHero _mother;
        private string _name;
        private bool _neverBecomePrisoner;
        private bool _noncombatant;
        private int _oneHanded;
        private IMobileParty _partyBelongedTo;
        private IPartyBase _partyBelongedToAsPrisoner;
        private float _passedTimeAtHomeSettlement;
        private int _polearm;
        private float _power;
        private float _probabilityOfDeath;
        private FactionRank _rank;
        private float _relationScoreWithPlayer;
        private int _riding;
        private int _roguery;
        private int _scouting;
        private IList<IHero> _siblings;
        private int _social;
        private IHero _spouse;
        private ISettlement _stayingInSettlementOfNotable;
        private int _steward;
        private IClan _supporterOf;
        private int _tactics;
        private int _throwing;
        private int _trade;
        private int _twoHanded;
        private float _unmodifiedClanLeaderRelationshipScoreWithPlayer;
        private int _vigor;
        private float? _weight;


        public BaseHero(Hero hero)
        {
            if (hero == null) return;

            Id = hero.Id;
            _name = hero.Name?.ToString();
            _firstName = hero.FirstName?.ToString();
            _isHumanPlayerCharacter = hero.IsHumanPlayerCharacter;
        }

        public BaseHero() { }

        public BaseHero(BasicCharacterObject character)
        {
            if (character == null) return;

            Id = character.Id;
            _name = character.Name?.ToString();
            _isHumanPlayerCharacter = character.IsPlayerCharacter;
        }

        public float Age
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _age = Origin.Age;

                return _age;
            }
            set => _age = value;
        }


        public int Athletics
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _riding = Origin.GetSkillValue(new SkillObject(Skills.Athletics.ToString()));

                return _athletics;
            }
            set => _athletics = value;
        }


        public ICampaignTime BirthDay
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _birthDay = new BaseCampaignTime(Origin.BirthDay);

                return _birthDay;
            }
            set => _birthDay = value;
        }

        public ISettlement BornSettlement
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _bornSettlement = new BaseSettlement(Origin.BornSettlement);

                return _bornSettlement;
            }
            set => _bornSettlement = value;
        }

        public int Bow
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _bow = Origin.GetSkillValue(new SkillObject(Skills.Bow.ToString()));

                return _bow;
            }
            set => _bow = value;
        }

        public int Calculating { get; set; }

        public bool CanBecomePrisoner
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _canBecomePrisoner = Origin.CanBecomePrisoner();

                return _canBecomePrisoner;
            }
            set => _canBecomePrisoner = value;
        }


        public bool CanBeCompanion
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _canBeCompanion = Origin.CanBeCompanion;

                return _canBeCompanion;
            }
            set => _canBeCompanion = value;
        }

        public bool CanHaveRecruits
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _canHaveRecruits = Origin.CanHaveRecruits;

                return _canHaveRecruits;
            }
            set => _canHaveRecruits = value;
        }

        public ICampaignTime CaptivityStartTime
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _captivityStartTime = new BaseCampaignTime(Origin.CaptivityStartTime);

                return _captivityStartTime;
            }
            set => _captivityStartTime = value;
        }

        public ICharacterObject CharacterObject
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _characterObject = new BaseCharacterObject(Origin.CharacterObject);

                return _characterObject;
            }
            set => _characterObject = value;
        }

        public int Charm
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _charm = Origin.GetSkillValue(new SkillObject(Skills.Charm.ToString()));

                return _charm;
            }
            set => _charm = value;
        }

        public IList<IHero> Children
        {
            get
            {
                if (CampaignState.CurrentGameStarted())
                {
                    _children = new List<IHero>();
                    foreach (var child in Origin.Children) _children.Add(new BaseHero(child));
                }

                return _children;
            }
            set => _children = value;
        }

        public IClan Clan
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _clan = new BaseClan(Origin.Clan);

                return _clan;
            }
            set => _clan = value;
        }

        public IClan CompanionOf
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _companionOf = new BaseClan(Origin.CompanionOf);

                return _companionOf;
            }
            set => _companionOf = value;
        }

        public IList<IHero> CompanionsInParty
        {
            get
            {
                if (CampaignState.CurrentGameStarted())
                {
                    _companionsInParty = new List<IHero>();
                    foreach (var hero in Origin.CompanionsInParty) _companionsInParty.Add(new BaseHero(hero));
                }

                return _companionsInParty;
            }
            set => _companionsInParty = value;
        }

        public int Control
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _control = Origin.GetAttributeValue(new CharacterAttribute(CharacterAttributesEnum.Control.ToString()));

                return _control;
            }
            set => _control = value;
        }


        public int Crafting
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _crafting = Origin.GetSkillValue(new SkillObject(Skills.Crafting.ToString()));

                return _crafting;
            }
            set => _crafting = value;
        }

        public int Crossbow
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _crossbow = Origin.GetSkillValue(new SkillObject(Skills.Crossbow.ToString()));

                return _crossbow;
            }
            set => _crossbow = value;
        }

        public IBasicCultureObject Culture
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _culture = new BaseBasicCultureObject(Origin.Culture);

                return _culture;
            }
            set => _culture = value;
        }

        public int Cunning
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _cunning = Origin.GetAttributeValue(new CharacterAttribute(CharacterAttributesEnum.Cunning.ToString()));

                return _cunning;
            }
            set => _cunning = value;
        }

        public ISettlement CurrentSettlement
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _currentSettlement = new BaseSettlement(Origin.CurrentSettlement);

                return _currentSettlement;
            }
            set => _currentSettlement = value;
        }

        public ICampaignTime DeathDay
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _deathDay = new BaseCampaignTime(Origin.DeathDay);

                return _deathDay;
            }
            set => _deathDay = value;
        }

        public IHero DeathMarkKillerHero
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _deathMarkKillerHero = new BaseHero(Origin.DeathMarkKillerHero);

                return _deathMarkKillerHero;
            }
            set => _deathMarkKillerHero = value;
        }


        public int Endurance
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _endurance = Origin.GetAttributeValue(new CharacterAttribute(CharacterAttributesEnum.Endurance.ToString()));

                return _endurance;
            }
            set => _endurance = value;
        }

        public int Engineering
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _engineering = Origin.GetSkillValue(new SkillObject(Skills.Engineering.ToString()));

                return _engineering;
            }
            set => _engineering = value;
        }

        public IEquipments Equipment { get; set; }

        public List<IEquipments> Equipments { get; set; }

        public IList<IHero> ExSpouses
        {
            get
            {
                if (CampaignState.CurrentGameStarted())
                {
                    _exSpouses = new List<IHero>();
                    foreach (var spouse in Origin.ExSpouses) _exSpouses.Add(new BaseHero(spouse));
                }

                return _exSpouses;
            }
            set => _exSpouses = value;
        }

        public IHero Father
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _father = new BaseHero(Origin.Father);

                return _father;
            }
            set => _father = value;
        }

        public string FirstName
        {
            get
            {
                if (CampaignState.CurrentGameStarted())
                    if (Origin.FirstName != null)
                        _firstName = Origin.FirstName.ToString();

                return _firstName;
            }
            set => _firstName = value;
        }

        public int Generosity { get; set; }

        public int Gold
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _gold = Origin.Gold;

                return _gold;
            }

            set => _gold = value;
        }


        public bool HasMet
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _hasMet = Origin.HasMet;

                return _hasMet;
            }
            set => _hasMet = value;
        }

        public CharacterSkills HeroSkills
        {
            get
            {
                if (CampaignState.CurrentGameStarted())
                {
                    _heroSkills = new BaseCharacterSkills
                    {
                        Athletics = Athletics,
                        Bow = Bow,
                        Charm = Charm,
                        Crafting = Crafting,
                        Crossbow = Crossbow,
                        Engineering = Engineering,
                        Leadership = Leadership,
                        Medicine = Medicine,
                        Onehanded = OneHanded,
                        Polearm = Polearm,
                        Riding = Riding,
                        Roguery = Roguery,
                        Scouting = Scouting,
                        Steward = Steward,
                        Tactics = Tactics,
                        Throwing = Throwing,
                        Trade = Trade,
                        Twohanded = TwoHanded
                    };
                }

                return _heroSkills;
            }
            set => _heroSkills = value;
        }


        public CharacterStates HeroState
        {
            get
            {
                if (CampaignState.CurrentGameStarted())
                {
                    Enum.TryParse(Origin.HeroState.ToString(), true, out CharacterStates s);
                    _heroState = s;
                }

                return _heroState;
            }
            set => _heroState = value;
        }

        public ICharacterTraits HeroTraits
        {
            get
            {
                if (CampaignState.CurrentGameStarted())
                {
                    _heroTraits = new BaseCharacterTraits(Origin.GetHeroTraits())
                    {
                        Calculating = Calculating,
                        Generosity = Generosity,
                        Honor = Honor,
                        Mercy = Mercy,
                        Valor = Valor
                    };
                }

                return _heroTraits;
            }
            set => _heroTraits = value;
        }


        public int HitPoints
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _hitPoints = Origin.HitPoints;

                return _hitPoints;
            }
            set => _hitPoints = value;
        }

        public ISettlement HomeSettlement
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _homeSettlement = new BaseSettlement(Origin.HomeSettlement);

                return _homeSettlement;
            }
            set => _homeSettlement = value;
        }

        public int Honor { get; set; }

        public MBGUID Id { get; set; }

        public int Intelligence
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _intelligence = Origin.GetAttributeValue(new CharacterAttribute(CharacterAttributesEnum.Intelligence.ToString()));

                return _intelligence;
            }
            set => _intelligence = value;
        }

        public bool IsActive
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isActive = Origin.IsActive;

                return _isActive;
            }
            set => _isActive = value;
        }

        public bool IsAlive
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isAlive = Origin.IsAlive;

                return _isAlive;
            }
            set => _isAlive = value;
        }

        public bool IsArtisan
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isArtisan = Origin.IsArtisan;

                return _isArtisan;
            }
            set => _isArtisan = value;
        }

        public bool IsChild
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isChild = Origin.IsChild;

                return _isChild;
            }
            set => _isChild = value;
        }

        public bool IsCommander
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isCommander = Origin.IsCommander;

                return _isCommander;
            }
            set => _isCommander = value;
        }

        public bool IsDead
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isDead = Origin.IsDead;

                return _isDead;
            }
            set => _isDead = value;
        }

        public bool IsDisabled
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isDisabled = Origin.IsDisabled;

                return _isDisabled;
            }
            set => _isDisabled = value;
        }

        public bool IsFactionLeader
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isFactionLeader = Origin.IsFactionLeader;

                return _isFactionLeader;
            }
            set => _isFactionLeader = value;
        }

        public bool IsFemale
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isFemale = Origin.IsFemale;

                return _isFemale;
            }
            set => _isFemale = value;
        }


        public bool IsFugitive
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isFugitive = Origin.IsFugitive;

                return _isFugitive;
            }
            set => _isFugitive = value;
        }

        public bool IsGangLeader
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isGangLeader = Origin.IsGangLeader;

                return _isGangLeader;
            }
            set => _isGangLeader = value;
        }

        public bool IsHeadman
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isHeadman = Origin.IsHeadman;

                return _isHeadman;
            }
            set => _isHeadman = value;
        }

        public bool IsHealthFull
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isHealthFull = Origin.IsHealthFull();

                return _isHealthFull;
            }
            set => _isHealthFull = value;
        }

        public bool IsHumanPlayerCharacter
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isHumanPlayerCharacter = Origin.IsHumanPlayerCharacter;

                return _isHumanPlayerCharacter;
            }
            set => _isHumanPlayerCharacter = value;
        }

        public bool IsMainHeroIll
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isMainHeroIll = Hero.IsMainHeroIll;

                return _isMainHeroIll;
            }
            set => _isMainHeroIll = value;
        }


        public bool IsMerchant
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isMerchant = Origin.IsMerchant;

                return _isMerchant;
            }
            set => _isMerchant = value;
        }

        public bool IsMinorFactionHero
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isMinorFactionHero = Origin.IsMinorFactionHero;

                return _isMinorFactionHero;
            }
            set => _isMinorFactionHero = value;
        }


        public bool IsNotable
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isNotable = Origin.IsNotable;

                return _isNotable;
            }
            set => _isNotable = value;
        }

        public bool IsNotSpawned
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isNotSpawned = Origin.IsNotSpawned;

                return _isNotSpawned;
            }
            set => _isNotSpawned = value;
        }


        public bool IsPartyLeader
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isPartyLeader = Origin.IsPartyLeader;

                return _isPartyLeader;
            }
            set => _isPartyLeader = value;
        }

        public bool IsPlayerCompanion
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isPlayerCompanion = Origin.IsPlayerCompanion;

                return _isPlayerCompanion;
            }
            set => _isPlayerCompanion = value;
        }

        public bool IsPreacher
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isPreacher = Origin.IsPreacher;

                return _isPreacher;
            }
            set => _isPreacher = value;
        }

        public bool IsPregnant
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isPregnant = Origin.IsPregnant;

                return _isPregnant;
            }
            set => _isPregnant = value;
        }

        public bool IsPrisoner
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isPrisoner = Origin.IsPrisoner;

                return _isPrisoner;
            }
            set => _isPrisoner = value;
        }

        public bool IsRebel
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isRebel = Origin.IsRebel;

                return _isRebel;
            }
            set => _isRebel = value;
        }

        public bool IsReleased
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isReleased = Origin.IsReleased;

                return _isReleased;
            }
            set => _isReleased = value;
        }

        public bool IsRuralNotable
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isRuralNotable = Origin.IsRuralNotable;

                return _isRuralNotable;
            }
            set => _isRuralNotable = value;
        }

        public bool IsSpecial
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isSpecial = Origin.IsSpecial;

                return _isSpecial;
            }
            set => _isSpecial = value;
        }


        public bool IsWanderer
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isWanderer = Origin.IsWanderer;

                return _isWanderer;
            }
            set => _isWanderer = value;
        }

        public bool IsWounded
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isWounded = Origin.IsWounded;

                return _isWounded;
            }
            set => _isWounded = value;
        }

        public ICampaignTime LastMeetingTimeWithPlayer
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _lastMeetingTimeWithPlayer = new BaseCampaignTime(Origin.LastMeetingTimeWithPlayer);

                return _lastMeetingTimeWithPlayer;
            }
            set => _lastMeetingTimeWithPlayer = value;
        }


        public int Leadership
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _leadership = Origin.GetSkillValue(new SkillObject(Skills.Leadership.ToString()));

                return _leadership;
            }
            set => _leadership = value;
        }

        public int Level
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _level = Origin.Level;

                return _level;
            }
            set => _level = value;
        }

        public IFaction MapFaction
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _mapFaction = new BaseFaction(Origin.MapFaction);

                return _mapFaction;
            }
            set => _mapFaction = value;
        }

        public int MaxHitPoints
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _maxHitPoints = Origin.MaxHitPoints;

                return _maxHitPoints;
            }
            set => _maxHitPoints = value;
        }

        public int Medicine
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _medicine = Origin.GetSkillValue(new SkillObject(Skills.Medicine.ToString()));

                return _medicine;
            }
            set => _medicine = value;
        }

        public int Mercy { get; set; }

        public IHero Mother
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _mother = new BaseHero(Origin.Mother);

                return _mother;
            }
            set => _mother = value;
        }

        public string Name
        {
            get
            {
                if (CampaignState.CurrentGameStarted())
                    if (Origin.Name != null)
                        _name = Origin.Name.ToString();

                return _name;
            }
            set => _name = value;
        }


        public int OneHanded
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _oneHanded = Origin.GetSkillValue(new SkillObject(Skills.OneHanded.ToString()));

                return _oneHanded;
            }
            set => _oneHanded = value;
        }


        public float PassedTimeAtHomeSettlement
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _passedTimeAtHomeSettlement = Origin.PassedTimeAtHomeSettlement;

                return _passedTimeAtHomeSettlement;
            }
            set => _passedTimeAtHomeSettlement = value;
        }

        public int Polearm
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _polearm = Origin.GetSkillValue(new SkillObject(Skills.Polearm.ToString()));

                return _polearm;
            }
            set => _polearm = value;
        }

        public float Power
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _power = Origin.Power;

                return _power;
            }
            set => _power = value;
        }

        public float ProbabilityOfDeath
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _probabilityOfDeath = Origin.ProbabilityOfDeath;

                return _probabilityOfDeath;
            }
            set => _probabilityOfDeath = value;
        }


        public float RelationScoreWithPlayer
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _relationScoreWithPlayer = Origin.GetRelationWithPlayer();

                return _relationScoreWithPlayer;
            }
            set => _relationScoreWithPlayer = value;
        }

        public int Riding
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _riding = Origin.GetSkillValue(new SkillObject(Skills.Riding.ToString()));

                return _riding;
            }
            set => _riding = value;
        }

        public int Roguery
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _roguery = Origin.GetSkillValue(new SkillObject(Skills.Roguery.ToString()));

                return _roguery;
            }
            set => _roguery = value;
        }

        public int Scouting
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _scouting = Origin.GetSkillValue(new SkillObject(Skills.Scouting.ToString()));

                return _scouting;
            }
            set => _scouting = value;
        }

        public IList<IHero> Siblings
        {
            get
            {
                if (CampaignState.CurrentGameStarted())
                {
                    _siblings = new List<IHero>();
                    foreach (var child in Origin.Siblings) _siblings.Add(new BaseHero(child));
                }

                return _siblings;
            }
            set => _siblings = value;
        }

        public int Social
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _social = Origin.GetAttributeValue(new CharacterAttribute(CharacterAttributesEnum.Social.ToString()));

                return _social;
            }
            set => _social = value;
        }

        public IHero Spouse
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _spouse = new BaseHero(Origin.Spouse);

                return _spouse;
            }
            set => _spouse = value;
        }


        public int Steward
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _steward = Origin.GetSkillValue(new SkillObject(Skills.Steward.ToString()));

                return _steward;
            }
            set => _steward = value;
        }

        public IClan SupporterOf
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _supporterOf = new BaseClan(Origin.SupporterOf);

                return _supporterOf;
            }
            set => _supporterOf = value;
        }

        public int Tactics
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _tactics = Origin.GetSkillValue(new SkillObject(Skills.Tactics.ToString()));

                return _tactics;
            }
            set => _tactics = value;
        }

        public int Throwing
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _throwing = Origin.GetSkillValue(new SkillObject(Skills.Throwing.ToString()));

                return _throwing;
            }
            set => _throwing = value;
        }

        public int Trade
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _trade = Origin.GetSkillValue(new SkillObject(Skills.Trade.ToString()));

                return _trade;
            }
            set => _trade = value;
        }


        public int TwoHanded
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _twoHanded = Origin.GetSkillValue(new SkillObject(Skills.TwoHanded.ToString()));

                return _twoHanded;
            }
            set => _twoHanded = value;
        }

        public float UnmodifiedClanLeaderRelationshipScoreWithPlayer
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _unmodifiedClanLeaderRelationshipScoreWithPlayer = Origin.GetUnmodifiedClanLeaderRelationshipWithPlayer();

                return _unmodifiedClanLeaderRelationshipScoreWithPlayer;
            }
            set => _unmodifiedClanLeaderRelationshipScoreWithPlayer = value;
        }

        public int Valor { get; set; }

        public int Vigor
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _vigor = Origin.GetAttributeValue(new CharacterAttribute(CharacterAttributesEnum.Vigor.ToString()));

                return _vigor;
            }
            set => _vigor = value;
        }

        public float? Weight
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _weight = Origin.Weight;

                return _weight;
            }
            set => _weight = value;
        }

        private Hero Origin
        {
            get
            {
                var t = Campaign.Current.AliveHeroes.FirstOrDefault(n => n.Id == Id);
                if (t == null)
                {
                    t = _isHumanPlayerCharacter
                        ? Campaign.Current.AliveHeroes.First(n => n.IsHumanPlayerCharacter)
                        : Campaign.Current.AliveHeroes.First(n => n.IsPartyLeader);
                    Id = t.Id;
                }

                return t;
            }
        }
    }
}