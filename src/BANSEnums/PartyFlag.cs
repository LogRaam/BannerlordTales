namespace TalesEnums
{
   public enum PartyFlag 
   {
      None = 0,
      CaravanParty = 1,
      LordParty = 2,
      VillagerParty = 4,
      MilitiaParty = 8,
      GarrisonParty = 16,
      CommonAreaParty = 32,
      Disbanding = 64,
      IsAlerted = 128,
      IsBanditBossParty = 256,
      ShouldJoinPlayerBattles = 512,
   }
}