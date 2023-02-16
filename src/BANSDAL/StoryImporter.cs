// Code written by Gabriel Mailhot, 02/12/2023.

#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TalesBase.Stories;
using TalesBase.Stories.Evaluation;
using TalesBase.TW;
using TalesContract;
using TalesEnums;

#endregion

namespace TalesDAL
{
    #region

    #endregion

    public class StoryImporter
    {
        public List<IStory> ImportFrom(FileInfo file)
        {
            var result = new List<IStory>();

            var extractedList = ExtractStoriesFromArray(File.ReadAllLines(file.FullName));

            foreach (var storyList in extractedList) result.Add(ImportFrom(storyList.ToArray()));


            return result;
        }

        public IStory ImportFrom(string[] story)
        {
            if (!story[0].ReferTo("STORY")) return null;

            story = story.RemoveEmptyItems();
            story = story.RemoveSpecialCharacters();

            var result = new BaseStory();
            for (var i = 0; i < story.Length; i++)
                while (!StoryIsFullyExtracted(story[i]))
                {
                    if (!story[i].ReferTo("ACT") && !story[i].ReferTo("SEQUENCE")) MovePointerForward(story, ref i);

                    if (story[i].ReferTo("ACT"))
                    {
                        var act = ExtractActFrom(story, ref i);
                        act.ParentStory = result;
                        result.Acts.Add(act);
                    }

                    if (story[i].ReferTo("SEQUENCE"))
                    {
                        var sequence = ExtractSequenceFrom(story, ref i);
                        sequence.ParentStory = result;
                        result.Sequences.Add(sequence);
                    }

                    ExtractStoryDataFrom(ref story[i], ref result);
                }

            return result;
        }

        #region private

        private bool ActIsFullyExtracted(string line)
        {
            if (line.ReferTo("ACT")) return true;
            if (line.ReferTo("SEQUENCE")) return true;
            if (line.ReferTo("END")) return true;

            return false;
        }

        private bool ChoiceIsFullyExtracted(string line)
        {
            if (line.ReferTo("CHOICE")) return true;
            if (line.ReferTo("SEQUENCE")) return true;
            if (line.ReferTo("ACT")) return true;
            if (line.ReferTo("END")) return true;

            return false;
        }

        private BaseAct ExtractActFrom(string[] story, ref int i)
        {
            var act = new BaseAct();
            MovePointerForward(story, ref i);

            while (!ActIsFullyExtracted(story[i]))
            {
                if (story[i].ReferTo("INTRO: ")) act.Intro = ExtractText(story, ref i);
                if (story[i].ReferTo("NAME: ")) act.Name = SetValueFrom(story[i]);
                if (story[i].ReferTo("IMAGE: ")) act.Image = SetValueFrom(story[i]);
                if (story[i].ReferTo("LOCATION: ")) act.Location = SetLocationFrom(story[i]);
                if (story[i].ReferTo("RESTRICTION: ")) act.Restrictions.Add(ExtractRestriction(ExtractText(story, ref i)));
                if (story[i].ReferTo("CHOICE:"))
                {
                    var c = ExtractChoice(story, ref i);
                    c.ParentAct = act;
                    act.Choices.Add(c);
                }

                if (!story[i].ReferTo("CHOICE") && !story[i].ReferTo("ACT") && !story[i].ReferTo("SEQUENCE")) MovePointerForward(story, ref i);
            }

            return act;
        }

        private BaseChoice ExtractChoice(string[] story, ref int i)
        {
            var result = new BaseChoice
            {
                Text = ExtractText(story, ref i)
            };

            if (story[i].ReferTo("ACT")) return result;
            if (story[i].ReferTo("SEQUENCE")) return result;

            while (!ChoiceIsFullyExtracted(story[i]))
            {
                if (story[i].ReferTo("CONDITION:")) result.Conditions.Add(ExtractEvaluationFrom(story[i]));
                if (story[i].ReferTo("CONSEQUENCE:")) result.Consequences.Add(ExtractEvaluationFrom(story[i]));
                if (story[i].ReferTo("GOTO:")) result.Triggers.Add(SetTriggerFrom(story[i]));
                MovePointerForward(story, ref i);
            }

            return result;
        }

