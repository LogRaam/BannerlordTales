// Code written by Gabriel Mailhot, 14/11/2020.

#region

using TalesContract;
using TalesEnums;

#endregion

namespace TalesBase.Stories.Evaluation
{
    public class Equipments : IEquipments
    {
        public float Appearance { get; set; }
        public string Armor { get; set; }
        public CultureCode Culture { get; set; }
        public ArmorMaterialTypes Material { get; set; }
        public string Weapon { get; set; }
    }
}