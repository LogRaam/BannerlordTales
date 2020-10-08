// Code written by Gabriel Mailhot, 11/09/2020.

#region

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TalesContract;
using TalesEntities.Stories;
using TalesEntities.TW;
using TalesEnums;

#endregion

namespace TalesDAL
{
    #region

    #endregion

    public class StoryImporter
    {
        public IStory ImportFrom(FileInfo file)
        {
            return ImportFrom(File.ReadAllLines(file.FullName));
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
                        act.ParentStory = result.Header.Name;
                        result.Acts.Add(act);
                    }

                    if (story[i].ReferTo("SEQUENCE"))
                    {
                        var sequence = ExtractSequenceFrom(story, ref i);
                        sequence.ParentStory = result.Header.Name;
                        result.Sequences.Add(sequence);
                    }

                    ExtractStoryDataFrom(ref story[i], ref result);
                }

            return result;
        }

        #region private

        private static string FormatLineForOccupation(string line)
        {
            line = line.Insert(line.Contains("NPC")
                ? line.IndexOf("NPC", StringComparison.Ordinal)
                : line.IndexOf(":", StringComparison.Ordinal), " OCCUPATION ");

            return line;
        }

        private static string GetSubjectItem(string line)
        {
            var s = line.Split(' ').RemoveEmptyItems().ToList();
            s.RemoveEmptyItems();
            var item = s[1].Reformat();

            if (item == "NPC") item = s[2].Reformat();

            return item;
        }

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
                if (story[i].ReferTo("NAME: ")) act.Name = SetValueFrom(story[i]);
                if (story[i].ReferTo("IMAGE: ")) act.Image = SetValueFrom(story[i]);
                if (story[i].ReferTo("LOCATION: ")) act.Location = SetLocationFrom(story[i]);
                if (story[i].ReferTo("INTRO: ")) act.Intro = ExtractText(story, ref i);
                if (story[i].ReferTo("CHOICE:")) act.Choices.Add(ExtractChoice(story, ref i));

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
                if (story[i].ReferTo("ID:")) result.Id = SetValueFrom(story[i]);
                if (story[i].ReferTo("GOTO:")) result.Triggers.Add(SetTriggerFrom(story[i]));
                MovePointerForward(story, ref i);
            }

            return result;
        }

        private IEvaluation ExtractEvaluationFrom(string line)
        {
            line = line.ToUpper();
            if (!line.Contains(" OCCUPATION ") && IsThereACultureWithinThis(line)) line = FormatLineForOccupation(line);

            var v = GetValueFrom(line);
            var item = GetSubjectItem(line);

            var result = new BaseEvaluation
            {
                Attribute = GetAttributeFrom(item),
                Characteristic = GetCharacteristicFrom(item),
                PersonalityTrait = GetPersonalityTraitFrom(item),
                Skill = GetSkillFrom(item),
                Operator = GetOperatorFrom(line),
                Value = v.Reformat().Replace("%", string.Empty),
                ValueIsPercentage = IsThisAPercentageValue(v),
                RandomStart = GetRandomStartFrom(v),
                RandomEnd = GetRandomEndFrom(v),
                Time = GetTimeFrom(line),
                PregnancyRisk = IsAtRiskOfBecomingPregnant(line),
                Subject = line.Contains("NPC")
                    ? Actor.NPC
                    : Actor.PLAYER
            };

            if (result.RandomEnd > 0) result.Value = result.RandomEnd.ToString();

            return result;
        }

        private BaseSequence ExtractSequenceFrom(string[] story, ref int i)
        {
            var seq = new BaseSequence();
            MovePointerForward(story, ref i);

            while (!ActIsFullyExtracted(story[i]))
            {
                if (story[i].ReferTo("NAME: ")) seq.Name = SetValueFrom(story[i]);
                if (story[i].ReferTo("IMAGE: ")) seq.Image = SetValueFrom(story[i]);
                if (story[i].ReferTo("LOCATION: ")) seq.Location = SetLocationFrom(story[i]);
                if (story[i].ReferTo("INTRO: ")) seq.Intro = ExtractText(story, ref i);
                if (story[i].ReferTo("CHOICE:")) seq.Choices.Add(ExtractChoice(story, ref i));

                if (!story[i].ReferTo("ACT") && !story[i].ReferTo("CHOICE") && !story[i].ReferTo("SEQUENCE")) MovePointerForward(story, ref i);
            }

            return seq;
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

        private string ExtractThanValue(string line)
        {
            var result = Regex.Split(line.ToUpper(), "THAN").Last();

            return result;
        }

        private string ExtractToValue(string line)
        {
            var result = Regex.Split(line.ToUpper(), "TO").Last();

            return result;
        }


        private Attributes? GetAttributeFrom(string line)
        {
            switch (line)
            {
                case "VIGOR":        return Attributes.VIGOR;
                case "CONTROL":      return Attributes.CONTROL;
                case "ENDURANCE":    return Attributes.ENDURANCE;
                case "CUNNING":      return Attributes.CUNNING;
                case "SOCIAL":       return Attributes.SOCIAL;
                case "INTELLIGENCE": return Attributes.INTELLIGENCE;
            }

            return null;
        }

        private Characteristics? GetCharacteristicFrom(string line)
        {
            switch (line)
            {
                case "AGE":        return Characteristics.AGE;
                case "GENDER":     return Characteristics.GENDER;
                case "HEALTH":     return Characteristics.HEALTH;
                case "GOLD":       return Characteristics.GOLD;
                case "CULTURE":    return Characteristics.CULTURE;
                case "OCCUPATION": return Characteristics.OCCUPATION;
            }

            return null;
        }

        private Operator GetOperatorFrom(string line)
        {
            if (line.Contains("=")) return Operator.EQUALTO;
            if (line.Contains(">")) return Operator.GREATERTHAN;
            if (line.Contains("<")) return Operator.LOWERTHAN;

            if (line.Contains(" GREATER THAN ")) return Operator.GREATERTHAN;
            if (line.Contains(" LOWER THAN ")) return Operator.LOWERTHAN;
            if (line.Contains(" IS ")) return Operator.EQUALTO;

            return Operator.EQUALTO;
        }

        private PersonalityTraits? GetPersonalityTraitFrom(string line)
        {
            switch (line)
            {
                case "MERCY":      return PersonalityTraits.MERCY;
                case "GENEROSITY": return PersonalityTraits.GENEROSITY;
                case "HONOR":      return PersonalityTraits.HONOR;
                case "VALOR":      return PersonalityTraits.VALOR;
            }

            return null;
        }

        private string GetRadomExpressionFrom(string line)
        {
            var result = line.Remove(0, line.LastIndexOf(" R ", StringComparison.Ordinal));

            return result;
        }

        private int GetRandomEndFrom(string line)
        {
            if (line.Contains("BETWEEN ")) return GetRandomEndVerboseFrom(line);

            if (!line.TrimStart().StartsWith("R")) return 0;

            var result = line.Split(' ').RemoveEmptyItems()[2].Trim();

            return Convert.ToInt32(result);
        }

        private int GetRandomEndVerboseFrom(string line)
        {
            var result = line.Split(' ').RemoveEmptyItems().Last().Reformat();

            return Convert.ToInt32(result);
        }

        private int GetRandomStartFrom(string line)
        {
            if (line.Contains("BETWEEN ")) return GetRandomStartVerboseFrom(line);

            if (!line.TrimStart().StartsWith("R")) return 0;

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
            line = line.ToUpper();

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

            return null;
        }

        private GameTime GetTimeFrom(string line)
        {
            if (line.EndsWith("DAYTIME")) return GameTime.DAYTIME;
            if (line.EndsWith("DAY")) return GameTime.DAYTIME;

            if (line.EndsWith("NIGHTTIME")) return GameTime.NIGHTTIME;
            if (line.EndsWith("NIGHT")) return GameTime.NIGHTTIME;

            return GameTime.ANYTIME;
        }

        private string GetValueFrom(string line)
        {
            return line.Contains(" R ")
                ? GetRadomExpressionFrom(line)
                : GetSimpleValueFrom(line);
        }

        private bool IsAtRiskOfBecomingPregnant(string line)
        {
            return line.Contains(" PREGNAN");
        }

        private bool IsThereACultureWithinThis(string line)
        {
            if (line.Contains("TAVERNKEEPER")) return true;
            if (line.Contains("MERCENARY")) return true;
            if (line.Contains("LORD")) return true;
            if (line.Contains("LADY")) return true;
            if (line.Contains("GOODSTRADER")) return true;
            if (line.Contains("ARENAMASTER")) return true;
            if (line.Contains("COMPANION")) return true;
            if (line.Contains("VILLAGER")) return true;
            if (line.Contains("SOLDIER")) return true;
            if (line.Contains("TOWNSFOLK")) return true;
            if (line.Contains("GUILDMASTER")) return true;
            if (line.Contains("MARSHALL")) return true;
            if (line.Contains("TOURNAMENTFIXER")) return true;
            if (line.Contains("RANSOMBROKER")) return true;
            if (line.Contains("WEAPONSMITH")) return true;
            if (line.Contains("ARMORER")) return true;
            if (line.Contains("HORSETRADER")) return true;
            if (line.Contains("TAVERNWENCH")) return true;
            if (line.Contains("SHOPKEEPER")) return true;
            if (line.Contains("TAVERNGAMEHOST")) return true;
            if (line.Contains("BANDIT")) return true;
            if (line.Contains("WANDERER")) return true;
            if (line.Contains("ARTISAN")) return true;
            if (line.Contains("MERCHANT")) return true;
            if (line.Contains("PREACHER")) return true;
            if (line.Contains("HEADMAN")) return true;
            if (line.Contains("GANGLEADER")) return true;
            if (line.Contains("RURALNOTABLE")) return true;
            if (line.Contains("OUTLAW")) return true;
            if (line.Contains("MINORFACTIONCHARACTER")) return true;
            if (line.Contains("PRISONGUARD")) return true;
            if (line.Contains("GUARD")) return true;
            if (line.Contains("SHOPWORKER")) return true;
            if (line.Contains("MUSICIAN")) return true;
            if (line.Contains("GANGSTER")) return true;
            if (line.Contains("BLACKSMITH")) return true;
            if (line.Contains("JUDGE")) return true;
            if (line.Contains("BANNERBEARER")) return true;
            if (line.Contains("CARAVANGUARD")) return true;
            if (line.Contains("SPECIAL")) return true;
            if (line.Contains("NUMBEROFOCCUPATIONS")) return true;

            return false;
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
                return Location.UNKNOWN;
            }
        }

        private bool SetOneTimeStoryFrom(string line)
        {
            var s = line.Split(':')[1].Reformat();

            if (s == "YES") return true;
            if (s == "TRUE") return true;

            return false;
        }

        private StoryType SetStoryTypeFrom(string line)
        {
            var t = line.ToUpper();

            if (t.Contains("CAPTIVE")) return StoryType.PLAYER_IS_CAPTIVE;
            if (t.Contains("CAPTOR")) return StoryType.PLAYER_IS_CAPTOR;
            if (t.Contains("MAP")) return StoryType.PLAYER_ON_CAMPAIGN_MAP;
            if (t.Contains("SETTLEMENT")) return StoryType.PLAYER_IN_SETTLEMENT;
            if (t.Contains("WAIT")) return StoryType.WAITING;

            return StoryType.NONE;
        }

        private GameTime SetTimeFrom(string line)
        {
            var s = line.Split(':')[1].Reformat();

            if (s == "DAY") return GameTime.DAYTIME;
            if (s == "NIGHT") return GameTime.NIGHTTIME;
            if (s == "ANYTIME") return GameTime.ANYTIME;

            return GameTime.UNKNOWN;
        }

        private ITrigger SetTriggerFrom(string line)
        {
            if (!line.Contains("%")) return SetUniqueTriggerFrom(line);

            var result = new BaseTrigger();
            var s = line.Split(':').RemoveEmptyItems();

            var t1 = s[1].Split(' ').RemoveEmptyItems()[0].Trim();
            result.Link = t1;

            var t2 = s[1].Split(' ').RemoveEmptyItems().Last().Replace("%", string.Empty);
            result.ChanceToTrigger = Convert.ToInt32(t2);

            return result;
        }

        private ITrigger SetUniqueTriggerFrom(string line)
        {
            var result = new BaseTrigger();
            var s = line.Split(':').RemoveEmptyItems();
            result.Link = s[1].Trim();
            result.ChanceToTrigger = 100;

            return result;
        }

        private string SetValueFrom(string line)
        {
            if (line.Contains("= R")) return string.Empty;

            var result = line.Split(':').RemoveEmptyItems()[1].Trim();
            result = result.Replace("%", string.Empty);

            return result;
        }

        private bool StoryIsFullyExtracted(string line)
        {
            return line.ReferTo("END");
        }

        private bool TextFullyExtracted(string line)
        {
            if (line.ReferTo("CONDITION:")) return true;
            if (line.ReferTo("CONSEQUENCE:")) return true;
            if (line.ReferTo("ID:")) return true;
            if (line.ReferTo("GOTO:")) return true;
            if (line.ReferTo("CHOICE:")) return true;
            if (line.ReferTo("ACT")) return true;
            if (line.ReferTo("SEQUENCE")) return true;
            if (line.ReferTo("END")) return true;

            return false;
        }

        #endregion
    }
}