        private CultureCode ExtractCultureCodeFrom(string cultureValue)
        {
            if (!Enum.TryParse(cultureValue, true, out CultureCode result)) return CultureCode.Invalid;

            return result;
        }

        private IEvaluation ExtractEvaluationFrom(string line)
        {
            line = line.ToUpper();

            var v = GetValueFrom(line);
            var item = GetSubjectItem(line.Replace("PLAYER", ""));

            var result = new BaseEvaluation
            {
                Persona = new Persona(),
                PartyType = GetPartyTypeFrom(line),
                Time = GetTimeFrom(line),
                Equipments = new Equipments
                {
                    Weapon = line.Contains("WEAPON")
                        ? GetWeaponFrom(line)
                        : null,
                    Armor = line.Contains("ARMOR")
                        ? GetArmorFrom(line)
                        : null,
                    Culture = line.Contains("CULTURE")
                        ? GetEquipmentCultureFrom(line)
                        : CultureCode.Invalid,
                    Material = line.Contains("MATERIAL")
                        ? GetEquipmentMaterialFrom(line)
                        : ArmorMaterialTypes.Unknown,
                    Appearance = line.Contains("APPEARANCE")
                        ? GetEquipmentAppearanceFrom(line)
                        : 0.0f
                },
                Numbers = new Numbers
                {
                    Operator = GetOperatorFrom(line),
                    Value = v.Reformat().Replace("%", string.Empty),
                    ValueIsPercentage = IsThisAPercentageValue(v),
                    RandomStart = GetRandomStartFrom(v),
                    RandomEnd = GetRandomEndFrom(v)
                },
                Outcome = new Outcome
                {
                    PregnancyRisk = IsAtRiskOfBecomingPregnant(line),
                    Escaping = IsEscapingFromCaptor(line),
                    ShouldUndress = ShouldRemoveClothes(line),
                    ShouldEquip = ShouldEquip(line)
                }
            };
            result.Persona.Attribute = GetAttributeFrom(item);
            result.Persona.Characteristic = GetCharacteristicFrom(item);
            result.Persona.PersonalityTrait = GetPersonalityTraitFrom(item);
            result.Persona.Skill = GetSkillFrom(item);
            result.Persona.Subject = line.Contains("NPC")
                ? Actor.Npc
                : Actor.Player;

            if (result.Numbers.RandomEnd > 0) result.Numbers.Value = result.Numbers.RandomEnd.ToString();

            return result;
        }

        private ArmorMaterialTypes ExtractMaterialTypeFrom(string materialValue)
        {
            return !Enum.TryParse(materialValue, true, out ArmorMaterialTypes result)
                ? ArmorMaterialTypes.Unknown
                : result;
        }

        private IEvaluation ExtractRestriction(string value)
        {
            var result = new BaseEvaluation();
            result.Tags.Add(value);

            return result;
        }

        private BaseSequence ExtractSequenceFrom(string[] story, ref int i)
        {
            var seq = new BaseSequence();
            MovePointerForward(story, ref i);

            while (!ActIsFullyExtracted(story[i]))
            {
                if (story[i].ReferTo("INTRO: ")) seq.Intro = ExtractText(story, ref i);
                if (story[i].ReferTo("NAME: ")) seq.Name = SetValueFrom(story[i]);
                if (story[i].ReferTo("IMAGE: ")) seq.Image = SetValueFrom(story[i]);
                if (story[i].ReferTo("LOCATION: ")) seq.Location = SetLocationFrom(story[i]);
                if (story[i].ReferTo("CHOICE:"))
                {
                    var s = ExtractChoice(story, ref i);
                    s.ParentAct = seq;
                    seq.Choices.Add(s);
                }

                if (!story[i].ReferTo("ACT") && !story[i].ReferTo("CHOICE") && !story[i].ReferTo("SEQUENCE")) MovePointerForward(story, ref i);
            }

            return seq;
        }

