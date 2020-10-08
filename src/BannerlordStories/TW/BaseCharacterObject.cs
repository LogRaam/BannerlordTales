// Code written by Gabriel Mailhot, 11/09/2020.

#region

using System;
using System.Collections.Generic;
using _45_TalesGameState;
using TalesContract;
using TaleWorlds.CampaignSystem;
using Occupation = TalesEnums.Occupation;

#endregion

namespace TalesEntities.TW
{
    #region

    #endregion

    public class BaseCharacterObject : ICharacterObject
    {
        private float _age;
        private List<ICharacterObject> _all = new List<ICharacterObject>();
        private int _conformityNeededToRecruitPrisoner;
        private IList<ICharacterObject> _conversationCharacters = new List<ICharacterObject>();
        private ICultureObject _culture;
        private string _encyclopediaLink;
        private string _encyclopediaLinkWithName;
        private string _hairTags;
        private IHero _heroObject;
        private int _hitPoints;
        private bool _isArcher;
        private bool _isBasicTroop;
        private bool _isChildTemplate;
        private bool _isFemale;
        private bool _isHero;
        private bool _isInfantry;
        private bool _isMounted;
        private bool _isNotTransferable;
        private bool _isOriginalCharacter;
        private bool _isPlayerCharacter;
        private bool _isRegular;
        private bool _isTemplate;
        private int _level;
        private string _name;
        private Occupation _occupation;
        private ICharacterObject _oneToOneConversationCharacter;
        private ICharacterObject _playerCharacter;
        private string _tattooTags;
        private ICharacterObject _templateCharacter;
        private IList<BaseCharacterObject> _templates = new List<BaseCharacterObject>();
        private int _tier;
        private int _troopWage;
        private int _upgradeXpCost;

        public BaseCharacterObject(CharacterObject character)
        {
            throw new NotImplementedException();
        }


        public BaseCharacterObject()
        {
        }

        public BaseCharacterObject(IHero hero)
        {
            HeroObject = hero;
            IsPlayerCharacter = hero.IsHumanPlayerCharacter;
            Name = hero.Name;
        }

        public float Age
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _age = Origin().Age;

