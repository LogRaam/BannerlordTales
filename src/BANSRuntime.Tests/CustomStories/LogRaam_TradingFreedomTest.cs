// Code written by Gabriel Mailhot, 02/19/2023.  Updated by  Gabriel Mailhot on 02/20/2023.

#region

using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TalesBase.Stories;
using TalesBase.Stories.Evaluation;
using TalesBase.TW;
using TalesContract;
using TalesEnums;
using TalesPersistence.Context;
using TalesPersistence.Entities;
using TalesPersistence.Stories;

#endregion

namespace BannerlordTales.Tests.CustomStories
{
    [SetUpFixture]
    public class TestsSetupClass
    {
        [OneTimeSetUp]
        public void GlobalSetup()
        {
            //new Stories().LoadStoriesFromDisk("LogRaam_TradingFreedom.txt");
        }

        [OneTimeTearDown]
        public void GlobalTeardown()
        {
            GameData.Instance.Reset(); // Do logout here
        }
    }


    [TestFixture]
    public class LogRaamTradingFreedomTest
    {
        [Test]
        public void ActLocationShouldBeOnMap()
        {
            //Arrange
            new Stories().LoadStoriesFromDisk("LogRaam_TradingFreedom.txt");
            var expectedResult = new Act
            {
                Name = "An Unexpected Meeting",
                Image = "LogCaptiveShut2",
                Intro = "You've been sleeping for a long time.  All of a sudden you wake up with a start.  A man is holding your mouth with his hand, waving at you not to make a sound.  He says, Shhh!  I don't want to hurt you.  Don't shout and listen to me.",
                Location = Location.Map,
                ParentStory = new Story
                {
                    Header = new StoryHeader
                    {
                        Name = "Trading Freedom"
                    }
                },
                Choices = new List<IChoice>
                {
                    new Choice
                    {
                        Text = "Stay still and wait to see what happens...",
                        Triggers = new List<ITrigger>
                        {
                            new BaseTrigger
                            {
                                ChanceToTrigger = 100,
                                Link = "Stay Quiet And Wait"
                            }
                        }
                    },
                    new Choice
                    {
                        Triggers = new List<ITrigger>
                        {
                            new BaseTrigger
                            {
                                ChanceToTrigger = 50,
                                Link = "Fight Back Failed"
                            },
                            new BaseTrigger
                            {
                                ChanceToTrigger = 50,
                                Link = "Fight Back Success"
                            }
                        }
                    }
                }
            };
            var sut = GameData.Instance.StoryContext.Stories.First();

            //Act
            var actualResult = sut.Acts[0];

            //Assert
            actualResult.IsEquivalentTo(expectedResult).Should().Be(true);
        }

        [Test]
        public void DependOnShouldBeNone()
        {
            //Arrange
            new Stories().LoadStoriesFromDisk("LogRaam_TradingFreedom.txt");
            var expectedResult = "None";
            var sut = GameData.Instance.StoryContext.Stories.First();

            //Act
            var actualResult = sut.Header.DependOn;

            //Assert
            actualResult.Should().Be(expectedResult);
        }

        [Test]
        public void FirstStoryRestrictionShouldBePlayerAgeGreaterThan18()
        {
            //Arrange
            new Stories().LoadStoriesFromDisk("LogRaam_TradingFreedom.txt");
            var expectedResult = new Evaluation
            {
                Persona = new Persona
                {
                    Subject = Actor.Player,
                    Characteristic = Characteristics.Age
                },
                Numbers = new Numbers
                {
                    Operator = Operator.Greaterthan,
                    Value = "18"
                }
            };
            var sut = GameData.Instance.StoryContext.Stories.First();

            //Act
            var actualResult = sut.Restrictions[0];

            //Assert
            actualResult.IsEquivalentTo(expectedResult).Should().Be(true);
        }


        [Test]
        public void GivenIHaveTheRightStoryFilename_WhenILoadTheStoryFromDisk_ThenTheStoryShouldExistInRuntime()
        {
            //Arrange
            new Stories().LoadStoriesFromDisk("LogRaam_TradingFreedom.txt");
            var expectedResult = 1;

            //Act
            var actualResult = GameData.Instance.StoryContext.Stories.Count;

            //Assert
            GameData.Instance.StoryContext.Stories.Count.Should().BeGreaterThan(0);
            actualResult.Should().Be(expectedResult);
        }