        private List<List<string>> ExtractStoriesFromArray(string[] lines)
        {
            var result = new List<List<string>>();
            var t = new List<string>();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                if (line.ToUpper() == "STORY") t = new List<string>();
                t.Add(line);
                if (line.ToUpper() == "END") result.Add(t);
            }

            return result;
        }

        private void ExtractStoryDataFrom(ref string line, ref BaseStory result)
        {
            if (line.ReferTo("NAME: ")) result.Header.Name = SetValueFrom(line);
            if (line.ReferTo("ONETIMESTORY: ")) result.Header.CanBePlayedOnlyOnce = SetOneTimeStoryFrom(line);
            if (line.ReferTo("DEPENDON: ")) result.Header.DependOn = SetValueFrom(line);
            if (line.ReferTo("TIME: ")) result.Header.Time = SetTimeFrom(line);
            if (line.ReferTo("STORYTYPE: ")) result.Header.TypeOfStory = SetStoryTypeFrom(line);
            if (line.ReferTo("RESTRICTION: ")) result.Restrictions.Add(ExtractEvaluationFrom(line));
        }

        private string ExtractText(string[] story, ref int i)
        {
            var result = SetValueFrom(story[i]);
            MovePointerForward(story, ref i);

            while (!TextFullyExtracted(story[i]))
            {
                result += " " + story[i];
                MovePointerForward(story, ref i);
            }

            result = result.Replace("~", string.Empty);
            result = result.Trim();

            return result;
        }

        private string ExtractValueFromDeclaration(string line, string declaration)
        {
            var s = line.Split(',');
            var t = "";

            foreach (var section in s)
            {
                if (!section.ToUpper().Contains(declaration)) continue;

                t = section.Split(' ').Last();
            }

            return t;
        }

        private string GetArmorFrom(string line)
        {
            //var r = new Regex(@"(?<=\b(armor|ARMOR|Armor)\s)(\w+)", RegexOptions.Compiled);
            //var armor = r.Match(line);

            var value = ExtractValueFromDeclaration(line, "ARMOR");

            if (value == "CULTURE") return null;
            if (value == "MATERIAL") return null;
            if (value == "APPEARANCE") return null;

            return value.ToLower();
        }


        private Attributes? GetAttributeFrom(string line)
        {
            Enum.TryParse(line, true, out Attributes result);

            /*switch (line)
            {
                case "VIGOR":        return Attributes.VIGOR;
                case "CONTROL":      return Attributes.CONTROL;
                case "ENDURANCE":    return Attributes.ENDURANCE;
                case "CUNNING":      return Attributes.CUNNING;
                case "SOCIAL":       return Attributes.SOCIAL;
                case "INTELLIGENCE": return Attributes.INTELLIGENCE;
            }

            return Attributes.UNKNOWN;*/

            return result;
        }

        private Characteristics? GetCharacteristicFrom(string line)
        {
            Enum.TryParse(line, true, out Characteristics result);

            /*switch (line)
            {
                case "AGE":     return Characteristics.AGE;
                case "GENDER":  return Characteristics.GENDER;
                case "HEALTH":  return Characteristics.HEALTH;
                case "GOLD":    return Characteristics.GOLD;
                case "CULTURE": return Characteristics.CULTURE;
            }

            return Characteristics.UNKNOWN;*/

            return result;
        }

        private float GetEquipmentAppearanceFrom(string line)
        {
            /*
            var r = new Regex(@"(?<=\b(appearance (<|>|=)|APPEARANCE (<|>|=)|Appearance (<|>|=))\s)(\w+)", RegexOptions.Compiled);
            var appearance = r.Match(line);

            if (appearance.Length == 0)
            {
                r = new Regex(@"(?<=\b(appearance|APPEARANCE|Appearance)\s)(\w+)", RegexOptions.Compiled);
                appearance = r.Match(line);
            }

            float.TryParse(appearance.Value, NumberStyles.Any, new NumberFormatInfo(), out var result);
            */

            var t = ExtractValueFromDeclaration(line, "APPEARANCE");


            float.TryParse(t, NumberStyles.Any, new NumberFormatInfo(), out var result);

            return result;
        }


