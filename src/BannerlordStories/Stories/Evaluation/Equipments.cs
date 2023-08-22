// Code written by Gabriel Mailhot, 02/12/2023.  Updated by  Gabriel Mailhot on 02/19/2023.

#region

using FluentAssertions.Execution;
using System;
using TalesContract;
using TalesEnums;

#endregion

namespace TalesBase.Stories.Evaluation
{
    public class Equipments : IEquipments
    {
        public float Appearance { get; set; }
        public string Armor { get; set; }
        public CultureCode Culture { get; set; } = CultureCode.Invalid;
        public ArmorMaterialTypes Material { get; set; } = ArmorMaterialTypes.Unknown;
        public string Weapon { get; set; }

        public void IsEquivalentTo(IEquipments equipments)
        {
            if (Math.Abs(equipments.Appearance - Appearance) > 0.01) throw new AssertionFailedException("Evaluation expected equipments.Appearance to be equivalent to " + equipments.Appearance + ", but found that its value is " + Appearance);
            if (equipments.Armor != Armor) throw new AssertionFailedException("Evaluation expected equipments.Armor to be equivalent to " + equipments.Armor + ", but found that its value is " + Armor);
            if (equipments.Culture != Culture) throw new AssertionFailedException("Evaluation expected equipments.Culture to be equivalent to " + equipments.Culture + ", but found that its value is " + Culture);
            if (equipments.Material != Material) throw new AssertionFailedException("Evaluation expected equipments.Material to be equivalent to " + equipments.Material + ", but found that its value is " + Material);
            if (equipments.Weapon != Weapon) throw new AssertionFailedException("Evaluation expected equipments.Weapon to be equivalent to " + equipments.Weapon + ", but found that its value is " + Weapon);
        }
    }
}