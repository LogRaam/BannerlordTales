// Code written by Gabriel Mailhot, 11/09/2020.

#region

using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TalesContract;
using TalesEntities.Stories;
using TalesEntities.TW;
using TalesEnums;
using TalesPersistence.Stories;

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
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "END"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Should().BeNull();
        }

        [Test]
        public void GivenStoryActChoiceOperatorGreaterTo_WhenImport_ThenIHaveAnOperatorGreaterTo()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Act",
                "Choice: a choice...",
                "Condition: health is equal to 100%",
                "END"
            };
            var expecText = "a choice...";
            var expectedResult = new BaseStory
            {
                Acts =
                {
                    new BaseAct
                    {
                        Choices =
                        {
                            new BaseChoice
                            {
                                Text = "a choice...",
                                Conditions =
                                {
                                    new BaseEvaluation
                                    {
                                        Operator = Operator.EQUALTO, Characteristic = Characteristics.HEALTH, Value = "100", ValueIsPercentage = true
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Text.Should().Be(expecText);
            actualResult.Acts[0].Choices[0].Conditions[0].Operator.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Operator);
            actualResult.Acts[0].Choices[0].Conditions[0].Characteristic.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Characteristic);
            actualResult.Acts[0].Choices[0].Conditions[0].Value.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Value);
            actualResult.Acts[0].Choices[0].Conditions[0].ValueIsPercentage.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].ValueIsPercentage);
        }


        [Test]
        public void GivenStoryActHaveIdEnd_WhenImport_ThenIHaveAPropertyIdentifyingItsDependance()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Act",
                "Choice: this is a choice.",
                "Id:             AnID",
                "END"
            };

            var expectedResult = "AnID";

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Id.Should().Be(expectedResult);
        }

        [Test]
        public void GivenStoryActIntroMultilineEnd_WhenImport_ThenIHaveAnIntroThatIncludeAllLines()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Act",
                "Intro: This is a multiline text",
                "and here is the second line.",
                "END"
            };

            var expectedResult = "This is a multiline text and here is the second line.";

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Intro.Should().Be(expectedResult);
        }

        [Test]
        public void GivenStoryActNameEnd_WhenImport_ThenIHaveAnEventWithAName()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "ACT", "Name:   FirstAct", "END"
            };
            var expectedResult = "FirstAct";

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Name.Should().Be(expectedResult);
        }

        [Test]
        public void GivenStoryActTimeDayEnd_WhenImport_ThenIHaveAnActThatCanBePlayedAtDay()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "ACT",
                "Choice: This is a choice",
                "Condition: Time = Day",
                "END"
            };

            var expectedResult = GameTime.DAYTIME;

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Conditions[0].Time.Should().Be(expectedResult);
        }

        [Test]
        public void GivenStoryActTimeNightEnd_WhenImport_ThenIHaveAnActThatCanBePlayedAtNight()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "ACT",
                "Choice: This is a choice",
                "Condition: Time = Night",
                "END"
            };

            var expectedResult = GameTime.NIGHTTIME;

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Conditions[0].Time.Should().Be(expectedResult);
        }


        [Test]
        public void GivenStoryChoiceConditionNpcHealthGreaterThanVerbose20PercentEnd_WhenImport_ThenIHaveAnEventWithProperResults()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "ACT",
                "Choice: This is another text.",
                "Condition: health greater than 20% ",
                "END"
            };
            var textResult = "This is another text.";
            var expectedResult = new BaseEvaluation
            {
                Operator = Operator.GREATERTHAN, Characteristic = Characteristics.HEALTH, Value = "20", ValueIsPercentage = true
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

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
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "ACT",
                "Choice: This is another text.",
                "Condition:       health > 20% ",
                "END"
            };
            var textResult = "This is another text.";
            var expectedResult = new BaseEvaluation
            {
                Operator = Operator.GREATERTHAN, Characteristic = Characteristics.HEALTH, Value = "20", ValueIsPercentage = true
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

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
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "ACT",
                "Choice: This is another text.",
                "Condition:       health greater than 20% ",
                "END"
            };
            var textResult = "This is another text.";
            var expectedResult = new BaseEvaluation
            {
                Operator = Operator.GREATERTHAN, Characteristic = Characteristics.HEALTH, Value = "20", ValueIsPercentage = true
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

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
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "ACT",
                "Choice: This is another text.",
                "Condition:       health is Lower than 20% ",
                "END"
            };
            var textResult = "This is another text.";
            var expectedResult = new BaseEvaluation
            {
                Operator = Operator.LOWERTHAN, Characteristic = Characteristics.HEALTH, Value = "20", ValueIsPercentage = true
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

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
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "ACT",
                "Choice: This is another text.",
                "Consequence:     Health = R -10 -20",
                "END"
            };
            var textResult = "This is another text.";
            var expectedResult = new BaseEvaluation
            {
                Operator = Operator.EQUALTO, Characteristic = Characteristics.HEALTH, RandomStart = -10, RandomEnd = -20
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

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
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "ACT",
                "Choice: This is another text.",
                "Consequence:     Health = between -10 -20",
                "END"
            };
            var textResult = "This is another text.";
            var expectedResult = new BaseEvaluation
            {
                Operator = Operator.EQUALTO, Characteristic = Characteristics.HEALTH, RandomStart = -10, RandomEnd = -20
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

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
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "ACT", "Choice: This is another text.", "END"
            };
            var expectedResult = "This is another text.";

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Text.Should().Be(expectedResult);
        }

        [Test]
        public void GivenStoryChoiceTrigger50PercentEnd_WhenImport_ThenIHaveAnEventWithProperResults()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "ACT",
                "Choice: This is another text.",
                "GoTo:        GoToNextAct 50%",
                "END"
            };
            var textResult = "This is another text.";
            var expectedResult = new BaseTrigger
            {
                Link = "GoToNextAct", ChanceToTrigger = 50
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Text.Should().Be(textResult);
            actualResult.Acts[0].Choices[0].Triggers[0].Link.Should().Be(expectedResult.Link);
            actualResult.Acts[0].Choices[0].Triggers[0].ChanceToTrigger.Should().Be(expectedResult.ChanceToTrigger);
        }

        [Test]
        public void GivenStoryChoiceTriggerEnd_WhenImport_ThenIHaveAnEventWithProperResults()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "ACT",
                "Choice: This is another text.",
                "Goto:        GoToAnotherAct",
                "END"
            };
            var textResult = "This is another text.";
            var expectedResult = new BaseTrigger
            {
                Link = "GoToAnotherAct", ChanceToTrigger = 100
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Text.Should().Be(textResult);
            actualResult.Acts[0].Choices[0].Triggers[0].Link.Should().Be(expectedResult.Link);
            actualResult.Acts[0].Choices[0].Triggers[0].ChanceToTrigger.Should().Be(expectedResult.ChanceToTrigger);
        }

        [Test]
        public void GivenStoryDependOnEnd_WhenImport_ThenIHaveAPropertyIdentifyingItsDependance()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "DependOn: ActID", "END"
            };

            var expectedResult = "ActID";

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Header.DependOn.Should().Be(expectedResult);
        }

        [Test]
        public void GivenStoryEnd_WhenImport_ThenIHaveAStory()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "ACT", "END"
            };
            var expectedResult = typeof(BaseStory);

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Should().BeOfType(expectedResult);
        }

        [Test]
        public void GivenStoryImageEnd_WhenImport_ThenIHaveAnEventWithImage()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "ACT", "Image:   ImageName", "END"
            };
            var expectedResult = "ImageName";

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Image.Should().Be(expectedResult);
        }

        [Test]
        public void GivenStoryIntroEnd_WhenImport_ThenIHaveAnEventWithIntro()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "ACT", "Intro:  This is a text for the event.", "END"
            };
            var expectedResult = "This is a text for the event.";

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Intro.Should().Be(expectedResult);
        }

        [Test]
        public void GivenStoryLocationMapEnd_WhenImport_ThenIHaveAStoryWithLocationMap()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "Act", "Location: Map", "END"
            };
            var expectedResult = Location.MAP;

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Location.Should().Be(expectedResult);
        }

        [Test]
        public void GivenStoryOneTimeStoryFalseEnd_WhenImport_ThenIHaveAStoryThatCanBePlayedOnlyOnce()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "OneTimeStory: False", "END"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Header.CanBePlayedOnlyOnce.Should().BeFalse();
        }

        [Test]
        public void GivenStoryOneTimeStoryNoEnd_WhenImport_ThenIHaveAStoryThatCanBePlayedOnlyOnce()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "OneTimeStory: No", "END"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Header.CanBePlayedOnlyOnce.Should().BeFalse();
        }

        [Test]
        public void GivenStoryOneTimeStoryTrueEnd_WhenImport_ThenIHaveAStoryThatCanBePlayedOnlyOnce()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "OneTimeStory: True", "END"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Header.CanBePlayedOnlyOnce.Should().BeTrue();
        }

        [Test]
        public void GivenStoryOneTimeStoryYesEnd_WhenImport_ThenIHaveAStoryThatCanBePlayedOnlyOnce()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "OneTimeStory: Yes", "END"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Header.CanBePlayedOnlyOnce.Should().BeTrue();
        }

        [Test]
        public void GivenStoryRestrictionPlayerAgeGT18End_WhenImport_ThenIHaveAStoryWithProperRestriction()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "Restriction:  age > 18", "END"
            };
            var expectedResult = new BaseEvaluation
            {
                Operator = Operator.GREATERTHAN, Characteristic = Characteristics.AGE, Value = "18"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Restrictions[0].Operator.Should().Be(expectedResult.Operator);
            actualResult.Restrictions[0].Value.Should().Be(expectedResult.Value);
            actualResult.Restrictions[0].Characteristic.Should().Be(expectedResult.Characteristic);
        }

        [Test]
        public void GivenStoryRestrictionPlayerGenderEqualToFemaleEnd_WhenImport_ThenIHaveAStoryWithProperRestriction()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "Restriction:  gender = female", "END"
            };
            var expectedResult = new BaseEvaluation
            {
                Operator = Operator.EQUALTO, Characteristic = Characteristics.GENDER, Value = "FEMALE"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Restrictions[0].Operator.Should().Be(expectedResult.Operator);
            actualResult.Restrictions[0].Value.Should().Be(expectedResult.Value);
            actualResult.Restrictions[0].Characteristic.Should().Be(expectedResult.Characteristic);
        }

        [Test]
        public void GivenStoryRestrictionPlayerValorLowerThan30End_WhenImport_ThenIHaveAStoryWithProperRestriction()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "Restriction:  Valor < 30", "END"
            };

            var expectedResult = new BaseEvaluation
            {
                Operator = Operator.LOWERTHAN, PersonalityTrait = PersonalityTraits.VALOR, Value = "30"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Restrictions[0].Operator.Should().Be(expectedResult.Operator);
            actualResult.Restrictions[0].Value.Should().Be(expectedResult.Value);
            actualResult.Restrictions[0].PersonalityTrait.Should().Be(expectedResult.PersonalityTrait);
        }


        [Test]
        public void StoryImporter_Test1_ShouldHaveActAndSequence()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Act",
                "Choice: Kiss the banner",
                "Choice: Refuse.",
                "Goto: Refuse to kiss the banner",
                "END"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[1].Triggers[0].Link.Should().Be("Refuse to kiss the banner");
        }


        [Test]
        public void StoryImporter_Test1_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "Name: Test", "END"
            };

            var expectedResult = "Test";

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Header.Name.Should().Be(expectedResult);
        }


        [Test]
        public void StoryImporter_Test10_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "Restriction: npc gender is male", "END"
            };

            var expectedResult = new BaseEvaluation
            {
                Operator = Operator.EQUALTO, Characteristic = Characteristics.GENDER, Value = "MALE", Subject = Actor.NPC
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Restrictions[0].Characteristic.Should().Be(expectedResult.Characteristic);
            actualResult.Restrictions[0].Operator.Should().Be(expectedResult.Operator);
            actualResult.Restrictions[0].Value.Should().Be(expectedResult.Value);
            actualResult.Restrictions[0].Subject.Should().Be(expectedResult.Subject);
        }


        [Test]
        public void StoryImporter_Test11_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "restriction: npc culture is nord", "END"
            };

            var expectedResult = new BaseEvaluation
            {
                Operator = Operator.EQUALTO, Characteristic = Characteristics.CULTURE, Value = "NORD", Subject = Actor.NPC
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Restrictions[0].Characteristic.Should().Be(expectedResult.Characteristic);
            actualResult.Restrictions[0].Operator.Should().Be(expectedResult.Operator);
            actualResult.Restrictions[0].Value.Should().Be(expectedResult.Value);
            actualResult.Restrictions[0].Subject.Should().Be(expectedResult.Subject);
        }


        [Test]
        public void StoryImporter_Test12_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "restriction: npc is lord", "END"
            };

            var expectedResult = new BaseEvaluation
            {
                Operator = Operator.EQUALTO, Characteristic = Characteristics.OCCUPATION, Value = "LORD", Subject = Actor.NPC
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Restrictions[0].Characteristic.Should().Be(expectedResult.Characteristic);
            actualResult.Restrictions[0].Operator.Should().Be(expectedResult.Operator);
            actualResult.Restrictions[0].Value.Should().Be(expectedResult.Value);
            actualResult.Restrictions[0].Subject.Should().Be(expectedResult.Subject);
        }

        [Test]
        public void StoryImporter_Test13_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Act",
                "Name:       My Act 1",
                "Image:      None",
                "Location:   Map",
                "Intro:      This is an intro for act 1.",
                "END"
            };

            var expectedResult = new BaseStory
            {
                Acts = new List<IAct>
                {
                    new Act
                    {
                        Name = "My Act 1", Image = "None", Location = Location.MAP, Intro = "This is an intro for act 1."
                    }
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Name.Should().Be(expectedResult.Acts[0].Name);
            actualResult.Acts[0].Image.Should().Be(expectedResult.Acts[0].Image);
            actualResult.Acts[0].Location.Should().Be(expectedResult.Acts[0].Location);
            actualResult.Acts[0].Intro.Should().Be(expectedResult.Acts[0].Intro);
        }


        [Test]
        public void StoryImporter_Test14_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "Act", "Choice: Act 1 choice 1.", "END"
            };

            var expectedResult = new BaseStory
            {
                Acts = new List<IAct>
                {
                    new Act
                    {
                        Choices = new List<IChoice>
                        {
                            new BaseChoice
                            {
                                Text = "Act 1 choice 1."
                            }
                        }
                    }
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Text.Should().Be(expectedResult.Acts[0].Choices[0].Text);
        }

        [Test]
        public void StoryImporter_Test15_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Act",
                "Choice: This is an act with a choice.",
                "Condition: vigor > 1",
                "END"
            };

            var expectedResult = new BaseStory
            {
                Acts = new List<IAct>
                {
                    new Act
                    {
                        Choices = new List<IChoice>
                        {
                            new BaseChoice
                            {
                                Text = "This is an act with a choice.",
                                Conditions = new List<IEvaluation>
                                {
                                    new BaseEvaluation
                                    {
                                        Attribute = Attributes.VIGOR, Operator = Operator.GREATERTHAN, Value = "1"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Text.Should().Be(expectedResult.Acts[0].Choices[0].Text);
            actualResult.Acts[0].Choices[0].Conditions[0].Attribute.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Attribute);
            actualResult.Acts[0].Choices[0].Conditions[0].Operator.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Operator);
            actualResult.Acts[0].Choices[0].Conditions[0].Value.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Value);
        }

        [Test]
        public void StoryImporter_Test16_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Act",
                "Choice: This is an act with a choice.",
                "Condition: endurance = 3",
                "END"
            };

            var expectedResult = new BaseStory
            {
                Acts = new List<IAct>
                {
                    new Act
                    {
                        Choices = new List<IChoice>
                        {
                            new BaseChoice
                            {
                                Text = "This is an act with a choice.",
                                Conditions = new List<IEvaluation>
                                {
                                    new BaseEvaluation
                                    {
                                        Attribute = Attributes.ENDURANCE, Operator = Operator.EQUALTO, Value = "3"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Text.Should().Be(expectedResult.Acts[0].Choices[0].Text);
            actualResult.Acts[0].Choices[0].Conditions[0].Attribute.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Attribute);
            actualResult.Acts[0].Choices[0].Conditions[0].Operator.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Operator);
            actualResult.Acts[0].Choices[0].Conditions[0].Value.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Value);
        }


        [Test]
        public void StoryImporter_Test17_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Act",
                "Choice: This is an act with a choice.",
                "Condition: control < 2",
                "END"
            };

            var expectedResult = new BaseStory
            {
                Acts = new List<IAct>
                {
                    new Act
                    {
                        Choices = new List<IChoice>
                        {
                            new BaseChoice
                            {
                                Text = "This is an act with a choice.",
                                Conditions = new List<IEvaluation>
                                {
                                    new BaseEvaluation
                                    {
                                        Attribute = Attributes.CONTROL, Operator = Operator.LOWERTHAN, Value = "2"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Text.Should().Be(expectedResult.Acts[0].Choices[0].Text);
            actualResult.Acts[0].Choices[0].Conditions[0].Attribute.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Attribute);
            actualResult.Acts[0].Choices[0].Conditions[0].Operator.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Operator);
            actualResult.Acts[0].Choices[0].Conditions[0].Value.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Value);
        }

        [Test]
        public void StoryImporter_Test18_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Act",
                "Choice: This is an act with a choice.",
                "Condition: cunning > 4",
                "Condition: social < 5",
                "Condition: intelligence = 6",
                "END"
            };

            var expectedResult = new BaseStory
            {
                Acts = new List<IAct>
                {
                    new Act
                    {
                        Choices = new List<IChoice>
                        {
                            new BaseChoice
                            {
                                Text = "This is an act with a choice.",
                                Conditions = new List<IEvaluation>
                                {
                                    new BaseEvaluation
                                    {
                                        Attribute = Attributes.CUNNING, Operator = Operator.GREATERTHAN, Value = "4"
                                    },
                                    new BaseEvaluation
                                    {
                                        Attribute = Attributes.SOCIAL, Operator = Operator.LOWERTHAN, Value = "5"
                                    },
                                    new BaseEvaluation
                                    {
                                        Attribute = Attributes.INTELLIGENCE, Operator = Operator.EQUALTO, Value = "6", ValueIsPercentage = false
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Text.Should().Be(expectedResult.Acts[0].Choices[0].Text);

            actualResult.Acts[0].Choices[0].Conditions[0].Attribute.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Attribute);
            actualResult.Acts[0].Choices[0].Conditions[0].Operator.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Operator);
            actualResult.Acts[0].Choices[0].Conditions[0].Value.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Value);

            actualResult.Acts[0].Choices[0].Conditions[1].Attribute.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[1].Attribute);
            actualResult.Acts[0].Choices[0].Conditions[1].Operator.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[1].Operator);
            actualResult.Acts[0].Choices[0].Conditions[1].Value.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[1].Value);

            actualResult.Acts[0].Choices[0].Conditions[2].Attribute.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[2].Attribute);
            actualResult.Acts[0].Choices[0].Conditions[2].Operator.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[2].Operator);
            actualResult.Acts[0].Choices[0].Conditions[2].Value.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[2].Value);
            actualResult.Acts[0].Choices[0].Conditions[0].ValueIsPercentage.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].ValueIsPercentage);
        }


        [Test]
        public void StoryImporter_Test19_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Act",
                "Choice: This is an act with a choice.",
                "Condition: health > 20%",
                "END"
            };

            var expectedResult = new BaseStory
            {
                Acts = new List<IAct>
                {
                    new Act
                    {
                        Choices = new List<IChoice>
                        {
                            new BaseChoice
                            {
                                Text = "This is an act with a choice.",
                                Conditions = new List<IEvaluation>
                                {
                                    new BaseEvaluation
                                    {
                                        Characteristic = Characteristics.HEALTH, Operator = Operator.GREATERTHAN, Value = "20", ValueIsPercentage = true
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Text.Should().Be(expectedResult.Acts[0].Choices[0].Text);
            actualResult.Acts[0].Choices[0].Conditions[0].Attribute.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Attribute);
            actualResult.Acts[0].Choices[0].Conditions[0].Operator.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Operator);
            actualResult.Acts[0].Choices[0].Conditions[0].Value.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Value);
            actualResult.Acts[0].Choices[0].Conditions[0].ValueIsPercentage.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].ValueIsPercentage);
        }


        [Test]
        public void StoryImporter_Test2_ShouldHaveActAndSequence()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
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
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[1].Triggers[0].Link.Should().Be("Refuse to kiss the banner");
            actualResult.Sequences.Count.Should().Be(1);
        }


        [Test]
        public void StoryImporter_Test2_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "OneTimeStory: no", "END"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Header.CanBePlayedOnlyOnce.Should().BeFalse();
        }


        [Test]
        public void StoryImporter_Test20_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Act",
                "Choice: This is an act with a choice.",
                "Condition: health > 20%",
                "Consequence: renown -1",
                "Consequence: health R -1 -10",
                "Consequence: risk of becoming pregnant",
                "END"
            };

            var expectedResult = new BaseStory
            {
                Acts = new List<IAct>
                {
                    new Act
                    {
                        Choices = new List<IChoice>
                        {
                            new BaseChoice
                            {
                                Text = "This is an act with a choice.",
                                Conditions = new List<IEvaluation>
                                {
                                    new BaseEvaluation
                                    {
                                        Characteristic = Characteristics.HEALTH,
                                        Operator = Operator.GREATERTHAN,
                                        Value = "20",
                                        ValueIsPercentage = true,
                                        Subject = Actor.PLAYER
                                    }
                                },
                                Consequences = new List<IEvaluation>
                                {
                                    new BaseEvaluation
                                    {
                                        Operator = Operator.EQUALTO,
                                        Value = "-1",
                                        ValueIsPercentage = false,
                                        Characteristic = Characteristics.RENOWN,
                                        Subject = Actor.PLAYER
                                    },
                                    new BaseEvaluation
                                    {
                                        Operator = Operator.EQUALTO,
                                        RandomStart = -1,
                                        RandomEnd = -10,
                                        ValueIsPercentage = false,
                                        Characteristic = Characteristics.HEALTH,
                                        Subject = Actor.PLAYER
                                    },
                                    new BaseEvaluation
                                    {
                                        PregnancyRisk = true, Subject = Actor.PLAYER
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Text.Should().Be(expectedResult.Acts[0].Choices[0].Text);
            actualResult.Acts[0].Choices[0].Conditions[0].Attribute.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Attribute);
            actualResult.Acts[0].Choices[0].Conditions[0].Operator.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Operator);
            actualResult.Acts[0].Choices[0].Conditions[0].Value.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Value);
            actualResult.Acts[0].Choices[0].Conditions[0].ValueIsPercentage.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].ValueIsPercentage);
            actualResult.Acts[0].Choices[0].Conditions[0].Subject.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Subject);

            actualResult.Acts[0].Choices[0].Consequences[0].Attribute.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[0].Attribute);
            actualResult.Acts[0].Choices[0].Consequences[0].Operator.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[0].Operator);
            actualResult.Acts[0].Choices[0].Consequences[0].Value.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[0].Value);
            actualResult.Acts[0].Choices[0].Consequences[0].ValueIsPercentage.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[0].ValueIsPercentage);
            actualResult.Acts[0].Choices[0].Consequences[0].Subject.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[0].Subject);

            actualResult.Acts[0].Choices[0].Consequences[1].Attribute.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[1].Attribute);
            actualResult.Acts[0].Choices[0].Consequences[1].Operator.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[1].Operator);
            actualResult.Acts[0].Choices[0].Consequences[1].RandomStart.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[1].RandomStart);
            actualResult.Acts[0].Choices[0].Consequences[1].RandomEnd.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[1].RandomEnd);
            actualResult.Acts[0].Choices[0].Consequences[1].ValueIsPercentage.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[1].ValueIsPercentage);
            actualResult.Acts[0].Choices[0].Consequences[1].Subject.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[1].Subject);

            actualResult.Acts[0].Choices[0].Consequences[2].PregnancyRisk.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[2].PregnancyRisk);
            actualResult.Acts[0].Choices[0].Consequences[2].Subject.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[2].Subject);
        }


        [Test]
        public void StoryImporter_Test21_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Act",
                "Choice: This is an act with a choice.",
                "Consequence: NPC can get pregnant",
                "END"
            };

            var expectedResult = new BaseStory
            {
                Acts = new List<IAct>
                {
                    new Act
                    {
                        Choices = new List<IChoice>
                        {
                            new BaseChoice
                            {
                                Text = "This is an act with a choice.",
                                Consequences = new List<IEvaluation>
                                {
                                    new BaseEvaluation
                                    {
                                        PregnancyRisk = true, Subject = Actor.NPC
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Consequences[0].PregnancyRisk.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[0].PregnancyRisk);
            actualResult.Acts[0].Choices[0].Consequences[0].Subject.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[0].Subject);
        }

        [Test]
        public void StoryImporter_Test22_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Act",
                "Name:       My Act",
                "Choice: This is an act with a choice.",
                "Sequence",
                "Name:       My Seq",
                "Choice: This is another act with a choice.",
                "Consequence: health -10",
                "END"
            };

            var expectedResult = new BaseStory
            {
                Acts = new List<IAct>
                {
                    new Act
                    {
                        Name = "My Act",
                        Choices = new List<IChoice>
                        {
                            new BaseChoice
                            {
                                Text = "This is an act with a choice."
                            }
                        }
                    }
                },
                Sequences = new List<ISequence>
                {
                    new Sequence
                    {
                        Name = "My Seq",
                        Choices = new List<IChoice>
                        {
                            new BaseChoice
                            {
                                Text = "This is another act with a choice.",
                                Consequences = new List<IEvaluation>
                                {
                                    new BaseEvaluation
                                    {
                                        Subject = Actor.PLAYER, Characteristic = Characteristics.HEALTH, Operator = Operator.EQUALTO, Value = "-10"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Name.Should().Be(expectedResult.Acts[0].Name);
            actualResult.Acts[0].Choices[0].Text.Should().Be(expectedResult.Acts[0].Choices[0].Text);
            actualResult.Sequences[0].Name.Should().Be(expectedResult.Sequences[0].Name);
            actualResult.Sequences[0].Choices[0].Text.Should().Be(expectedResult.Sequences[0].Choices[0].Text);
            actualResult.Sequences[0].Choices[0].Consequences[0].Subject.Should().Be(expectedResult.Sequences[0].Choices[0].Consequences[0].Subject);
            actualResult.Sequences[0].Choices[0].Consequences[0].Characteristic.Should().Be(expectedResult.Sequences[0].Choices[0].Consequences[0].Characteristic);
            actualResult.Sequences[0].Choices[0].Consequences[0].Operator.Should().Be(expectedResult.Sequences[0].Choices[0].Consequences[0].Operator);
            actualResult.Sequences[0].Choices[0].Consequences[0].Value.Should().Be(expectedResult.Sequences[0].Choices[0].Consequences[0].Value);
        }


        [Test]
        public void StoryImporter_Test23_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Act A",
                "Choice: A",
                "Sequence B",
                "Choice: B",
                "Act C",
                "Choice: C",
                "Sequence D",
                "Choice: D",
                "Sequence E",
                "Choice: E",
                "END"
            };

            var expectedResult = new BaseStory();
            expectedResult.Acts.Add(new Act
            {
                Choices = new List<IChoice>
                {
                    new BaseChoice
                    {
                        Text = "A"
                    }
                }
            });
            expectedResult.Acts.Add(new Act
            {
                Choices = new List<IChoice>
                {
                    new BaseChoice
                    {
                        Text = "C"
                    }
                }
            });
            expectedResult.Sequences.Add(new Sequence
            {
                Choices = new List<IChoice>
                {
                    new BaseChoice
                    {
                        Text = "B"
                    }
                }
            });
            expectedResult.Sequences.Add(new Sequence
            {
                Choices = new List<IChoice>
                {
                    new BaseChoice
                    {
                        Text = "D"
                    }
                }
            });
            expectedResult.Sequences.Add(new Sequence
            {
                Choices = new List<IChoice>
                {
                    new BaseChoice
                    {
                        Text = "E"
                    }
                }
            });

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Text.Should().Be(expectedResult.Acts[0].Choices[0].Text);
            actualResult.Acts[1].Choices[0].Text.Should().Be(expectedResult.Acts[1].Choices[0].Text);
            actualResult.Sequences[1].Choices[0].Text.Should().Be(expectedResult.Sequences[1].Choices[0].Text);
            actualResult.Sequences[1].Choices[0].Text.Should().Be(expectedResult.Sequences[1].Choices[0].Text);
            actualResult.Sequences[1].Choices[0].Text.Should().Be(expectedResult.Sequences[1].Choices[0].Text);
        }

        [Test]
        public void StoryImporter_Test24_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Name: MyStory",
                "Act",
                "Name: MyAct",
                "END"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].ParentStory.Should().Be("MyStory");
            actualResult.Acts[0].Name.Should().Be("MyAct");
        }


        [Test]
        public void StoryImporter_Test3_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "DependOn: None", "END"
            };

            var expectedResult = "None";

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Header.DependOn.Should().Be(expectedResult);
        }

        [Test]
        public void StoryImporter_Test4_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "Time: Night", "END"
            };

            var expectedResult = GameTime.NIGHTTIME;

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Header.Time.Should().Be(expectedResult);
        }


        [Test]
        public void StoryImporter_Test5_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "StoryType: is captive", "END"
            };

            var expectedResult = StoryType.PLAYER_IS_CAPTIVE;

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Header.TypeOfStory.Should().Be(expectedResult);
        }

        [Test]
        public void StoryImporter_Test6_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "Restriction: age > 18", "END"
            };

            var expectedResult = new BaseEvaluation
            {
                Operator = Operator.GREATERTHAN, Characteristic = Characteristics.AGE, Value = "18", Subject = Actor.PLAYER
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Restrictions[0].Characteristic.Should().Be(expectedResult.Characteristic);
            actualResult.Restrictions[0].Operator.Should().Be(expectedResult.Operator);
            actualResult.Restrictions[0].Value.Should().Be(expectedResult.Value);
            actualResult.Restrictions[0].Subject.Should().Be(expectedResult.Subject);
        }


        [Test]
        public void StoryImporter_Test7_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "Restriction: gender is female", "END"
            };

            var expectedResult = new BaseEvaluation
            {
                Operator = Operator.EQUALTO, Characteristic = Characteristics.GENDER, Value = "FEMALE", Subject = Actor.PLAYER
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Restrictions[0].Characteristic.Should().Be(expectedResult.Characteristic);
            actualResult.Restrictions[0].Operator.Should().Be(expectedResult.Operator);
            actualResult.Restrictions[0].Value.Should().Be(expectedResult.Value);
            actualResult.Restrictions[0].Subject.Should().Be(expectedResult.Subject);
        }

        [Test]
        public void StoryImporter_Test8_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "restriction: culture is empire", "END"
            };

            var expectedResult = new BaseEvaluation
            {
                Operator = Operator.EQUALTO, Characteristic = Characteristics.CULTURE, Value = "EMPIRE", Subject = Actor.PLAYER
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Restrictions[0].Characteristic.Should().Be(expectedResult.Characteristic);
            actualResult.Restrictions[0].Operator.Should().Be(expectedResult.Operator);
            actualResult.Restrictions[0].Value.Should().Be(expectedResult.Value);
            actualResult.Restrictions[0].Subject.Should().Be(expectedResult.Subject);
        }


        [Test]
        public void StoryImporter_Test9_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY", "Restriction: npc age > 18", "END"
            };

            var expectedResult = new BaseEvaluation
            {
                Operator = Operator.GREATERTHAN, Characteristic = Characteristics.AGE, Value = "18", Subject = Actor.NPC
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Restrictions[0].Characteristic.Should().Be(expectedResult.Characteristic);
            actualResult.Restrictions[0].Operator.Should().Be(expectedResult.Operator);
            actualResult.Restrictions[0].Value.Should().Be(expectedResult.Value);
            actualResult.Restrictions[0].Subject.Should().Be(expectedResult.Subject);
        }
    }
}