        private CultureCode GetEquipmentCultureFrom(string line)
        {
            //var r = new Regex(@"(?<=\b(culture|CULTURE|Culture)\s)(\w+)", RegexOptions.Compiled);
            //var culture = r.Match(line);

            var value = ExtractValueFromDeclaration(line, "CULTURE");

            return ExtractCultureCodeFrom(value);
        }

        private ArmorMaterialTypes GetEquipmentMaterialFrom(string line)
        {
            //var r = new Regex(@"(?<=\b(material|MATERIAL|Material)\s)(\w+)", RegexOptions.Compiled);
            //var material = r.Match(line);

            var value = ExtractValueFromDeclaration(line, "MATERIAL");

            return ExtractMaterialTypeFrom(value);
        }

        private Operator GetOperatorFrom(string line)
        {
            if (line.Contains("=")) return Operator.Equalto;
            if (line.Contains(">")) return Operator.Greaterthan;
            if (line.Contains("<")) return Operator.Lowerthan;

            if (line.Contains(" GREATER THAN ")) return Operator.Greaterthan;
            if (line.Contains(" LOWER THAN ")) return Operator.Lowerthan;
            if (line.Contains(" IS ")) return Operator.Equalto;

            return Operator.Equalto;
        }

        private PartyType GetPartyTypeFrom(string line)
        {
            Enum.TryParse(line, true, out PartyType result);

            /*if (line.Contains("LORD")) return PartyType.LORD;
            if (line.Contains("BANDIT")) return PartyType.BANDIT;
            if (line.Contains("VILLAGER")) return PartyType.VILLAGER;
            if (line.Contains("GARRISON")) return PartyType.GARRISONPARTY;

            return PartyType.UNKNOWN;*/

            return result;
        }

        private PersonalityTraits? GetPersonalityTraitFrom(string line)
        {
            Enum.TryParse(line, true, out PersonalityTraits result);

            /*switch (line)
            {
                case "MERCY":      return PersonalityTraits.MERCY;
                case "GENEROSITY": return PersonalityTraits.GENEROSITY;
                case "HONOR":      return PersonalityTraits.HONOR;
                case "VALOR":      return PersonalityTraits.VALOR;
            }

            return PersonalityTraits.UNKNOWN;*/

            return result;
        }

        private int GetRandomEndFrom(string line)
        {
            if (line.ToUpper().Contains("BETWEEN ")) return GetRandomEndVerboseFrom(line);

            if (!line.TrimStart().StartsWith("R ")) return 0;

            var result = line.Split(' ').RemoveEmptyItems()[2].Trim();

            return Convert.ToInt32(result);
        }

        private int GetRandomEndVerboseFrom(string line)
        {
            var result = line.Split(' ').RemoveEmptyItems().Last().Reformat();

            return Convert.ToInt32(result);
        }

        private string GetRandomExpressionFrom(string line)
        {
            var result = line.Remove(0, line.LastIndexOf(" R ", StringComparison.Ordinal));

            return result;
        }

        private int GetRandomStartFrom(string line)
        {
            if (line.ToUpper().Contains("BETWEEN ")) return GetRandomStartVerboseFrom(line);

            if (!line.TrimStart().StartsWith("R ")) return 0;

            var result = line.Split(' ').RemoveEmptyItems()[1].Trim();

            return Convert.ToInt32(result);
        }

        private int GetRandomStartVerboseFrom(string line)
        {
            var result = line.Split(' ').RemoveEmptyItems()[1].Reformat();

            return Convert.ToInt32(result);
        }

        private string GetSimpleValueFrom(string line)
        {
            line = line.ToUpper().Trim();

            if (!line.Contains(" IS GREATER THAN ") && !line.Contains(" IS LOWER THAN ")) line = line.Replace(" IS ", " = ");

            line = line.Replace(" EQUAL TO ", " = ");
            line = line.Replace(" GREATER THAN ", " > ");
            line = line.Replace(" LOWER THAN ", " < ");

            if (line.Contains("=")) return line.Split('=').RemoveEmptyItems().Last().Reformat();
            if (line.Contains(">")) return line.Split('>').RemoveEmptyItems().Last().Reformat();
            if (line.Contains("<")) return line.Split('<').RemoveEmptyItems().Last().Reformat();

            return line.Split(' ').Last();
        }

