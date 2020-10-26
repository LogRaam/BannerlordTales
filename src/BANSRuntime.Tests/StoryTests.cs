// Code written by Gabriel Mailhot, 11/09/2020.

#region

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TalesContract;
using TalesEntities.Stories;
using TalesEntities.TW;
using TalesEnums;
using TalesPersistence.Context;
using TalesPersistence.Stories;

#endregion

namespace BannerlordTales.Tests
{
    #region

    #endregion

    [TestFixture]
    public class StoryTests
    {
        [Test]
        public void AllLinksExistFor_Test()
        {
            // Arrange

            var sut = new StoryContext
            {
                Stories = new List<IStory>
                {
                    new Story
                    {
                        Acts = new List<IAct>
                        {
                            new Act
                            {
                                Name = "Something",
                                Choices = new List<IChoice>
                                {
                                    new BaseChoice
                                    {
                                        Text = "abc",
                                        Triggers = new List<ITrigger>
                                        {
                                            new BaseTrigger
                                            {
                                                Link = "Link 1"
                                            }
                                        }
                                    }
                                }
                            },
                            new Sequence
                            {
                                Name = "Link 1",
                                Choices = new List<IChoice>
                                {
                                    new BaseChoice
                                    {
                                        Text = "def",
                                        Triggers = new List<ITrigger>
                                        {
                                            new BaseTrigger
                                            {
                                                Link = "Link 2"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new Story
                    {
                        Acts = new List<IAct>
                        {
                            new Act
                            {
                                Name = "A Name",
                                Choices = new List<IChoice>
                                {
                                    new BaseChoice
                                    {
                                        Text = "ghi",
                                        Triggers = new List<ITrigger>
                                        {
                                            new BaseTrigger
                                            {
                                                Link = "Link 1"
                                            }
                                        }
                                    }
                                }
                            },
                            new Sequence
                            {
                                Name = "Link 2",
                                Choices = new List<IChoice>
                                {
                                    new BaseChoice
                                    {
                                        Text = "jkl",
                                        Triggers = new List<ITrigger>
                                        {
                                            new BaseTrigger
                                            {
                                                Link = "A Name"
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        Sequences = new List<ISequence>
                        {
                            new Sequence
                            {
                                Name = "Another",
                                Choices = new List<IChoice>
                                {
                                    new BaseChoice
                                    {
                                        Text = "mno",
                                        Triggers = new List<ITrigger>
                                        {
                                            new BaseTrigger
                                            {
                                                Link = "Something"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act
            var t1 = sut.AllLinksExistFor(sut.Stories[0].Acts[0]);
            var t2 = sut.AllLinksExistFor(sut.Stories[0].Acts[1]);
            var t3 = sut.AllLinksExistFor(sut.Stories[1].Acts[0]);
            var t4 = sut.AllLinksExistFor(sut.Stories[1].Acts[1]);
            var t5 = sut.AllLinksExistFor(sut.Stories[1].Sequences[0]);

            // Assert
            t1.Should().BeTrue();
            t2.Should().BeTrue();
            t3.Should().BeTrue();
            t4.Should().BeTrue();
            t5.Should().BeTrue();
        }

        [Test]
        public void IsQualifiedEventFor_QualifiedStory1_ReturnTrue()
        {
            // Arrange
            GameData.Instance = new GameData
            {
                GameContext = new GameContext
                {
                    PlayerIsCaptor = true, IsDay = true
                }
            };

            var sut = new Story
            {
                Header = new StoryHeader
                {
                    Time = GameTime.DAYTIME, TypeOfStory = StoryType.PLAYER_IS_CAPTOR
                }
            };

            // Act
            var actualResult = sut.IsQualifiedRightNow();

            // Assert
            actualResult.Should().BeTrue();
        }

        [Test]
        public void IsQualifiedEventFor_QualifiedStory2_ReturnTrue()
        {
            // Arrange
            GameData.Instance = new GameData
            {
                GameContext = new GameContext
                {
                    Player = new BaseHero
                    {
                        Age = 18, IsFemale = true, IsHumanPlayerCharacter = true, IsPrisoner = true
                    },
                    IsDay = true
                }
            };

            var sut = new Story
            {
                Header = new StoryHeader
                {
                    Time = GameTime.DAYTIME, TypeOfStory = StoryType.PLAYER_IS_CAPTIVE
                }
            };

            // Act
            var actualResult = sut.IsQualifiedRightNow();

            // Assert
            actualResult.Should().BeTrue();
        }

        [Test]
        public void IsQualifiedEventFor_QualifiedStory3_ReturnTrue()
        {
            // Arrange
            GameData.Instance = new GameData
            {
                GameContext = new GameContext
                {
                    IsDay = true,
                    Player = new BaseHero
                    {
                        IsPrisoner = true
                    }
                }
            };


            var sut = new Story
            {
                Header = new StoryHeader
                {
                    Time = GameTime.DAYTIME, TypeOfStory = StoryType.PLAYER_IS_CAPTIVE
                }
            };

            // Act
            var actualResult = sut.IsQualifiedRightNow();

            // Assert
            actualResult.Should().BeTrue();
        }


        [Test]
        public void IsQualifiedEventFor_QualifiedStory4_ReturnTrue()
        {
            // Arrange
            new Stories().LoadStoriesFromDisk();
            var story = new Story(GameData.Instance.StoryContext.Stories.First(n => n.Header.Name == "Test"));
            var act = new Act(story.Acts[0]);

            GameData.Instance.GameContext.IsNight = true;
            GameData.Instance.StoryContext.PlayedStories.Add(story);

            GameData.Instance.GameContext.Player = new BaseHero
            {
                Age = 19,
                IsFemale = true,
                Culture = new BaseBasicCultureObject
                {
                    CultureCode = CultureCode.EMPIRE
                },
                Vigor = 5,
                IsPrisoner = true
            };

            GameData.Instance.GameContext.Captor = new BaseHero
            {
                Age = 23
            };

            GameData.Instance.GameContext.IsCurrentlyOnMap = true;


            // Act
            var r1 = story.IsQualifiedRightNow();
            var r2 = act.IsQualifiedRightNow();

            // Assert
            r1.Should().BeTrue(); //because PlayedStories
            r2.Should().BeTrue();
        }


        [Test]
        public void IsQualifiedEventFor_QualifiedStory5_ReturnFalse()
        {
            // Arrange
            new Stories().SetupKissTheBanner();

            GameData.Instance.GameContext.IsCurrentlyOnMap = false;
            GameData.Instance.GameContext.IsCurrentlyInSettlement = true;

            GameData.Instance.StoryContext.Stories[0].Acts[0].Location = Location.MAP;

            var sut = (Story)GameData.Instance.StoryContext.Stories[0];

            // Act
            var actualResult = ((Act)sut.Acts[0]).IsQualifiedRightNow();

            // Assert
            actualResult.Should().BeFalse();
        }

        [Test]
        public void TestStory_Act_ShouldPass()
        {
            // Arrange
            new Stories().LoadStoriesFromDisk();
            var story = new Story(GameData.Instance.StoryContext.Stories.First(n => n.Header.Name == "PrisonerWaiting"));

            // Act
            var sut = new Act(story.Acts[0]);

            // Assert
            story.Acts.Count.Should().Be(2);
            story.Acts[0].ParentStory.Header.Name.Should().Be(story.Header.Name);
            story.Acts[1].ParentStory.Header.Name.Should().Be(story.Header.Name);
            sut.ParentStory.Header.Name.Should().Be(story.Header.Name);
            story.Header.Name.Should().NotBeNullOrEmpty();
        }


        [Test]
        public void TestStory_FullTest_ShouldPass()
        {
            // Arrange
            new Stories().LoadStoriesFromDisk();
            var story = new Story(GameData.Instance.StoryContext.Stories.First(n => n.Header.Name == "Test"));
            var act1 = new Act(story.Acts[0]);
            var act2 = new Act(story.Acts[1]);
            var seq1 = new Sequence(story.Sequences[0]);
            var seq2 = new Sequence(story.Sequences[1]);

            GameData.Instance.GameContext.IsNight = true;
            GameData.Instance.StoryContext.PlayedStories.Add(story);

            GameData.Instance.GameContext.Player = new BaseHero
            {
                Age = 19,
                IsFemale = true,
                Culture = new BaseBasicCultureObject
                {
                    CultureCode = CultureCode.EMPIRE
                },
                Vigor = 5,
                IsPrisoner = true
            };

            GameData.Instance.GameContext.Captor = new BaseHero
            {
                Age = 23
            };

            GameData.Instance.GameContext.IsCurrentlyOnMap = true;


            // Act
            var s1 = story.IsQualifiedRightNow();
            var a1 = act1.IsQualifiedRightNow();
            var a2 = act2.IsQualifiedRightNow();
            var sq1 = seq1.IsQualifiedRightNow();
            var sq2 = seq2.IsQualifiedRightNow();

            // Assert
            s1.Should().BeTrue();
            a1.Should().BeTrue();
            a2.Should().BeTrue();
            sq1.Should().BeTrue();
            sq2.Should().BeTrue();
        }

        [Test]
        public void TestStory_Triggers_ShouldPass()
        {
            // Arrange
            new Stories().LoadStoriesFromDisk();

            // Act
            var story = new Story(GameData.Instance.StoryContext.Stories.First(n => n.Header.Name == "Test"));

            // Assert
            story.Acts.Count.Should().Be(3);
            story.Acts[0].ParentStory.Header.Name.Should().Be(story.Header.Name);
            story.Acts[1].ParentStory.Header.Name.Should().Be(story.Header.Name);
            story.Acts[2].ParentStory.Header.Name.Should().Be(story.Header.Name);
            story.Acts.First(n => n.Name == "MyAct3").Choices.First(n => n.Text == "Act2choice2").Triggers.Count.Should().Be(2);
            story.Acts.First(n => n.Name == "MyAct3").Choices.First(n => n.Text == "Act2choice2").Triggers[0].Link.Should().Be("MySequence3");
            story.Acts.First(n => n.Name == "MyAct3").Choices.First(n => n.Text == "Act2choice2").Triggers[0].ChanceToTrigger.Should().Be(25);
            story.Acts.First(n => n.Name == "MyAct3").Choices.First(n => n.Text == "Act2choice2").Triggers[1].ChanceToTrigger.Should().Be(75);
        }

        [Test]
        public void TestStory_WaitingMenu_ShouldPass()
        {
            // Arrange
            new Stories().LoadStoriesFromDisk();

            // Act
            var story = new Story(GameData.Instance.StoryContext.Stories.First(n => n.Header.Name == "PrisonerWaiting"));

            // Assert
            story.Acts.Count.Should().Be(2);
        }

        [Test]
        public void TestStory_WaitingMenu2_ShouldPass()
        {
            // Arrange
            new Stories().LoadStoriesFromDisk();

            // Act
            var story = new Story(GameData.Instance.StoryContext.Stories.First(n => n.Header.Name == "PrisonerWaiting"));

            // Assert
            story.Acts.Count.Should().Be(2);
            story.Acts[0].ParentStory.Header.Name.Should().Be(story.Header.Name);
            story.Acts[1].ParentStory.Header.Name.Should().Be(story.Header.Name);
        }

        [Test]
        public void TestStory_WaitingMenu3_ShouldPass()
        {
            // Arrange
            new Stories().LoadStoriesFromDisk();

            // Act
            var story = new Story(GameData.Instance.StoryContext.Stories.First(n => n.Header.Name == "Default"));

            // Assert
            story.Acts.Count.Should().Be(1);
            story.Acts[0].ParentStory.Header.Name.Should().Be(story.Header.Name);
            story.Sequences.First(n => n.Name == "Refuse to kiss the banner").Choices[0].Consequences[0].Value.Should().Be("-10");
            story.Sequences.First(n => n.Name == "Refuse to kiss the banner").Choices[0].Consequences[0].Subject.Should().Be(Actor.PLAYER);
            story.Sequences.First(n => n.Name == "Refuse to kiss the banner").Choices[0].Consequences[0].Characteristic.Should().Be(Characteristics.HEALTH);
        }
    }
}