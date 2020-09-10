// Code written by Gabriel Mailhot, 29/08/2020.

#region

using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TalesContract;
using TalesEntities.Stories;
using TalesEntities.TW;
using TalesEnums;

#endregion

namespace TalesDAL.Tests
{
   #region

   #endregion

   [TestFixture]
   public class StoryImporterTests
   {
      [Test]
      public void GivenFileDoNotHaveStoryTag_WhenImport_ThenReturnValueShouldBeNull()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "END"
         };

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Should().BeNull();
      }

      [Test]
      public void GivenStoryActChoiceOperatorGreaterTo_WhenImport_ThenIHaveAnOperatorGreaterTo()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "Act",
            "Choice: a choice...",
            "Condition: Player health is equal to 100%",
            "END"
         };
         string expecText = "a choice...";
         BaseStory expectedResult = new BaseStory {
            Acts = {
               new BaseAct {
                  Choices = {
                     new Choice {
                        Text = "a choice...",
                        Conditions = {
                           new Evaluation {
                              Operator = Operator.EQUALTO,
                              Characteristic = Characteristics.HEALTH,
                              Value = "100",
                              ValueIsPercentage = true
                           }
                        }
                     }
                  }
               }
            }
         };

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Acts[0].Choices[0].Text.Should().Be(expecText);
         actualResult.Acts[0].Choices[0].Conditions[0].Operator.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Operator);
         actualResult.Acts[0].Choices[0].Conditions[0].Characteristic.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Characteristic);
         actualResult.Acts[0].Choices[0].Conditions[0].Value.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Value);
         actualResult.Acts[0].Choices[0].Conditions[0].ValueIsPercentage.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].ValueIsPercentage);
      }

      //TODO: continue testing...

      [Test]
      public void GivenStoryActHaveIdEnd_WhenImport_ThenIHaveAPropertyIdentifyingItsDependance()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "Act",
            "Choice: this is a choice.",
            "Id:             AnID",
            "END"
         };

         string expectedResult = "AnID";

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Acts[0].Choices[0].Id.Should().Be(expectedResult);
      }

      [Test]
      public void GivenStoryActIntroMultilineEnd_WhenImport_ThenIHaveAnIntroThatIncludeAllLines()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "Act",
            "Intro: This is a multiline text",
            "and here is the second line.",
            "END"
         };

         string expectedResult = "This is a multiline text and here is the second line.";

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Acts[0].Intro.Should().Be(expectedResult);
      }

      [Test]
      public void GivenStoryActNameEnd_WhenImport_ThenIHaveAnEventWithAName()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "ACT",
            "Name:   FirstAct",
            "END"
         };
         string expectedResult = "FirstAct";

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Acts[0].Name.Should().Be(expectedResult);
      }

      [Test]
      public void GivenStoryActTimeDayEnd_WhenImport_ThenIHaveAnActThatCanBePlayedAtDay()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "ACT",
            "Choice: This is a choice",
            "Condition: Time = Day",
            "END"
         };

         GameTime expectedResult = GameTime.DAYTIME;

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Acts[0].Choices[0].Conditions[0].Time.Should().Be(expectedResult);
      }

      [Test]
      public void GivenStoryActTimeNightEnd_WhenImport_ThenIHaveAnActThatCanBePlayedAtNight()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "ACT",
            "Choice: This is a choice",
            "Condition: Time = Night",
            "END"
         };

         GameTime expectedResult = GameTime.NIGHTTIME;

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Acts[0].Choices[0].Conditions[0].Time.Should().Be(expectedResult);
      }


      [Test]
      public void GivenStoryChoiceConditionNpcHealthGreaterThanVerbose20PercentEnd_WhenImport_ThenIHaveAnEventWithProperResults()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "ACT",
            "Choice: This is another text.",
            "Condition:      Npc health greater than 20% ",
            "END"
         };
         string textResult = "This is another text.";
         Evaluation expectedResult = new Evaluation {
            Operator = Operator.GREATERTHAN,
            Characteristic = Characteristics.HEALTH,
            Value = "20",
            ValueIsPercentage = true
         };

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Acts[0].Choices[0].Text.Should().Be(textResult);
         actualResult.Acts[0].Choices[0].Conditions[0].Operator.Should().Be(expectedResult.Operator);
         actualResult.Acts[0].Choices[0].Conditions[0].ValueIsPercentage.Should().Be(expectedResult.ValueIsPercentage);
         actualResult.Acts[0].Choices[0].Conditions[0].Value.Should().Be(expectedResult.Value);
         actualResult.Acts[0].Choices[0].Conditions[0].Characteristic.Should().Be(expectedResult.Characteristic);
      }

      [Test]
      public void GivenStoryChoiceConditionPlayerHealthGreaterThan20PercentEnd_WhenImport_ThenIHaveAnEventWithProperResults()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "ACT",
            "Choice: This is another text.",
            "Condition:      Player health > 20% ",
            "END"
         };
         string textResult = "This is another text.";
         Evaluation expectedResult = new Evaluation {
            Operator = Operator.GREATERTHAN,
            Characteristic = Characteristics.HEALTH,
            Value = "20",
            ValueIsPercentage = true
         };

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Acts[0].Choices[0].Text.Should().Be(textResult);
         actualResult.Acts[0].Choices[0].Conditions[0].Operator.Should().Be(expectedResult.Operator);
         actualResult.Acts[0].Choices[0].Conditions[0].ValueIsPercentage.Should().Be(expectedResult.ValueIsPercentage);
         actualResult.Acts[0].Choices[0].Conditions[0].Value.Should().Be(expectedResult.Value);
         actualResult.Acts[0].Choices[0].Conditions[0].Characteristic.Should().Be(expectedResult.Characteristic);
      }

      [Test]
      public void GivenStoryChoiceConditionPlayerHealthGreaterThanVerbose20PercentEnd_WhenImport_ThenIHaveAnEventWithProperResults()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "ACT",
            "Choice: This is another text.",
            "Condition:      Player health greater than 20% ",
            "END"
         };
         string textResult = "This is another text.";
         Evaluation expectedResult = new Evaluation {
            Operator = Operator.GREATERTHAN,
            Characteristic = Characteristics.HEALTH,
            Value = "20",
            ValueIsPercentage = true
         };

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Acts[0].Choices[0].Text.Should().Be(textResult);
         actualResult.Acts[0].Choices[0].Conditions[0].Operator.Should().Be(expectedResult.Operator);
         actualResult.Acts[0].Choices[0].Conditions[0].ValueIsPercentage.Should().Be(expectedResult.ValueIsPercentage);
         actualResult.Acts[0].Choices[0].Conditions[0].Value.Should().Be(expectedResult.Value);
         actualResult.Acts[0].Choices[0].Conditions[0].Characteristic.Should().Be(expectedResult.Characteristic);
      }

      [Test]
      public void GivenStoryChoiceConditionPlayerHealthLowerThanVerbose20PercentEnd_WhenImport_ThenIHaveAnEventWithProperResults()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "ACT",
            "Choice: This is another text.",
            "Condition:      Player health is Lower than 20% ",
            "END"
         };
         string textResult = "This is another text.";
         Evaluation expectedResult = new Evaluation {
            Operator = Operator.LOWERTHAN,
            Characteristic = Characteristics.HEALTH,
            Value = "20",
            ValueIsPercentage = true
         };

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Acts[0].Choices[0].Text.Should().Be(textResult);
         actualResult.Acts[0].Choices[0].Conditions[0].Operator.Should().Be(expectedResult.Operator);
         actualResult.Acts[0].Choices[0].Conditions[0].ValueIsPercentage.Should().Be(expectedResult.ValueIsPercentage);
         actualResult.Acts[0].Choices[0].Conditions[0].Value.Should().Be(expectedResult.Value);
         actualResult.Acts[0].Choices[0].Conditions[0].Characteristic.Should().Be(expectedResult.Characteristic);
      }

      [Test]
      public void GivenStoryChoiceConsequencePlayerHealthEqualToRandomRangeEnd_WhenImport_ThenIHaveAnEventWithProperResults()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "ACT",
            "Choice: This is another text.",
            "Consequence:    Player Health = R -10 -20",
            "END"
         };
         string textResult = "This is another text.";
         Evaluation expectedResult = new Evaluation {
            Operator = Operator.EQUALTO,
            Characteristic = Characteristics.HEALTH,
            RandomStart = -10,
            RandomEnd = -20
         };

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Acts[0].Choices[0].Text.Should().Be(textResult);
         actualResult.Acts[0].Choices[0].Consequences[0].Operator.Should().Be(expectedResult.Operator);
         actualResult.Acts[0].Choices[0].Consequences[0].ValueIsPercentage.Should().Be(expectedResult.ValueIsPercentage);
         actualResult.Acts[0].Choices[0].Consequences[0].RandomStart.Should().Be(expectedResult.RandomStart);
         actualResult.Acts[0].Choices[0].Consequences[0].RandomEnd.Should().Be(expectedResult.RandomEnd);
         actualResult.Acts[0].Choices[0].Consequences[0].Characteristic.Should().Be(expectedResult.Characteristic);
      }

      [Test]
      public void GivenStoryChoiceConsequenceRandomVerboseEnd_WhenImport_ThenIHaveAnEventWithProperResults()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "ACT",
            "Choice: This is another text.",
            "Consequence:    Player Health = between -10 -20",
            "END"
         };
         string textResult = "This is another text.";
         Evaluation expectedResult = new Evaluation {
            Operator = Operator.EQUALTO,
            Characteristic = Characteristics.HEALTH,
            RandomStart = -10,
            RandomEnd = -20
         };

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Acts[0].Choices[0].Text.Should().Be(textResult);
         actualResult.Acts[0].Choices[0].Consequences[0].Operator.Should().Be(expectedResult.Operator);
         actualResult.Acts[0].Choices[0].Consequences[0].ValueIsPercentage.Should().Be(expectedResult.ValueIsPercentage);
         actualResult.Acts[0].Choices[0].Consequences[0].RandomStart.Should().Be(expectedResult.RandomStart);
         actualResult.Acts[0].Choices[0].Consequences[0].RandomEnd.Should().Be(expectedResult.RandomEnd);
         actualResult.Acts[0].Choices[0].Consequences[0].Characteristic.Should().Be(expectedResult.Characteristic);
      }

      [Test]
      public void GivenStoryChoiceEnd_WhenImport_ThenIHaveAnEventWithProperResults()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "ACT",
            "Choice: This is another text.",
            "END"
         };
         string expectedResult = "This is another text.";

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Acts[0].Choices[0].Text.Should().Be(expectedResult);
      }

      [Test]
      public void GivenStoryChoiceTrigger50PercentEnd_WhenImport_ThenIHaveAnEventWithProperResults()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "ACT",
            "Choice: This is another text.",
            "GoTo:        GoToNextAct 50%",
            "END"
         };
         string textResult = "This is another text.";
         Trigger expectedResult = new Trigger {
            Link = "GoToNextAct",
            ChanceToTrigger = 50
         };

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Acts[0].Choices[0].Text.Should().Be(textResult);
         actualResult.Acts[0].Choices[0].Triggers[0].Link.Should().Be(expectedResult.Link);
         actualResult.Acts[0].Choices[0].Triggers[0].ChanceToTrigger.Should().Be(expectedResult.ChanceToTrigger);
      }

      [Test]
      public void GivenStoryChoiceTriggerEnd_WhenImport_ThenIHaveAnEventWithProperResults()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "ACT",
            "Choice: This is another text.",
            "Goto:        GoToAnotherAct",
            "END"
         };
         string textResult = "This is another text.";
         Trigger expectedResult = new Trigger {
            Link = "GoToAnotherAct",
            ChanceToTrigger = 100
         };

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Acts[0].Choices[0].Text.Should().Be(textResult);
         actualResult.Acts[0].Choices[0].Triggers[0].Link.Should().Be(expectedResult.Link);
         actualResult.Acts[0].Choices[0].Triggers[0].ChanceToTrigger.Should().Be(expectedResult.ChanceToTrigger);
      }

      [Test]
      public void GivenStoryDependOnEnd_WhenImport_ThenIHaveAPropertyIdentifyingItsDependance()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "DependOn: ActID",
            "END"
         };

         string expectedResult = "ActID";

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Header.DependOn.Should().Be(expectedResult);
      }

      [Test]
      public void GivenStoryEnd_WhenImport_ThenIHaveAStory()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "ACT",
            "END"
         };
         Type expectedResult = typeof(BaseStory);

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Should().BeOfType(expectedResult);
      }

      [Test]
      public void GivenStoryImageEnd_WhenImport_ThenIHaveAnEventWithImage()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "ACT",
            "Image:   ImageName",
            "END"
         };
         string expectedResult = "ImageName";

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Acts[0].Image.Should().Be(expectedResult);
      }

      [Test]
      public void GivenStoryIntroEnd_WhenImport_ThenIHaveAnEventWithIntro()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "ACT",
            "Intro:  This is a text for the event.",
            "END"
         };
         string expectedResult = "This is a text for the event.";

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Acts[0].Intro.Should().Be(expectedResult);
      }

      [Test]
      public void GivenStoryLocationMapEnd_WhenImport_ThenIHaveAStoryWithLocationMap()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "Act",
            "Location: Map",
            "END"
         };
         Location expectedResult = Location.MAP;

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Acts[0].Location.Should().Be(expectedResult);
      }

      [Test]
      public void GivenStoryOneTimeStoryFalseEnd_WhenImport_ThenIHaveAStoryThatCanBePlayedOnlyOnce()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "OneTimeStory: False",
            "END"
         };

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Header.CanBePlayedOnlyOnce.Should().BeFalse();
      }

      [Test]
      public void GivenStoryOneTimeStoryNoEnd_WhenImport_ThenIHaveAStoryThatCanBePlayedOnlyOnce()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "OneTimeStory: No",
            "END"
         };

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Header.CanBePlayedOnlyOnce.Should().BeFalse();
      }

      [Test]
      public void GivenStoryOneTimeStoryTrueEnd_WhenImport_ThenIHaveAStoryThatCanBePlayedOnlyOnce()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "OneTimeStory: True",
            "END"
         };

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Header.CanBePlayedOnlyOnce.Should().BeTrue();
      }

      [Test]
      public void GivenStoryOneTimeStoryYesEnd_WhenImport_ThenIHaveAStoryThatCanBePlayedOnlyOnce()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "OneTimeStory: Yes",
            "END"
         };

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Header.CanBePlayedOnlyOnce.Should().BeTrue();
      }

      [Test]
      public void GivenStoryRestrictionPlayerAgeGT18End_WhenImport_ThenIHaveAStoryWithProperRestriction()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "Restriction: Player age > 18",
            "END"
         };
         Evaluation expectedResult = new Evaluation {
            Operator = Operator.GREATERTHAN,
            Characteristic = Characteristics.AGE,
            Value = "18"
         };

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Restrictions[0].Operator.Should().Be(expectedResult.Operator);
         actualResult.Restrictions[0].Value.Should().Be(expectedResult.Value);
         actualResult.Restrictions[0].Characteristic.Should().Be(expectedResult.Characteristic);
      }

      [Test]
      public void GivenStoryRestrictionPlayerGenderEqualToFemaleEnd_WhenImport_ThenIHaveAStoryWithProperRestriction()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "Restriction: Player gender = female",
            "END"
         };
         Evaluation expectedResult = new Evaluation {
            Operator = Operator.EQUALTO,
            Characteristic = Characteristics.GENDER,
            Value = "FEMALE"
         };

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Restrictions[0].Operator.Should().Be(expectedResult.Operator);
         actualResult.Restrictions[0].Value.Should().Be(expectedResult.Value);
         actualResult.Restrictions[0].Characteristic.Should().Be(expectedResult.Characteristic);
      }

      [Test]
      public void GivenStoryRestrictionPlayerValorLowerThan30End_WhenImport_ThenIHaveAStoryWithProperRestriction()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "Restriction: Player Valor < 30",
            "END"
         };

         Evaluation expectedResult = new Evaluation {
            Operator = Operator.LOWERTHAN,
            PersonalityTrait = PersonalityTraits.VALOR,
            Value = "30"
         };

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Restrictions[0].Operator.Should().Be(expectedResult.Operator);
         actualResult.Restrictions[0].Value.Should().Be(expectedResult.Value);
         actualResult.Restrictions[0].PersonalityTrait.Should().Be(expectedResult.PersonalityTrait);
      }


      [Test]
      public void StoryImporter_Test1_ShouldHaveActAndSequence()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "Act",
            "Choice: Kiss the banner",
            "Choice: Refuse.",
            "Goto: Refuse to kiss the banner",
            "END"
         };

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Acts[0].Choices[1].Triggers[0].Link.Should().Be("Refuse to kiss the banner");
      }


      [Test]
      public void StoryImporter_Test1_ShouldWork()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "Name: Test",
            "END"
         };

         string expectedResult = "Test";

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Header.Name.Should().Be(expectedResult);
      }

      [Test]
      public void StoryImporter_Test2_ShouldHaveActAndSequence()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "Act",
            "Choice: Kiss the banner",
            "Consequence: Player renown -1",
            "Choice: Refuse.",
            "Goto: Refuse to kiss the banner",
            "Sequence",
            "Name:       Refuse to kiss the banner",
            "Image:      None",
            "Intro:      Your captors wrap you in their banner and beat your body with clubs.",
            "Choice: Endure the beating.",
            "Consequence: Player health -10",
            "END"
         };


         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Acts[0].Choices[1].Triggers[0].Link.Should().Be("Refuse to kiss the banner");
         actualResult.Acts.Count.Should().Be(2);
      }


      [Test]
      public void StoryImporter_Test2_ShouldWork()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "OneTimeStory: no",
            "END"
         };

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Header.CanBePlayedOnlyOnce.Should().BeFalse();
      }


      [Test]
      public void StoryImporter_Test3_ShouldWork()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "DependOn: None",
            "END"
         };

         string expectedResult = "None";

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Header.DependOn.Should().Be(expectedResult);
      }

      [Test]
      public void StoryImporter_Test4_ShouldWork()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "Time: Night",
            "END"
         };

         GameTime expectedResult = GameTime.NIGHTTIME;

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Header.Time.Should().Be(expectedResult);
      }


      [Test]
      public void StoryImporter_Test5_ShouldWork()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "StoryType: is captive",
            "END"
         };

         StoryType expectedResult = StoryType.PLAYER_IS_CAPTIVE;

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Header.TypeOfStory.Should().Be(expectedResult);
      }

      [Test]
      public void StoryImporter_Test6_ShouldWork()
      {
         // Arrange
         StoryImporter sut = new StoryImporter();

         List<string> param = new List<string> {
            "STORY",
            "Restriction: age > 18",
            "END"
         };

         Evaluation expectedResult = new Evaluation {
            Operator = Operator.GREATERTHAN,
            Characteristic = Characteristics.AGE,
            Value = "18",
            Subject = Actor.PLAYER
         };

         // Act
         IStory actualResult = sut.ImportFrom(param.ToArray());

         // Assert
         actualResult.Restrictions[0].Characteristic.Should().Be(expectedResult.Characteristic);
         actualResult.Restrictions[0].Operator.Should().Be(expectedResult.Operator);
         actualResult.Restrictions[0].Value.Should().Be(expectedResult.Value);
         actualResult.Restrictions[0].Subject.Should().Be(expectedResult.Subject);
      }
   }
}