        private Skills? GetSkillFrom(string line)
        {
            Enum.TryParse(line, true, out Skills result);

            /*
            switch (line)
            {
                case "ONEHANDED":   return Skills.ONEHANDED;
                case "TWOHANDED":   return Skills.TWOHANDED;
                case "POLEARM":     return Skills.POLEARM;
                case "BOW":         return Skills.BOW;
                case "CROSSBOW":    return Skills.CROSSBOW;
                case "THROWING":    return Skills.THROWING;
                case "RIDING":      return Skills.RIDING;
                case "ATHLETICS":   return Skills.ATHLETICS;
                case "CRAFTING":    return Skills.CRAFTING;
                case "SCOUTING":    return Skills.SCOUTING;
                case "TACTICS":     return Skills.TACTICS;
                case "ROGUERY":     return Skills.ROGUERY;
                case "CHARM":       return Skills.CHARM;
                case "LEADERSHIP":  return Skills.LEADERSHIP;
                case "TRADE":       return Skills.TRADE;
                case "STEWARD":     return Skills.STEWARD;
                case "MEDICINE":    return Skills.MEDICINE;
                case "ENGINEERING": return Skills.ENGINEERING;
            }
            
            return Skills.UNKNOWN;*/

            return result;
        }

        private string GetSubjectItem(string line)
        {
            if (line.ToUpper().Contains("ESCAP")) return "ESCAPE";

            if (line.Reformat().Contains("Npc Is ")) return "Npc";

            var s = line.Split(' ').RemoveEmptyItems().ToList();
            s.RemoveEmptyItems();

            var item = s[1].Reformat();

            if (item == "Npc") item = s[2].Reformat();

            return item;
        }

        private GameTime GetTimeFrom(string line)
        {
            if (line.EndsWith("DAYTIME")) return GameTime.Daytime;
            if (line.EndsWith("DAY")) return GameTime.Daytime;

            if (line.EndsWith("NIGHTTIME")) return GameTime.Nighttime;
            if (line.EndsWith("NIGHT")) return GameTime.Nighttime;

            return GameTime.Anytime;
        }

        private string GetValueFrom(string line)
        {
            if (!line.Contains(" ")) return "";

            return line.Contains(" R ")
                ? GetRandomExpressionFrom(line)
                : GetSimpleValueFrom(line);
        }

        private string GetWeaponFrom(string line)
        {
            if (line.Contains("TYPE ")) return GetWeaponTypeFrom(line);

            //var r = new Regex(@"(?<=\b(weapon|WEAPON|Weapon)\s)(\w+)", RegexOptions.Compiled);
            //var weapon = r.Match(line);

            var value = ExtractValueFromDeclaration(line, "WEAPON");

            if (value == "CULTURE") return null;
            if (value == "MATERIAL") return null;
            if (value == "APPEARANCE") return null;

            return value;
        }

        private string GetWeaponTypeFrom(string line)
        {
            //var r = new Regex(@"(?<=\b(type|TYPE|Type)\s)(\w+)", RegexOptions.Compiled);
            //var weapon = r.Match(line);

            var value = ExtractValueFromDeclaration(line, "TYPE");

            return value.ToLower();
        }

        private bool IsAtRiskOfBecomingPregnant(string line)
        {
            return line.Contains(" PREGNAN");
        }

        private bool IsEscapingFromCaptor(string line)
        {
            return line.Contains("ESCAP");
        }


        private bool IsThisAPercentageValue(string line)
        {
            return line.TrimEnd().EndsWith("%");
        }

        private void MovePointerForward(string[] story, ref int i)
        {
            if (story[i].ReferTo("END")) return;

            i++;

            if (i == story.Length) throw new ApplicationException("Got END of story without hitting on END tag.");
        }


        private Location SetLocationFrom(string line)
        {
            var s = line.Split(':').RemoveEmptyItems()[1].Reformat();

            try
            {
                Enum.TryParse(s, out Location result);

                return result;
            }
            catch
            {
                return Location.Unknown;
            }
        }

