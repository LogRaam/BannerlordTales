// Code written by Gabriel Mailhot, 02/12/2023.

#region

using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using TalesBase.Stories;
using TalesBase.Stories.Evaluation;
using TalesBase.TW;
using TalesContract;
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
        public void ActConsequence_GiveArmorTatteredRags_ShouldWorks()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Act",
                "Choice: this is a choice.",
                "Consequence: give armor tattered_rags",
                "END"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            var test = actualResult.Acts[0].Choices[0].Consequences[0];
            test.Outcome.ShouldEquip.Should().BeTrue();
            test.Equipments.Armor.Should().Be("tattered_rags");
        }

        [Test]
        public void ActConsequence_GiveArmorUnspecified_ShouldWorks()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Act",
                "Choice: this is a choice.",
                "Consequence: give armor culture Sturgia, material leather, appearance < 2",
                "END"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            var test = actualResult.Acts[0].Choices[0].Consequences[0];
            test.Outcome.ShouldEquip.Should().BeTrue();
            test.Equipments.Culture.Should().Be(CultureCode.Sturgia);
            test.Equipments.Material.Should().Be(ArmorMaterialTypes.Leather);
            test.Equipments.Appearance.Should().Be(2);
            test.Numbers.Operator.Should().Be(Operator.Lowerthan);
        }

        [Test]
        public void ActConsequence_GiveArmorUnspecified2_ShouldWorks()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Act",
                "Choice: this is a choice.",
                "Consequence: give armor culture Sturgia, appearance > 2, material leather",
                "END"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            var test = actualResult.Acts[0].Choices[0].Consequences[0];
            test.Outcome.ShouldEquip.Should().BeTrue();
            test.Equipments.Culture.Should().Be(CultureCode.Sturgia);
            test.Equipments.Material.Should().Be(ArmorMaterialTypes.Leather);
            test.Equipments.Appearance.Should().Be(2);
            test.Numbers.Operator.Should().Be(Operator.Greaterthan);
        }


        [Test]
        public void ActConsequence_GiveArmorUnspecified3_ShouldWorks()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Act",
                "Choice: this is a choice.",
                "Consequence: give armor appearance > 2",
                "END"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            var test = actualResult.Acts[0].Choices[0].Consequences[0];
            test.Outcome.ShouldEquip.Should().BeTrue();
            test.Equipments.Appearance.Should().Be(2);
            test.Numbers.Operator.Should().Be(Operator.Greaterthan);
        }

        [Test]
        public void ActConsequence_GiveArmorUnspecifiedWithoutOperator_ShouldWorks()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Act",
                "Choice: this is a choice.",
                "Consequence: give armor culture Sturgia, material leather, appearance 2",
                "END"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            var test = actualResult.Acts[0].Choices[0].Consequences[0];
            test.Outcome.ShouldEquip.Should().BeTrue();
            test.Equipments.Culture.Should().Be(CultureCode.Sturgia);
            test.Equipments.Material.Should().Be(ArmorMaterialTypes.Leather);
            test.Equipments.Appearance.Should().Be(2);
            test.Numbers.Operator.Should().Be(Operator.Equalto);
        }

        [Test]
        public void ActConsequence_GiveWeaponCultureSturgiaTypeDagger_ShouldWorks()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Act",
                "Choice: this is a choice.",
                "Consequence: give weapon culture Sturgia, type dagger",
                "END"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            var test = actualResult.Acts[0].Choices[0].Consequences[0];
            test.Outcome.ShouldEquip.Should().BeTrue();
            test.Equipments.Culture.Should().Be(CultureCode.Sturgia);
            test.Equipments.Weapon.Should().Be("dagger");
        }

        [Test]
        public void ActConsequence_GiveWeaponSeax_ShouldWorks()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Act",
                "Choice: this is a choice.",
                "Consequence: give weapon seax",
                "END"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            var test = actualResult.Acts[0].Choices[0].Consequences[0];
            test.Outcome.ShouldEquip.Should().BeTrue();
            test.Equipments.Weapon.Should().Be("SEAX");
        }

        [Test]
        public void ActConsequence_RemoveClothes_ShouldWorks()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Name: aStoryName",
                "Act",
                "Name: aName",
                "Choice: this is a choice.",
                "Consequence: strip player",
                "END"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Consequences[0].Outcome.ShouldUndress.Should().BeTrue();
        }


        [Test]
        public void ActConsequence_RemoveClothes2_ShouldWorks()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Name: aStoryName",
                "Act",
                "Name: aName",
                "Choice: this is a choice.",
                "Consequence: remove clothes",
                "END"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Consequences[0].Outcome.ShouldUndress.Should().BeTrue();
        }

        [Test]
        public void ActConsequence_RemoveClothes3_ShouldWorks()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Name: aStoryName",
                "Act",
                "Name: aName",
                "Choice: this is a choice.",
                "Consequence: undress",
                "END"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Consequences[0].Outcome.ShouldUndress.Should().BeTrue();
        }


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
                                        Persona = new Persona
                                        {
                                            Characteristic = Characteristics.Health
                                        },
                                        Numbers = new Numbers
                                        {
                                            Operator = Operator.Equalto,
                                            Value = "100",
                                            ValueIsPercentage = true
                                        }
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
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.Operator.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Numbers.Operator);
            actualResult.Acts[0].Choices[0].Conditions[0].Persona.Characteristic.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Persona.Characteristic);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.Value.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Numbers.Value);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.ValueIsPercentage.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Numbers.ValueIsPercentage);
        }


        [Test]
        public void GivenStoryActHaveIdEnd_WhenImport_ThenIHaveAPropertyIdentifyingItsDependance()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Name: aStoryName",
                "Act",
                "Name: aName",
                "Choice: this is a choice.",
                "END"
            };

            var expectedResult = "aStoryName_aName_thisisachoice.";

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
                "STORY",
                "ACT",
                "Name:   FirstAct",
                "END"
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

            var expectedResult = GameTime.Daytime;

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

            var expectedResult = GameTime.Nighttime;

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
                Persona = new Persona
                {
                    Characteristic = Characteristics.Health
                },
                Numbers = new Numbers
                {
                    Operator = Operator.Greaterthan,
                    Value = "20",
                    ValueIsPercentage = true
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Text.Should().Be(textResult);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.Operator.Should().Be(expectedResult.Numbers.Operator);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.ValueIsPercentage.Should().Be(expectedResult.Numbers.ValueIsPercentage);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.Value.Should().Be(expectedResult.Numbers.Value);
            actualResult.Acts[0].Choices[0].Conditions[0].Persona.Characteristic.Should().Be(expectedResult.Persona.Characteristic);
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
                Persona = new Persona
                {
                    Characteristic = Characteristics.Health
                },
                Numbers = new Numbers
                {
                    Operator = Operator.Greaterthan,
                    Value = "20",
                    ValueIsPercentage = true
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Text.Should().Be(textResult);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.Operator.Should().Be(expectedResult.Numbers.Operator);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.ValueIsPercentage.Should().Be(expectedResult.Numbers.ValueIsPercentage);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.Value.Should().Be(expectedResult.Numbers.Value);
            actualResult.Acts[0].Choices[0].Conditions[0].Persona.Characteristic.Should().Be(expectedResult.Persona.Characteristic);
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
                Persona = new Persona
                {
                    Characteristic = Characteristics.Health
                },
                Numbers = new Numbers
                {
                    Operator = Operator.Greaterthan,
                    Value = "20",
                    ValueIsPercentage = true
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Text.Should().Be(textResult);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.Operator.Should().Be(expectedResult.Numbers.Operator);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.ValueIsPercentage.Should().Be(expectedResult.Numbers.ValueIsPercentage);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.Value.Should().Be(expectedResult.Numbers.Value);
            actualResult.Acts[0].Choices[0].Conditions[0].Persona.Characteristic.Should().Be(expectedResult.Persona.Characteristic);
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
                Persona = new Persona
                {
                    Characteristic = Characteristics.Health
                },
                Numbers = new Numbers
                {
                    Operator = Operator.Lowerthan,
                    Value = "20",
                    ValueIsPercentage = true
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Text.Should().Be(textResult);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.Operator.Should().Be(expectedResult.Numbers.Operator);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.ValueIsPercentage.Should().Be(expectedResult.Numbers.ValueIsPercentage);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.Value.Should().Be(expectedResult.Numbers.Value);
            actualResult.Acts[0].Choices[0].Conditions[0].Persona.Characteristic.Should().Be(expectedResult.Persona.Characteristic);
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
                Persona = new Persona
                {
                    Characteristic = Characteristics.Health
                },
                Numbers = new Numbers
                {
                    Operator = Operator.Equalto,
                    RandomStart = -10,
                    RandomEnd = -20
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Text.Should().Be(textResult);
            actualResult.Acts[0].Choices[0].Consequences[0].Numbers.Operator.Should().Be(expectedResult.Numbers.Operator);
            actualResult.Acts[0].Choices[0].Consequences[0].Numbers.ValueIsPercentage.Should().Be(expectedResult.Numbers.ValueIsPercentage);
            actualResult.Acts[0].Choices[0].Consequences[0].Numbers.RandomStart.Should().Be(expectedResult.Numbers.RandomStart);
            actualResult.Acts[0].Choices[0].Consequences[0].Numbers.RandomEnd.Should().Be(expectedResult.Numbers.RandomEnd);
            actualResult.Acts[0].Choices[0].Consequences[0].Persona.Characteristic.Should().Be(expectedResult.Persona.Characteristic);
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
                Persona = new Persona
                {
                    Characteristic = Characteristics.Health
                },
                Numbers = new Numbers
                {
                    Operator = Operator.Equalto,
                    RandomStart = -10,
                    RandomEnd = -20
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Text.Should().Be(textResult);
            actualResult.Acts[0].Choices[0].Consequences[0].Numbers.Operator.Should().Be(expectedResult.Numbers.Operator);
            actualResult.Acts[0].Choices[0].Consequences[0].Numbers.ValueIsPercentage.Should().Be(expectedResult.Numbers.ValueIsPercentage);
            actualResult.Acts[0].Choices[0].Consequences[0].Numbers.RandomStart.Should().Be(expectedResult.Numbers.RandomStart);
            actualResult.Acts[0].Choices[0].Consequences[0].Numbers.RandomEnd.Should().Be(expectedResult.Numbers.RandomEnd);
            actualResult.Acts[0].Choices[0].Consequences[0].Persona.Characteristic.Should().Be(expectedResult.Persona.Characteristic);
        }

        [Test]
        public void GivenStoryChoiceEnd_WhenImport_ThenIHaveAnEventWithProperResults()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "ACT",
                "Choice: This is another text.",
                "END"
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
                Link = "GoToNextAct",
                ChanceToTrigger = 50
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
                Link = "GoToAnotherAct",
                ChanceToTrigger = 100
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
                "STORY",
                "DependOn: ActID",
                "END"
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
                "STORY",
                "ACT",
                "END"
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
                "STORY",
                "ACT",
                "Image:   ImageName",
                "END"
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
                "STORY",
                "ACT",
                "Intro:  This is a text for the event.",
                "END"
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
                "STORY",
                "Act",
                "Location: Map",
                "END"
            };
            var expectedResult = Location.Map;

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
                "STORY",
                "OneTimeStory: False",
                "END"
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
                "STORY",
                "OneTimeStory: No",
                "END"
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
                "STORY",
                "OneTimeStory: True",
                "END"
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
                "STORY",
                "OneTimeStory: Yes",
                "END"
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
                "STORY",
                "Restriction:  age > 18",
                "END"
            };
            var expectedResult = new BaseEvaluation
            {
                Persona = new Persona
                {
                    Characteristic = Characteristics.Age
                },
                Numbers = new Numbers
                {
                    Operator = Operator.Greaterthan,
                    Value = "18"
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Restrictions[0].Numbers.Operator.Should().Be(expectedResult.Numbers.Operator);
            actualResult.Restrictions[0].Numbers.Value.Should().Be(expectedResult.Numbers.Value);
            actualResult.Restrictions[0].Persona.Characteristic.Should().Be(expectedResult.Persona.Characteristic);
        }

        [Test]
        public void GivenStoryRestrictionPlayerGenderEqualToFemaleEnd_WhenImport_ThenIHaveAStoryWithProperRestriction()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Restriction:  gender = female",
                "END"
            };
            var expectedResult = new BaseEvaluation
            {
                Persona = new Persona
                {
                    Characteristic = Characteristics.Gender
                },
                Numbers = new Numbers
                {
                    Operator = Operator.Equalto,
                    Value = "Female"
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Restrictions[0].Numbers.Operator.Should().Be(expectedResult.Numbers.Operator);
            actualResult.Restrictions[0].Numbers.Value.Should().Be(expectedResult.Numbers.Value);
            actualResult.Restrictions[0].Persona.Characteristic.Should().Be(expectedResult.Persona.Characteristic);
        }

        [Test]
        public void GivenStoryRestrictionPlayerValorLowerThan30End_WhenImport_ThenIHaveAStoryWithProperRestriction()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Restriction:  Valor < 30",
                "END"
            };

            var expectedResult = new BaseEvaluation
            {
                Persona = new Persona
                {
                    PersonalityTrait = PersonalityTraits.Valor
                },
                Numbers = new Numbers
                {
                    Operator = Operator.Lowerthan,
                    Value = "30"
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Restrictions[0].Numbers.Operator.Should().Be(expectedResult.Numbers.Operator);
            actualResult.Restrictions[0].Numbers.Value.Should().Be(expectedResult.Numbers.Value);
            actualResult.Restrictions[0].Persona.PersonalityTrait.Should().Be(expectedResult.Persona.PersonalityTrait);
        }


        [Test]
        public void ImportStoryWithConsequenceEscapeWithoutSpace_ShouldPass()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "Story, ",
                "Act",
                "Sequence",
                "Choice: Continue..",
                "Consequence:Escape",
                "END"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Sequences[0].Choices[0].Consequences[0].Outcome.Escaping.Should().BeTrue();
            actualResult.Sequences[0].Choices[0].Consequences[0].Numbers.Value.Should().BeNullOrEmpty();
        }

        [Test]
        public void ImportStoryWithNullImageReference_ShouldPass()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "Story, ",
                "Act",
                "Image: ",
                " END"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Image.Should().BeNullOrEmpty();
        }

        [Test]
        public void StoryImporter_Generosity_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Restriction: generosity > 5",
                "END"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Restrictions[0].Persona.PersonalityTrait.Should().Be(PersonalityTraits.Generosity);
            actualResult.Restrictions[0].Numbers.Operator.Should().Be(Operator.Greaterthan);
            actualResult.Restrictions[0].Numbers.Value.Should().Be("5");
            actualResult.Restrictions[0].Persona.Subject.Should().Be(Actor.Player);
        }


        [Test]
        public void StoryImporter_Honor_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Restriction: honor > 5",
                "END"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Restrictions[0].Persona.PersonalityTrait.Should().Be(PersonalityTraits.Honor);
            actualResult.Restrictions[0].Numbers.Operator.Should().Be(Operator.Greaterthan);
            actualResult.Restrictions[0].Numbers.Value.Should().Be("5");
            actualResult.Restrictions[0].Persona.Subject.Should().Be(Actor.Player);
        }


        [Test]
        public void StoryImporter_Mercy_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Restriction: mercy > 5",
                "END"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Restrictions[0].Persona.PersonalityTrait.Should().Be(PersonalityTraits.Mercy);
            actualResult.Restrictions[0].Numbers.Operator.Should().Be(Operator.Greaterthan);
            actualResult.Restrictions[0].Numbers.Value.Should().Be("5");
            actualResult.Restrictions[0].Persona.Subject.Should().Be(Actor.Player);
        }

        [Test]
        public void StoryImporter_RemoveClothes_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Act",
                "Name: Remove clothes",
                "Choice: Continue....",
                "Consequence: Remove clothes",
                "END"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Consequences[0].Outcome.ShouldUndress.Should().BeTrue();
        }


        [Test]
        public void StoryImporter_ReturnClothes_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Act",
                "Name: Return clothes",
                "Choice: Continue....",
                "Consequence: Return clothes",
                "END"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Consequences[0].Outcome.ShouldEquip.Should().BeTrue();
        }


        [Test]
        public void StoryImporter_Strip_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Act",
                "Name: Strip",
                "Choice: Continue....",
                "Consequence: Strip player",
                "END"
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Acts[0].Choices[0].Consequences[0].Outcome.ShouldUndress.Should().BeTrue();
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
                "STORY",
                "Name: Test",
                "END"
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
                "STORY",
                "Restriction: npc gender is male",
                "END"
            };

            var expectedResult = new BaseEvaluation
            {
                Persona = new Persona
                {
                    Characteristic = Characteristics.Gender,
                    Subject = Actor.Npc
                },
                Numbers = new Numbers
                {
                    Operator = Operator.Equalto,
                    Value = "Male"
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Restrictions[0].Persona.Characteristic.Should().Be(expectedResult.Persona.Characteristic);
            actualResult.Restrictions[0].Numbers.Operator.Should().Be(expectedResult.Numbers.Operator);
            actualResult.Restrictions[0].Numbers.Value.Should().Be(expectedResult.Numbers.Value);
            actualResult.Restrictions[0].Persona.Subject.Should().Be(expectedResult.Persona.Subject);
        }


        [Test]
        public void StoryImporter_Test11_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "restriction: npc culture is nord",
                "END"
            };

            var expectedResult = new BaseEvaluation
            {
                Persona = new Persona
                {
                    Characteristic = Characteristics.Culture,
                    Subject = Actor.Npc
                },
                Numbers = new Numbers
                {
                    Operator = Operator.Equalto,
                    Value = "Nord"
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Restrictions[0].Persona.Characteristic.Should().Be(expectedResult.Persona.Characteristic);
            actualResult.Restrictions[0].Numbers.Operator.Should().Be(expectedResult.Numbers.Operator);
            actualResult.Restrictions[0].Numbers.Value.Should().Be(expectedResult.Numbers.Value);
            actualResult.Restrictions[0].Persona.Subject.Should().Be(expectedResult.Persona.Subject);
        }


        [Test]
        public void StoryImporter_Test12_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "restriction: npc is lord",
                "END"
            };

            var expectedResult = new BaseEvaluation
            {
                Persona = new Persona
                {
                    Subject = Actor.Npc
                },
                PartyType = PartyType.Lord,
                Numbers = new Numbers
                {
                    Operator = Operator.Equalto,
                    Value = "Lord"
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Restrictions[0].Persona.Characteristic.Should().Be(expectedResult.Persona.Characteristic);
            actualResult.Restrictions[0].Numbers.Operator.Should().Be(expectedResult.Numbers.Operator);
            actualResult.Restrictions[0].Numbers.Value.Should().Be(expectedResult.Numbers.Value);
            actualResult.Restrictions[0].Persona.Subject.Should().Be(expectedResult.Persona.Subject);
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
                        Name = "My Act 1",
                        Image = "None",
                        Location = Location.Map,
                        Intro = "This is an intro for act 1."
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
                "STORY",
                "Act",
                "Choice: Act 1 choice 1.",
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
                                        Persona = new Persona
                                        {
                                            Attribute = Attributes.Vigor
                                        },
                                        Numbers = new Numbers
                                        {
                                            Operator = Operator.Greaterthan,
                                            Value = "1"
                                        }
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
            actualResult.Acts[0].Choices[0].Conditions[0].Persona.Attribute.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Persona.Attribute);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.Operator.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Numbers.Operator);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.Value.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Numbers.Value);
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
                                        Persona = new Persona
                                        {
                                            Attribute = Attributes.Endurance
                                        },
                                        Numbers = new Numbers
                                        {
                                            Operator = Operator.Equalto,
                                            Value = "3"
                                        }
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
            actualResult.Acts[0].Choices[0].Conditions[0].Persona.Attribute.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Persona.Attribute);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.Operator.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Numbers.Operator);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.Value.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Numbers.Value);
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
                                        Persona = new Persona
                                        {
                                            Attribute = Attributes.Control
                                        },
                                        Numbers = new Numbers
                                        {
                                            Operator = Operator.Lowerthan,
                                            Value = "2"
                                        }
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
            actualResult.Acts[0].Choices[0].Conditions[0].Persona.Attribute.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Persona.Attribute);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.Operator.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Numbers.Operator);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.Value.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Numbers.Value);
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
                                        Persona = new Persona
                                        {
                                            Attribute = Attributes.Cunning
                                        },
                                        Numbers = new Numbers
                                        {
                                            Operator = Operator.Greaterthan,
                                            Value = "4"
                                        }
                                    },
                                    new BaseEvaluation
                                    {
                                        Persona = new Persona
                                        {
                                            Attribute = Attributes.Social
                                        },
                                        Numbers = new Numbers
                                        {
                                            Operator = Operator.Lowerthan,
                                            Value = "5"
                                        }
                                    },
                                    new BaseEvaluation
                                    {
                                        Persona = new Persona
                                        {
                                            Attribute = Attributes.Intelligence
                                        },
                                        Numbers = new Numbers
                                        {
                                            Operator = Operator.Equalto,
                                            Value = "6",
                                            ValueIsPercentage = false
                                        }
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

            actualResult.Acts[0].Choices[0].Conditions[0].Persona.Attribute.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Persona.Attribute);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.Operator.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Numbers.Operator);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.Value.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Numbers.Value);

            actualResult.Acts[0].Choices[0].Conditions[1].Persona.Attribute.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[1].Persona.Attribute);
            actualResult.Acts[0].Choices[0].Conditions[1].Numbers.Operator.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[1].Numbers.Operator);
            actualResult.Acts[0].Choices[0].Conditions[1].Numbers.Value.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[1].Numbers.Value);

            actualResult.Acts[0].Choices[0].Conditions[2].Persona.Attribute.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[2].Persona.Attribute);
            actualResult.Acts[0].Choices[0].Conditions[2].Numbers.Operator.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[2].Numbers.Operator);
            actualResult.Acts[0].Choices[0].Conditions[2].Numbers.Value.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[2].Numbers.Value);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.ValueIsPercentage.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Numbers.ValueIsPercentage);
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
                                        Persona = new Persona
                                        {
                                            Characteristic = Characteristics.Health
                                        },
                                        Numbers = new Numbers
                                        {
                                            Operator = Operator.Greaterthan,
                                            Value = "20",
                                            ValueIsPercentage = true
                                        }
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
            actualResult.Acts[0].Choices[0].Conditions[0].Persona.Attribute.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Persona.Attribute);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.Operator.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Numbers.Operator);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.Value.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Numbers.Value);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.ValueIsPercentage.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Numbers.ValueIsPercentage);
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
                "STORY",
                "OneTimeStory: no",
                "END"
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
                                        Persona = new Persona
                                        {
                                            Characteristic = Characteristics.Health,
                                            Subject = Actor.Player,
                                            PersonalityTrait = PersonalityTraits.NotAssigned
                                        },
                                        Numbers = new Numbers
                                        {
                                            Operator = Operator.Greaterthan,
                                            Value = "20",
                                            ValueIsPercentage = true
                                        }
                                    }
                                },
                                Consequences = new List<IEvaluation>
                                {
                                    new BaseEvaluation
                                    {
                                        Persona = new Persona
                                        {
                                            Characteristic = Characteristics.Renown,
                                            Subject = Actor.Player
                                        },
                                        Numbers = new Numbers
                                        {
                                            Operator = Operator.Equalto,
                                            Value = "-1",
                                            ValueIsPercentage = false
                                        }
                                    },
                                    new BaseEvaluation
                                    {
                                        Persona = new Persona
                                        {
                                            Characteristic = Characteristics.Health,
                                            Subject = Actor.Player
                                        },
                                        Numbers = new Numbers
                                        {
                                            Operator = Operator.Equalto,
                                            RandomStart = -1,
                                            RandomEnd = -10,
                                            ValueIsPercentage = false
                                        }
                                    },
                                    new BaseEvaluation
                                    {
                                        Persona = new Persona
                                        {
                                            Subject = Actor.Player
                                        },
                                        Outcome = new Outcome
                                        {
                                            PregnancyRisk = true
                                        }
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
            actualResult.Acts[0].Choices[0].Conditions[0].Persona.Attribute.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Persona.Attribute);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.Operator.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Numbers.Operator);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.Value.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Numbers.Value);
            actualResult.Acts[0].Choices[0].Conditions[0].Numbers.ValueIsPercentage.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Numbers.ValueIsPercentage);
            actualResult.Acts[0].Choices[0].Conditions[0].Persona.Subject.Should().Be(expectedResult.Acts[0].Choices[0].Conditions[0].Persona.Subject);

            actualResult.Acts[0].Choices[0].Consequences[0].Persona.Attribute.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[0].Persona.Attribute);
            actualResult.Acts[0].Choices[0].Consequences[0].Numbers.Operator.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[0].Numbers.Operator);
            actualResult.Acts[0].Choices[0].Consequences[0].Numbers.Value.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[0].Numbers.Value);
            actualResult.Acts[0].Choices[0].Consequences[0].Numbers.ValueIsPercentage.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[0].Numbers.ValueIsPercentage);
            actualResult.Acts[0].Choices[0].Consequences[0].Persona.Subject.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[0].Persona.Subject);

            actualResult.Acts[0].Choices[0].Consequences[1].Persona.Attribute.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[1].Persona.Attribute);
            actualResult.Acts[0].Choices[0].Consequences[1].Numbers.Operator.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[1].Numbers.Operator);
            actualResult.Acts[0].Choices[0].Consequences[1].Numbers.RandomStart.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[1].Numbers.RandomStart);
            actualResult.Acts[0].Choices[0].Consequences[1].Numbers.RandomEnd.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[1].Numbers.RandomEnd);
            actualResult.Acts[0].Choices[0].Consequences[1].Numbers.ValueIsPercentage.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[1].Numbers.ValueIsPercentage);
            actualResult.Acts[0].Choices[0].Consequences[1].Persona.Subject.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[1].Persona.Subject);

            actualResult.Acts[0].Choices[0].Consequences[2].Outcome.PregnancyRisk.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[2].Outcome.PregnancyRisk);
            actualResult.Acts[0].Choices[0].Consequences[2].Persona.Subject.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[2].Persona.Subject);
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
                                        Persona = new Persona
                                        {
                                            Subject = Actor.Npc
                                        },
                                        Outcome = new Outcome
                                        {
                                            PregnancyRisk = true
                                        }
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
            actualResult.Acts[0].Choices[0].Consequences[0].Outcome.PregnancyRisk.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[0].Outcome.PregnancyRisk);
            actualResult.Acts[0].Choices[0].Consequences[0].Persona.Subject.Should().Be(expectedResult.Acts[0].Choices[0].Consequences[0].Persona.Subject);
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
                                        Persona = new Persona
                                        {
                                            Subject = Actor.Player,
                                            Characteristic = Characteristics.Health
                                        },
                                        Numbers = new Numbers
                                        {
                                            Operator = Operator.Equalto,
                                            Value = "-10"
                                        }
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
            actualResult.Sequences[0].Choices[0].Consequences[0].Persona.Subject.Should().Be(expectedResult.Sequences[0].Choices[0].Consequences[0].Persona.Subject);
            actualResult.Sequences[0].Choices[0].Consequences[0].Persona.Characteristic.Should().Be(expectedResult.Sequences[0].Choices[0].Consequences[0].Persona.Characteristic);
            actualResult.Sequences[0].Choices[0].Consequences[0].Numbers.Operator.Should().Be(expectedResult.Sequences[0].Choices[0].Consequences[0].Numbers.Operator);
            actualResult.Sequences[0].Choices[0].Consequences[0].Numbers.Value.Should().Be(expectedResult.Sequences[0].Choices[0].Consequences[0].Numbers.Value);
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
            actualResult.Acts[0].ParentStory.Header.Name.Should().Be("MyStory");
            actualResult.Acts[0].Name.Should().Be("MyAct");
        }


        [Test]
        public void StoryImporter_Test26_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "StoryType: surrender",
                "END"
            };

            var expectedResult = StoryType.PlayerSurrender;

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Header.TypeOfStory.Should().Be(expectedResult);
        }


        [Test]
        public void StoryImporter_Test3_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "DependOn: None",
                "END"
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
                "STORY",
                "Time: Night",
                "END"
            };

            var expectedResult = GameTime.Nighttime;

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
                "STORY",
                "StoryType: is captive",
                "END"
            };

            var expectedResult = StoryType.PlayerIsCaptive;

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
                "STORY",
                "Restriction: age > 18",
                "END"
            };

            var expectedResult = new BaseEvaluation
            {
                Persona = new Persona
                {
                    Characteristic = Characteristics.Age,
                    Subject = Actor.Player
                },
                Numbers = new Numbers
                {
                    Operator = Operator.Greaterthan,
                    Value = "18"
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Restrictions[0].Persona.Characteristic.Should().Be(expectedResult.Persona.Characteristic);
            actualResult.Restrictions[0].Numbers.Operator.Should().Be(expectedResult.Numbers.Operator);
            actualResult.Restrictions[0].Numbers.Value.Should().Be(expectedResult.Numbers.Value);
            actualResult.Restrictions[0].Persona.Subject.Should().Be(expectedResult.Persona.Subject);
        }


        [Test]
        public void StoryImporter_Test7_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Restriction: gender is female",
                "END"
            };

            var expectedResult = new BaseEvaluation
            {
                Persona = new Persona
                {
                    Characteristic = Characteristics.Gender,
                    Subject = Actor.Player
                },
                Numbers = new Numbers
                {
                    Operator = Operator.Equalto,
                    Value = "Female"
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Restrictions[0].Persona.Characteristic.Should().Be(expectedResult.Persona.Characteristic);
            actualResult.Restrictions[0].Numbers.Operator.Should().Be(expectedResult.Numbers.Operator);
            actualResult.Restrictions[0].Numbers.Value.Should().Be(expectedResult.Numbers.Value);
            actualResult.Restrictions[0].Persona.Subject.Should().Be(expectedResult.Persona.Subject);
        }

        [Test]
        public void StoryImporter_Test8_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "restriction: culture is empire",
                "END"
            };

            var expectedResult = new BaseEvaluation
            {
                Persona = new Persona
                {
                    Characteristic = Characteristics.Culture,
                    Subject = Actor.Player
                },
                Numbers = new Numbers
                {
                    Operator = Operator.Equalto,
                    Value = "Empire"
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Restrictions[0].Persona.Characteristic.Should().Be(expectedResult.Persona.Characteristic);
            actualResult.Restrictions[0].Numbers.Operator.Should().Be(expectedResult.Numbers.Operator);
            actualResult.Restrictions[0].Numbers.Value.Should().Be(expectedResult.Numbers.Value);
            actualResult.Restrictions[0].Persona.Subject.Should().Be(expectedResult.Persona.Subject);
        }


        [Test]
        public void StoryImporter_Test9_ShouldWork()
        {
            // Arrange
            var sut = new StoryImporter();

            var param = new List<string>
            {
                "STORY",
                "Restriction: npc age > 18",
                "END"
            };

            var expectedResult = new BaseEvaluation
            {
                Persona = new Persona
                {
                    Characteristic = Characteristics.Age,
                    Subject = Actor.Npc
                },
                Numbers = new Numbers
                {
                    Operator = Operator.Greaterthan,
                    Value = "18"
                }
            };

            // Act
            var actualResult = sut.ImportFrom(param.ToArray());

            // Assert
            actualResult.Restrictions[0].Persona.Characteristic.Should().Be(expectedResult.Persona.Characteristic);
            actualResult.Restrictions[0].Numbers.Operator.Should().Be(expectedResult.Numbers.Operator);
            actualResult.Restrictions[0].Numbers.Value.Should().Be(expectedResult.Numbers.Value);
            actualResult.Restrictions[0].Persona.Subject.Should().Be(expectedResult.Persona.Subject);
        }
    }
}