// Code written by Gabriel Mailhot, 11/09/2020.

#region

using System;
using System.IO;
using System.Linq;
using TalesBase.Stories;
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
                if (story[i].ReferTo("INTRO: ")) act.Intro = ExtractText(story, ref i);
                if (story[i].ReferTo("NAME: ")) act.Name = SetValueFrom(story[i]);
                if (story[i].ReferTo("IMAGE: ")) act.Image = SetValueFrom(story[i]);
                if (story[i].ReferTo("LOCATION: ")) act.Location = SetLocationFrom(story[i]);
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

        private IEvaluation ExtractEvaluationFrom(string line)
        {
            line = line.ToUpper();

            var v = GetValueFrom(line);
            var item = GetSubjectItem(line.Replace("PLAYER", ""));

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
                    : Actor.PLAYER,
                Escaping = IsEscapingFromCaptor(line)
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

            return Attributes.UNKNOWN;
        }

        private Characteristics? GetCharacteristicFrom(string line)
        {
            switch (line)
            {
                case "AGE":     return Characteristics.AGE;
                case "GENDER":  return Characteristics.GENDER;
                case "HEALTH":  return Characteristics.HEALTH;
                case "GOLD":    return Characteristics.GOLD;
                case "CULTURE": return Characteristics.CULTURE;
            }

            return Characteristics.UNKNOWN;
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

            return PersonalityTraits.UNKNOWN;
        }

        private int GetRandomEndFrom(string line)
        {
            if (line.Contains("BETWEEN ")) return GetRandomEndVerboseFrom(line);

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
            if (line.Contains("BETWEEN ")) return GetRandomStartVerboseFrom(line);

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

            return Skills.UNKNOWN;
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
                ? GetRandomExpressionFrom(line)
                : GetSimpleValueFrom(line);
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

            if (t.Contains("SURRENDER")) return StoryType.PLAYER_SURRENDER;
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

            var result = new BaseTrigger
            {
                Link = line.ExtractLinkWithoutPercentage(), ChanceToTrigger = line.ExtractPercentageChancesFromLink()
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
            if (line.ReferTo("NAME:")) return true;
            if (line.ReferTo("IMAGE:")) return true;
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