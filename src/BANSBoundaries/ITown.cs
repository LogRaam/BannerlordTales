﻿// Code written by Gabriel Mailhot, 01/09/2020.

namespace TalesContract
{
   #region

   using System.Collections.Generic;

   #endregion

   public interface ITown : IFief
   {
      public bool AfterSneakFight { get; set; }

      public IList<ITown> AllCastles { get; set; }

      public IList<ITown> AllFiefs { get; set; }

      public IList<ITown> AllTowns { get; set; }

      public int BoostBuildingProcess { get; set; }

      public List<IBuilding> Buildings { get; set; }

      public Queue<IBuilding> BuildingsInProgress { get; set; }

      public float Construction { get; set; }

      public ICultureObject Culture { get; set; }

      public IBuilding CurrentBuilding { get; set; }

      public IBuilding CurrentDefaultBuilding { get; set; }

      public int DaysAtUnrest { get; set; }

      public float FoodChange { get; set; }

      public int FoodStocksUpperLimit { get; set; }

      public int GarrisonChange { get; set; }

      public IHero Governor { get; set; }

      public bool HasTournament { get; set; }

      public bool IsCastle { get; set; }

      public bool IsRebelling { get; set; }

      public bool IsTown { get; set; }

      public bool IsUnderSiege { get; set; }

      public IClan LastCapturedBy { get; set; }

      public float Loyalty { get; set; }

      public float LoyaltyChange { get; set; }

      public IFaction MapFaction { get; set; }

      public float MilitiaChange { get; set; }

      public IMobileParty MilitiaParty { get; set; }

      public IClan OwnerClan { get; set; }

      public float ProsperityChange { get; set; }

      public float Security { get; set; }

      public float SecurityChange { get; set; }

      public int TradeTaxAccumulated { get; set; }

      public IReadOnlyList<IVillage> Villages { get; set; }
   }
}