// Code written by Gabriel Mailhot, 29/08/2020.

#region

using System;
using System.Collections.Generic;
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
         BaseStory result = new BaseStory();
         for (int i = 0; i < story.Length; i++)
            while (!StoryIsFullyExtracted(story[i]))
            {
               MovePointerForward(story, ref i);

               if (story[i].ReferTo("ACT"))
               {
                  BaseAct act = ExtractActFrom(story, ref i);
                  result.Acts.Add(act);
               }

               if (story[i].ReferTo("SEQUENCE"))
               {
                  Sequence act = ExtractSequenceFrom(story, ref i);
                  result.Acts.Add(act);
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
         BaseAct act = new BaseAct();
         MovePointerForward(story, ref i);

         while (!ActIsFullyExtracted(story[i]))
         {
            if (story[i].ReferTo("NAME: ")) act.Name = SetValueFrom(story[i]);
            if (story[i].ReferTo("IMAGE: ")) act.Image = SetValueFrom(story[i]);
            if (story[i].ReferTo("LOCATION: ")) act.Location = SetLocationFrom(story[i]);
            if (story[i].ReferTo("INTRO: ")) act.Intro = ExtractText(story, ref i);
            if (story[i].ReferTo("CHOICE:")) act.Choices.Add(ExtractChoice(story, ref i));
            if (story[i].ReferTo("SEQUENCE:")) act.Choices.Add(ExtractChoice(story, ref i));

            if (!story[i].ReferTo("CHOICE") && !story[i].ReferTo("SEQUENCE")) MovePointerForward(story, ref i);
         }

         return act;
      }

      private Choice ExtractChoice(string[] story, ref int i)
      {
         Choice result = new Choice {
            Text = ExtractText(story, ref i)
         };

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
         string v = GetValueFrom(line);

         List<string> s = line.Split(' ').RemoveEmptyItems().ToList();
         s.RemoveEmptyItems();
         string item = s[1].Reformat();

         Evaluation result = new Evaluation {
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
            Subject = line.ToUpper().Contains("NPC")
               ? Actor.NPC
               : Actor.PLAYER
         };

         return result;
      }

      private Sequence ExtractSequenceFrom(string[] story, ref int i)
      {
         Sequence seq = new Sequence();
         MovePointerForward(story, ref i);

         while (!ActIsFullyExtracted(story[i]))
         {
            if (story[i].ReferTo("NAME: ")) seq.Name = SetValueFrom(story[i]);
            if (story[i].ReferTo("IMAGE: ")) seq.Image = SetValueFrom(story[i]);
            if (story[i].ReferTo("LOCATION: ")) seq.Location = SetLocationFrom(story[i]);
            if (story[i].ReferTo("INTRO: ")) seq.Intro = ExtractText(story, ref i);
            if (story[i].ReferTo("CHOICE:")) seq.Choices.Add(ExtractChoice(story, ref i));
            if (story[i].ReferTo("SEQUENCE:")) seq.Choices.Add(ExtractChoice(story, ref i));

            if (!story[i].ReferTo("CHOICE") && !story[i].ReferTo("SEQUENCE")) MovePointerForward(story, ref i);
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
         string result = SetValueFrom(story[i]);
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
         string result = Regex.Split(line.ToUpper(), "THAN").Last();

         return result;
      }

      private string ExtractToValue(string line)
      {
         string result = Regex.Split(line.ToUpper(), "TO").Last();

         return result;
      }


      private Attributes? GetAttributeFrom(string line)
      {
         switch (line.ToUpper())
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
         switch (line.ToUpper())
         {
            case "AGE":    return Characteristics.AGE;
            case "GENDER": return Characteristics.GENDER;
            case "HEALTH": return Characteristics.HEALTH;
            case "GOLD":   return Characteristics.GOLD;
         }

         return null;
      }

      private Operator GetOperatorFrom(string line)
      {
         if (line.Contains("=")) return Operator.EQUALTO;
         if (line.Contains(">")) return Operator.GREATERTHAN;
         if (line.Contains("<")) return Operator.LOWERTHAN;

         if (line.ToUpper().Contains(" GREATER THAN ")) return Operator.GREATERTHAN;
         if (line.ToUpper().Contains(" LOWER THAN ")) return Operator.LOWERTHAN;
         if (line.ToUpper().Contains(" IS ")) return Operator.EQUALTO;

         return Operator.EQUALTO;
      }

      private PersonalityTraits? GetPersonalityTraitFrom(string line)
      {
         switch (line.ToUpper())
         {
            case "MERCY":      return PersonalityTraits.MERCY;
            case "GENEROSITY": return PersonalityTraits.GENEROSITY;
            case "HONOR":      return PersonalityTraits.HONOR;
            case "VALOR":      return PersonalityTraits.VALOR;
         }

         return null;
      }

      private int GetRandomEndFrom(string line)
      {
         if (line.ToUpper().Contains("BETWEEN ")) return GetRandomEndVerboseFrom(line);

         if (!line.StartsWith("R")) return 0;

         string result = line.Split(' ').RemoveEmptyItems()[2].Trim();

         return Convert.ToInt32(result);
      }

      private int GetRandomEndVerboseFrom(string line)
      {
         string result = line.Split(' ').RemoveEmptyItems().Last().Reformat();

         return Convert.ToInt32(result);
      }

      private int GetRandomStartFrom(string line)
      {
         if (line.ToUpper().Contains("BETWEEN ")) return GetRandomStartVerboseFrom(line);

         if (!line.StartsWith("R")) return 0;

         string result = line.Split(' ').RemoveEmptyItems()[1].Trim();

         return Convert.ToInt32(result);
      }

      private int GetRandomStartVerboseFrom(string line)
      {
         string result = line.Split(' ').RemoveEmptyItems()[1].Reformat();

         return Convert.ToInt32(result);
      }

      private Skills? GetSkillFrom(string line)
      {
         switch (line.ToUpper())
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
         line = line.ToUpper();

         if (line.EndsWith("DAYTIME")) return GameTime.DAYTIME;
         if (line.EndsWith("DAY")) return GameTime.DAYTIME;

         if (line.EndsWith("NIGHTTIME")) return GameTime.NIGHTTIME;
         if (line.EndsWith("NIGHT")) return GameTime.NIGHTTIME;

         return GameTime.ANYTIME;
      }

      private string GetValueFrom(string line)
      {
         line = line.ToUpper();
         line = line.Replace(" EQUAL TO ", " = ");
         line = line.Replace(" GREATER THAN ", " > ");
         line = line.Replace(" LOWER THAN ", " < ");

         if (line.Contains("=")) return line.Split('=').RemoveEmptyItems().Last().Reformat();
         if (line.Contains(">")) return line.Split('>').RemoveEmptyItems().Last().Reformat();
         if (line.Contains("<")) return line.Split('<').RemoveEmptyItems().Last().Reformat();

         return "Unknown";
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
         string s = line.Split(':').RemoveEmptyItems()[1].Reformat();

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
         string s = line.Split(':')[1].Reformat();

         if (s == "YES") return true;
         if (s == "TRUE") return true;

         return false;
      }

      private StoryType SetStoryTypeFrom(string line)
      {
         string t = line.ToUpper();

         if (t.Contains("CAPTIVE")) return StoryType.PLAYER_IS_CAPTIVE;
         if (t.Contains("CAPTOR")) return StoryType.PLAYER_IS_CAPTOR;
         if (t.Contains("MAP")) return StoryType.PLAYER_ON_CAMPAIGN_MAP;
         if (t.Contains("SETTLEMENT")) return StoryType.PLAYER_IN_SETTLEMENT;

         return StoryType.NONE;
      }

      private GameTime SetTimeFrom(string line)
      {
         string s = line.Split(':')[1].Reformat();

         if (s == "DAY") return GameTime.DAYTIME;
         if (s == "NIGHT") return GameTime.NIGHTTIME;
         if (s == "ANYTIME") return GameTime.ANYTIME;

         return GameTime.UNKNOWN;
      }

      private ITrigger SetTriggerFrom(string line)
      {
         if (!line.Contains("%")) return SetUniqueTriggerFrom(line);

         Trigger result = new Trigger();
         string[] s = line.Split(':').RemoveEmptyItems();

         string t1 = s[1].Split(' ').RemoveEmptyItems()[0].Trim();
         result.Link = t1;

         string t2 = s[1].Split(' ').RemoveEmptyItems().Last().Replace("%", string.Empty);
         result.ChanceToTrigger = Convert.ToInt32(t2);

         return result;
      }

      private ITrigger SetUniqueTriggerFrom(string line)
      {
         Trigger result = new Trigger();
         string[] s = line.Split(':').RemoveEmptyItems();
         result.Link = s[1].Trim();
         result.ChanceToTrigger = 100;

         return result;
      }

      private string SetValueFrom(string line)
      {
         if (line.Contains("= R")) return string.Empty;

         string result = line.Split(':').RemoveEmptyItems()[1].Trim();
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
         if (line.ReferTo("END")) return true;

         return false;
      }

      #endregion
   }
}