        [Test]
        public void ItShouldHave3RestrictionsForStory()
        {
            //Arrange
            new Stories().LoadStoriesFromDisk("LogRaam_TradingFreedom.txt");
            var expectedResult = 3;
            var sut = GameData.Instance.StoryContext.Stories.First();

            //Act
            var actualResult = sut.Restrictions.Count;

            //Assert
            actualResult.Should().Be(expectedResult);
        }

        [Test]
        public void OneTimeStoryShouldBeYes()
        {
            //Arrange
            new Stories().LoadStoriesFromDisk("LogRaam_TradingFreedom.txt");
            var expectedResult = true;
            var sut = GameData.Instance.StoryContext.Stories.First();

            //Act
            var actualResult = sut.Header.CanBePlayedOnlyOnce;

            //Assert
            actualResult.Should().Be(expectedResult);
        }

        [Test]
        public void SecondStoryRestrictionShouldBePlayerGenderIsFemale()
        {
            //Arrange
            new Stories().LoadStoriesFromDisk("LogRaam_TradingFreedom.txt");
            var expectedResult = new Evaluation
            {
                Persona = new Persona
                {
                    Subject = Actor.Player,
                    Characteristic = Characteristics.Gender
                },
                Numbers = new Numbers
                {
                    Operator = Operator.Equalto,
                    Value = "Female"
                }
            };
            var sut = GameData.Instance.StoryContext.Stories.First();

            //Act
            var actualResult = sut.Restrictions[1];

            //Assert
            actualResult.IsEquivalentTo(expectedResult).Should().Be(true);
        }


        [Test]
        public void StoryNameShouldBeTradingFreedom()
        {
            //Arrange
            new Stories().LoadStoriesFromDisk("LogRaam_TradingFreedom.txt");
            var expectedResult = "TradingFreedom";
            var sut = GameData.Instance.StoryContext.Stories.First();

            //Act
            var actualResult = sut.Id;

            //Assert
            actualResult.Should().Be(expectedResult);
        }


        [Test]
        public void StoryTypeShouldBePlayerIsCaptive()
        {
            //Arrange
            new Stories().LoadStoriesFromDisk("LogRaam_TradingFreedom.txt");
            var expectedResult = StoryType.PlayerIsCaptive;
            var sut = GameData.Instance.StoryContext.Stories.First();

            //Act
            var actualResult = sut.Header.TypeOfStory;

            //Assert
            actualResult.Should().Be(expectedResult);
        }


        [Test]
        public void TheStoryShouldHaveOneAct()
        {
            //Arrange
            new Stories().LoadStoriesFromDisk("LogRaam_TradingFreedom.txt");
            var expectedResult = 1;
            var sut = GameData.Instance.StoryContext.Stories.First();

            //Act
            var actualResult = sut.Acts.Count;

            //Assert
            actualResult.Should().Be(expectedResult);
        }


        [Test]
        public void ThirdStoryRestrictionShouldBeNpcGenderIsMale()
        {
            //Arrange
            new Stories().LoadStoriesFromDisk("LogRaam_TradingFreedom.txt");
            var expectedResult = new Evaluation
            {
                Persona = new Persona
                {
                    Subject = Actor.Npc,
                    Characteristic = Characteristics.Gender
                },
                Numbers = new Numbers
                {
                    Operator = Operator.Equalto,
                    Value = "Male"
                }
            };
            var sut = GameData.Instance.StoryContext.Stories.First();

            //Act
            var actualResult = sut.Restrictions[2];

            //Assert
            actualResult.IsEquivalentTo(expectedResult).Should().Be(true);
        }

        [Test]
        public void TimeShouldBeNight()
        {
            //Arrange
            new Stories().LoadStoriesFromDisk("LogRaam_TradingFreedom.txt");
            var expectedResult = GameTime.Nighttime;
            var sut = GameData.Instance.StoryContext.Stories.First();

            //Act
            var actualResult = sut.Header.Time;

            //Assert
            actualResult.Should().Be(expectedResult);
        }
    }
}