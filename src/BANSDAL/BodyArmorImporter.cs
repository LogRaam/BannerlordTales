// Code written by Gabriel Mailhot, 12/11/2020.

#region

using System.Collections.Generic;
using System.IO;
using System.Xml;
using TalesBase.Items;
using TalesEnums;

#endregion

namespace TalesDAL
{
    public class BodyArmorImporter
    {
        public List<BaseBodyArmor> ImportBodyArmorsFrom(FileInfo file)
        {
            var result = new List<BaseBodyArmor>();

            var xml = new XmlDocument();
            xml.Load(file.OpenRead());

            var parentNode = xml.ChildNodes.Item(1);

            if (parentNode == null) return result;


            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.Name != "Item") continue;

                var item = new BaseBodyArmor
                {
                    Id = node.Attributes?.GetNamedItem("id").Value,
                    Name = node.Attributes?.GetNamedItem("name").Value,
                    Culture = GetCultureCodeFrom(node.Attributes?.GetNamedItem("culture").Value),
                    Appearance = ConvertAppearanceFrom(node.Attributes?.GetNamedItem("appearance")?.Value),
                    MaterialType = GetMaterialTypeFrom(node.ChildNodes[0].ChildNodes[0].Attributes?.GetNamedItem("material_type").Value),
                    IsCivilian = node.ChildNodes[1]?.Attributes?.GetNamedItem("Civilian")?.Value == "true"
                };
                result.Add(item);
            }

            return result;
        }

        #region private

        private float ConvertAppearanceFrom(string value)
        {
            if (value == null) return 0.0f;

            return float.Parse(value);
        }

        private CultureCode GetCultureCodeFrom(string value)
        {
            if (value.Contains("aserai")) return CultureCode.ASERAI;
            if (value.Contains("sturgia")) return CultureCode.STURGIA;
            if (value.Contains("battania")) return CultureCode.BATTANIA;
            if (value.Contains("looters")) return CultureCode.LOOTERS;
            if (value.Contains("khuzait")) return CultureCode.KHUZAIT;
            if (value.Contains("vlandia")) return CultureCode.VLANDIA;
            if (value.Contains("empire")) return CultureCode.EMPIRE;
            if (value.Contains("neutral_culture")) return CultureCode.NEUTRAL;

            return CultureCode.ANYOTHERCULTURE;
        }

        private ArmorMaterialTypes GetMaterialTypeFrom(string value)
        {
            if (value.Contains("Cloth")) return ArmorMaterialTypes.CLOTH;
            if (value.Contains("Leather")) return ArmorMaterialTypes.LEATHER;
            if (value.Contains("Chainmail")) return ArmorMaterialTypes.CHAINMAIL;
            if (value.Contains("Plate")) return ArmorMaterialTypes.PLATE;

            return ArmorMaterialTypes.UNKNOWN;
        }

        #endregion
    }
}