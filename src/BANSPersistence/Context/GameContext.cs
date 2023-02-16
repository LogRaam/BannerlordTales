// Code written by Gabriel Mailhot, 02/12/2023.

#region

using System.Collections.Generic;
using TalesContract;
using TalesEnums;
using TaleWorlds.TwoDimension;

#endregion

namespace TalesPersistence.Context
{
    public class GameContext
    {
        public readonly ActContext Acts = new ActContext();
        public readonly EventContext Events = new EventContext();
        public readonly HeroContext Heroes = new HeroContext();
        public readonly InventoryContext Inventory = new InventoryContext();
        public readonly TimeContext Time = new TimeContext();
        public readonly LocationContext Tracking = new LocationContext();


        public string LastGameMenuOpened { get; set; } = "Unknown";

        public List<Texture> OriginalBackgroundSpriteSheets { get; set; }

        public bool IsActLocationValidInContext(IAct act)
        {
            switch (act.Location)
            {
                case Location.Unknown: return true;
                case Location.Map when Tracking.IsCurrentlyOnMap != null: return (bool)Tracking.IsCurrentlyOnMap;
                case Location.Settlement when Tracking.IsCurrentlyInSettlement != null: return (bool)Tracking.IsCurrentlyInSettlement;
                case Location.Village when Tracking.IsCurrentlyInVillage != null: return (bool)Tracking.IsCurrentlyInVillage;
                case Location.Dungeon when Tracking.IsCurrentlyInDungeon != null: return (bool)Tracking.IsCurrentlyInDungeon;
                case Location.Castle when Tracking.IsCurrentlyInCastle != null: return (bool)Tracking.IsCurrentlyInCastle;
                case Location.Fortification when Tracking.IsCurrentlyInFortification != null: return (bool)Tracking.IsCurrentlyInFortification;
                case Location.Town when Tracking.IsCurrentlyInTown != null: return (bool)Tracking.IsCurrentlyInTown;
                case Location.Hideout when Tracking.IsCurrentlyInHideout != null: return (bool)Tracking.IsCurrentlyInHideout;

                default: return true;
            }
        }
    }
}