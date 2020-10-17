// Code written by Gabriel Mailhot, 11/09/2020.

#region

using System.Collections.Generic;
using TalesContract;
using TalesEnums;
using TaleWorlds.CampaignSystem;

#endregion

namespace TalesEntities.TW
{
    #region

    #endregion

    public class BaseCultureObject : ICultureObject
    {
        public BaseCultureObject(CultureObject culture)
        {
            //TODO
        }

        public BaseCultureObject()
        {
        }

        public ICharacterObject ArmedTrader { get; set; }

        public ICharacterObject Armorer { get; set; }

        public ICharacterObject ArtisanNotary { get; set; }

        public ICharacterObject BanditBandit { get; set; }

        public ICharacterObject BanditBoss { get; set; }

        public ICharacterObject BanditChief { get; set; }

        public ICharacterObject BanditRaider { get; set; }

        public ICharacterObject BasicTroop { get; set; }

        public ICharacterObject Beggar { get; set; }

        public ICharacterObject Blacksmith { get; set; }

        public BoardGameType BoardGame { get; set; }

        public string BodyPropertiesValue { get; set; }

        public ICharacterObject CaravanGuard { get; set; }

        public ICharacterObject CaravanMaster { get; set; }

        public IList<string> ClanNameList { get; set; }

        public ICharacterObject DuelPreset { get; set; }

        public ICharacterObject EliteBasicTroop { get; set; }

        public string EncyclopediaText { get; set; }

        public ICharacterObject FemaleBeggar { get; set; }

        public ICharacterObject FemaleDancer { get; set; }

        public IList<string> FemaleNameList { get; set; }

        public ICharacterObject GangleaderBodyguard { get; set; }

        public ICharacterObject GearDummy { get; set; }

        public ICharacterObject GearPracticeDummy { get; set; }

        public ICharacterObject Guard { get; set; }

        public ICharacterObject HorseMerchant { get; set; }

        public IList<string> MaleNameList { get; set; }

        public ICharacterObject MeleeEliteMilitiaTroop { get; set; }

        public ICharacterObject MeleeMilitiaTroop { get; set; }

        public ICharacterObject Merchant { get; set; }

        public ICharacterObject MerchantNotary { get; set; }

        public ICharacterObject MilitiaArcher { get; set; }

        public int MilitiaBonus { get; set; }

        public ICharacterObject MilitiaSpearman { get; set; }

        public ICharacterObject MilitiaVeteranArcher { get; set; }

        public ICharacterObject MilitiaVeteranSpearman { get; set; }

        public ICharacterObject Musician { get; set; }

        public ICharacterObject PreacherNotary { get; set; }

        public ICharacterObject PrisonGuard { get; set; }

        public int ProsperityBonus { get; set; }

        public ICharacterObject RangedEliteMilitiaTroop { get; set; }

        public ICharacterObject RangedMilitiaTroop { get; set; }

        public ICharacterObject RansomBroker { get; set; }

        public ICharacterObject RuralNotableNotary { get; set; }

        public ICharacterObject ShopWorker { get; set; }

        public ICharacterObject Steward { get; set; }

        public ICharacterObject TavernGamehost { get; set; }

        public ICharacterObject Tavernkeeper { get; set; }

        public ICharacterObject TavernWench { get; set; }

        public ICharacterObject TournamentMaster { get; set; }

        public int TownEdgeNumber { get; set; }

        public ICharacterObject Townsman { get; set; }

        public ICharacterObject TownsmanChild { get; set; }

        public ICharacterObject TownsmanInfant { get; set; }

        public ICharacterObject TownsmanTeenager { get; set; }

        public ICharacterObject Townswoman { get; set; }

        public ICharacterObject TownswomanChild { get; set; }

        public ICharacterObject TownswomanInfant { get; set; }

        public ICharacterObject TownswomanTeenager { get; set; }

        public ICharacterObject Villager { get; set; }

        public ICharacterObject VillagerFemaleChild { get; set; }

        public ICharacterObject VillagerFemaleTeenager { get; set; }

        public ICharacterObject VillagerMaleChild { get; set; }

        public ICharacterObject VillagerMaleTeenager { get; set; }

        public ICharacterObject VillageWoman { get; set; }

        public ICharacterObject WeaponPracticeStage1 { get; set; }

        public ICharacterObject WeaponPracticeStage2 { get; set; }

        public ICharacterObject WeaponPracticeStage3 { get; set; }

        public ICharacterObject Weaponsmith { get; set; }
    }
}