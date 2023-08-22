// Code written by Gabriel Mailhot, 02/12/2023.  Updated by  Gabriel Mailhot on 02/19/2023.

#region

using TalesEnums;

#endregion

namespace TalesContract
{
    public interface IEquipments
    {
        public float Appearance { get; set; }
        public string Armor { get; set; }
        public CultureCode Culture { get; set; }
        public ArmorMaterialTypes Material { get; set; }
        public string Weapon { get; set; }
        void IsEquivalentTo(IEquipments equipments);
    }
}