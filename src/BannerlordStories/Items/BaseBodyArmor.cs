// Code written by Gabriel Mailhot, 12/11/2020.

#region

using TalesEnums;

#endregion

namespace TalesBase.Items
{
    public class BaseBodyArmor
    {
        public float Appearance { get; set; }
        public CultureCode Culture { get; set; }
        public string Id { get; set; }
        public bool IsCivilian { get; set; }
        public ArmorMaterialTypes MaterialType { get; set; }
        public string Name { get; set; }
    }
}