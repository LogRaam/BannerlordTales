// Code written by Gabriel Mailhot, 02/12/2023.

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
            if (value.Contains("aserai")) return CultureCode.Aserai;
            if (value.Contains("sturgia")) return CultureCode.Sturgia;
            if (value.Contains("battania")) return CultureCode.Battania;
            if (value.Contains("looters")) return CultureCode.Looters;
            if (value.Contains("khuzait")) return CultureCode.Khuzait;
            if (value.Contains("vlandia")) return CultureCode.Vlandia;
            if (value.Contains("empire")) return CultureCode.Empire;
            if (value.Contains("neutral_culture")) return CultureCode.Neutral;

            return CultureCode.Anyotherculture;
        }

        private ArmorMaterialTypes GetMaterialTypeFrom(string value)
        {
            if (value.Contains("Cloth")) return ArmorMaterialTypes.Cloth;
            if (value.Contains("Leather")) return ArmorMaterialTypes.Leather;
            if (value.Contains("Chainmail")) return ArmorMaterialTypes.Chainmail;
            if (value.Contains("Plate")) return ArmorMaterialTypes.Plate;

            return ArmorMaterialTypes.Unknown;
        }

        #endregion
    }
}