                return _age;
            }
            set => _age = value;
        }

        public List<ICharacterObject> All
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _all = GetAll();

                return _all;
            }
            set => _all = value;
        }

        public int ConformityNeededToRecruitPrisoner
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _conformityNeededToRecruitPrisoner = Origin().ConformityNeededToRecruitPrisoner;

                return _conformityNeededToRecruitPrisoner;
            }
            set => _conformityNeededToRecruitPrisoner = value;
        }

        public IList<ICharacterObject> ConversationCharacters
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _conversationCharacters = GetConversationCharacters();

                return _conversationCharacters;
            }
            set => _conversationCharacters = value;
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

        public string HairTags
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _hairTags = Origin().HairTags;

                return _hairTags;
            }
            set => _hairTags = value;
        }

        public IHero HeroObject
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _heroObject = new BaseHero(Origin().HeroObject);

                return _heroObject;
            }
            set => _heroObject = value;
        }

        public int HitPoints
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _hitPoints = Origin().HitPoints;

                return _hitPoints;
            }
            set => _hitPoints = value;
        }

        public bool IsArcher
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isArcher = Origin().IsArcher;

                return _isArcher;
            }
            set => _isArcher = value;
        }

        public bool IsBasicTroop
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isBasicTroop = Origin().IsBasicTroop;

                return _isBasicTroop;
            }
            set => _isBasicTroop = value;
        }

        public bool IsChildTemplate
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isChildTemplate = Origin().IsChildTemplate;

                return _isChildTemplate;
            }
            set => _isChildTemplate = value;
        }

        public bool IsFemale
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isFemale = Origin().IsFemale;

                return _isFemale;
            }
            set => _isFemale = value;
        }

        public bool IsHero
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isHero = Origin().IsHero;

                return _isHero;
            }
            set => _isHero = value;
        }

        public bool IsInfantry
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isInfantry = Origin().IsInfantry;

                return _isInfantry;
            }
            set => _isInfantry = value;
        }

        public bool IsMounted
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isMounted = Origin().IsMounted;

                return _isMounted;
            }
            set => _isMounted = value;
        }

        public bool IsNotTransferable
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isMounted = Origin().IsNotTransferable;

                return _isMounted;
            }
            set => _isNotTransferable = value;
        }

        public bool IsOriginalCharacter
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isOriginalCharacter = Origin().IsOriginalCharacter;

                return _isOriginalCharacter;
            }
            set => _isOriginalCharacter = value;
        }

        public bool IsPlayerCharacter
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isPlayerCharacter = Origin().IsPlayerCharacter;

                return _isPlayerCharacter;
            }
            set => _isPlayerCharacter = value;
        }

        public bool IsRegular
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isRegular = Origin().IsRegular;

                return _isRegular;
            }
            set => _isRegular = value;
        }

        public bool IsTemplate
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _isTemplate = Origin().IsTemplate;

                return _isTemplate;
            }
            set => _isTemplate = value;
        }

        public int Level
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _level = Origin().Level;

                return _level;
            }
            set => _level = value;
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

        public Occupation Occupation
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _occupation = GetOccupation();

                return _occupation;
            }
            set => _occupation = value;
        }

        public ICharacterObject OneToOneConversationCharacter
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _oneToOneConversationCharacter = new BaseCharacterObject(CharacterObject.OneToOneConversationCharacter);

                return _oneToOneConversationCharacter;
            }
            set => _oneToOneConversationCharacter = value;
        }

        public ICharacterObject PlayerCharacter
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _playerCharacter = new BaseCharacterObject(CharacterObject.PlayerCharacter);

                return _playerCharacter;
            }
            set => _playerCharacter = value;
        }

        public string TattooTags
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _tattooTags = Origin().TattooTags;

                return _tattooTags;
            }
            set => _tattooTags = value;
        }

        public ICharacterObject TemplateCharacter
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _templateCharacter = new BaseCharacterObject(Origin().TemplateCharacter);

                return _templateCharacter;
            }
            set => _templateCharacter = value;
        }

        public IList<BaseCharacterObject> Templates
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _templates = GetTemplates();

                return _templates;
            }
            set => _templates = value;
        }

        public int Tier
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _tier = Origin().Tier;

                return _tier;
            }
            set => _tier = value;
        }

        public int TroopWage
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _troopWage = Origin().TroopWage;

                return _troopWage;
            }
            set => _troopWage = value;
        }

        public ICharacterObject[] UpgradeTargets { get; set; }

        public int UpgradeXpCost
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _upgradeXpCost = Origin().UpgradeXpCost;

                return _upgradeXpCost;
            }
            set => _upgradeXpCost = value;
        }

        #region private

        private List<ICharacterObject> GetAll()
        {
            var result = new List<ICharacterObject>();
            var t = CharacterObject.All;
            foreach (var o in t) result.Add(new BaseCharacterObject(o));

            return result;
        }

        private IList<ICharacterObject> GetConversationCharacters()
        {
            var result = new List<ICharacterObject>();
            var t = CharacterObject.ConversationCharacters;
            foreach (var o in t) result.Add(new BaseCharacterObject(o));

            return result;
        }

        private Occupation GetOccupation()
        {
            Enum.TryParse(Origin().Occupation.ToString(), true, out Occupation result);

            return result;
        }

        private IList<BaseCharacterObject> GetTemplates()
        {
            throw new NotImplementedException();
        }

        private CharacterObject Origin()
        {
            return CharacterObject.FindFirst(n => n.IsPlayerCharacter == IsPlayerCharacter && n.Name.ToString() == Name);
        }

        #endregion
    }
}