        private bool SetOneTimeStoryFrom(string line)
        {
            var s = line.Split(':')[1].Reformat();

            if (s == "Yes") return true;
            if (s == "True") return true;

            return false;
        }

        private StoryType SetStoryTypeFrom(string line)
        {
            var t = line.ToUpper();

            if (t.Contains("SURRENDER")) return StoryType.PlayerSurrender;
            if (t.Contains("CAPTIVE")) return StoryType.PlayerIsCaptive;
            if (t.Contains("CAPTOR")) return StoryType.PlayerIsCaptor;
            if (t.Contains("MAP")) return StoryType.PlayerOnCampaignMap;
            if (t.Contains("SETTLEMENT")) return StoryType.PlayerInSettlement;
            if (t.Contains("WAIT")) return StoryType.Waiting;

            return StoryType.None;
        }

        private GameTime SetTimeFrom(string line)
        {
            var s = line.Split(':')[1].Reformat();

            if (s == "Day") return GameTime.Daytime;
            if (s == "Night") return GameTime.Nighttime;
            if (s == "Anytime") return GameTime.Anytime;

            return GameTime.None;
        }

        private ITrigger SetTriggerFrom(string line)
        {
            if (!line.Contains("%")) return SetUniqueTriggerFrom(line);

            var result = new BaseTrigger
            {
                Link = line.ExtractLinkWithoutPercentage(),
                ChanceToTrigger = line.ExtractPercentageChancesFromLink()
            };


            return result;
        }

        private ITrigger SetUniqueTriggerFrom(string line)
        {
            var result = new BaseTrigger();
            var s = line.Split(':').RemoveEmptyItems();
            result.Link = s[1].Trim().Replace(".", "");
            result.ChanceToTrigger = 100;

            return result;
        }

        private string SetValueFrom(string line)
        {
            if (line.Contains("= R")) return string.Empty;

            var s = line.Split(':').RemoveEmptyItems();

            if (s.Length < 2) return "";

            var result = s[1].Trim().Replace("%", string.Empty);

            return result;
        }

        private bool ShouldEquip(string line)
        {
            if (line.Contains("RETURN CLOTH"))
            {
                //TODO: return civil clothes
                return true;
            }

            if (line.Contains("RETURN ARMOR"))
            {
                //TODO: return military armor
                return true;
            }

            if (line.Contains("RETURN EQUIPMENT"))
            {
                //TODO: return all equipment
                return true;
            }

            if (line.Contains("GIVE WEAPON"))
            {
                //TODO: give new weapons
                return true;
            }

            if (line.Contains("GIVE ARMOR"))
            {
                //TODO: give new military armor
                return true;
            }

            if (line.Contains("GIVE CLOTH"))
            {
                //TODO: give new civilian clothes
                return true;
            }

            return false;
        }

        private bool ShouldRemoveClothes(string line)
        {
            if (line.ToUpper().Contains("STRIP")) return true;
            if (line.ToUpper().Contains("REMOVE CLOTHES")) return true;
            if (line.ToUpper().Contains("REMOVE EQUIPMENT")) return true;
            if (line.ToUpper().Contains("UNDRESS")) return true;
            if (line.ToUpper().Contains("UNEQUIP")) return true;

            return false;
        }

        private bool StoryIsFullyExtracted(string line)
        {
            return line.ReferTo("END");
        }

        private bool TextFullyExtracted(string line)
        {
            if (line.ReferTo("NAME:")) return true;
            if (line.ReferTo("IMAGE:")) return true;
            if (line.ReferTo("CONDITION:")) return true;
            if (line.ReferTo("CONSEQUENCE:")) return true;
            if (line.ReferTo("ID:")) return true;
            if (line.ReferTo("GOTO:")) return true;
            if (line.ReferTo("CHOICE:")) return true;
            if (line.ReferTo("ACT")) return true;
            if (line.ReferTo("SEQUENCE")) return true;
            if (line.ReferTo("RESTRICTION:")) return true;
            if (line.ReferTo("END")) return true;

            return false;
        }

        #endregion
    }
}