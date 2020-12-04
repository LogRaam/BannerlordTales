// Code written by Gabriel Mailhot, 21/11/2020.

/*
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace TalesRuntime.Behavior
{
  internal class PrisonerBehavior : CampaignBehaviorBase
  {
    private readonly EquipmentIndex[] eIndices;

    public virtual void RegisterEvents() => CampaignEvents.get_HeroPrisonerTaken().AddNonSerializedListener((object) this, (Action<PartyBase, Hero>) ((party, hero) =>
    {
      if (hero != Hero.MainHero() && !hero.IsPlayerCompanion()) return;

      ItemRoster itemRoster1 = party.ItemRoster();
      IEnumerable<PartyBase> ownedParties = hero.OwnedParties();

      if (ownedParties != null && ownedParties.Count<PartyBase>() > 0)
      {
        string name = ((object) hero.Name).ToString();
        PartyBase partyBase = ownedParties.First<PartyBase>((Func<PartyBase, bool>) (x => ((object) x.Name()).ToString().Contains(name) 
                                                                                          && x.MapFaction() == hero.MapFaction()));
        
        if (partyBase != null)
        {
          ItemRoster itemRoster2 = partyBase.ItemRoster();

          for (int index = itemRoster2.Count() - 1; index > -1; --index)
          {
            ItemRosterElement itemRosterElement = itemRoster2.Item(index);
            EquipmentElement equipmentElement = ((ItemRosterElement) ref itemRosterElement).EquipmentElement();
            int amount = ((ItemRosterElement) ref itemRosterElement).Amount();
            
            if (itemRoster1.FindIndexOfElement(equipmentElement) != -1)
              itemRoster1.AddToCounts(equipmentElement, amount, true);
            else
              itemRoster1.Add(new ItemRosterElement(equipmentElement, amount));
            
            itemRoster2.AddToCountsAtIndex(index, amount * -1, true);
          }
          itemRoster2.RemoveZeroCounts();
        }
      }
      
      foreach (int eIndex in this.eIndices)
      {
        EquipmentIndex ei = (EquipmentIndex) eIndex;
        this.TransferItem(true, ei, hero, itemRoster1);
        this.TransferItem(false, ei, hero, itemRoster1);
      }

      ItemObject itemObject = Items.FindAll((Func<ItemObject, bool>) (x => ((MBObjectBase) x).get_StringId() == "tattered_rags")).FirstOrDefault<ItemObject>();
      hero.get_BattleEquipment().set_Item((EquipmentIndex) 6, new EquipmentElement(itemObject, (ItemModifier) null));
      hero.get_CivilianEquipment().set_Item((EquipmentIndex) 6, new EquipmentElement(itemObject, (ItemModifier) null));
    }));

    private void TransferItem(
      bool battleEquipment,
      EquipmentIndex ei,
      Hero hero,
      ItemRoster partyItems)
    {
      EquipmentElement equipmentElement = battleEquipment ? hero.get_BattleEquipment().get_Item(ei) : hero.get_CivilianEquipment().get_Item(ei);
      if (((EquipmentElement) ref equipmentElement).get_Item() == null)
        return;
      if (partyItems.FindIndexOfElement(equipmentElement) != -1)
        partyItems.AddToCounts(equipmentElement, 1, true);
      else
        partyItems.Add(new ItemRosterElement(equipmentElement, 1));
      if (battleEquipment)
        hero.get_BattleEquipment().set_Item(ei, (EquipmentElement) EquipmentElement.Invalid);
      else
        hero.get_CivilianEquipment().set_Item(ei, (EquipmentElement) EquipmentElement.Invalid);
    }

    public virtual void SyncData(IDataStore dataStore)
    {
    }

    public PrisonerBehavior()
    {
      // ISSUE: unable to decompile the method.
    }
  }
}
*/

