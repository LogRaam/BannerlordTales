// unset

#region

using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TalesBase.Stories;
using TalesBase.TW;
using TalesContract;
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
                    Heroes =
                    {
                        PlayerIsCaptor = true
                    },

                    Time =
                    {
                        IsDay = true
                    }
                }
            };

            var sut = new Story
            {
                Header = new StoryHeader
                {
                    Time = GameTime.DAYTIME,
                    TypeOfStory = StoryType.PLAYER_IS_CAPTOR
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
                    Heroes =
                    {
                        Player = new BaseHero
                        {
                            Age = 18,
                            IsFemale = true,
                            IsHumanPlayerCharacter = true,
                            IsPrisoner = true
                        }
                    },
                    Time =
                    {
                        IsDay = true
                    }
                }
            };

            var sut = new Story
            {
                Header = new StoryHeader
                {
                    Time = GameTime.DAYTIME,
                    TypeOfStory = StoryType.PLAYER_IS_CAPTIVE
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
                    Time =
                    {
                        IsDay = true
                    },
                    Heroes =
                    {
                        Player = new BaseHero
                        {
                            IsPrisoner = true
                        }
                    }
                }
            };


            var sut = new Story
            {
                Header = new StoryHeader
                {
                    Time = GameTime.DAYTIME,
                    TypeOfStory = StoryType.PLAYER_IS_CAPTIVE
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

            GameData.Instance.GameContext.Time.IsNight = true;
            GameData.Instance.StoryContext.PlayedStories.Add(story);

            GameData.Instance.GameContext.Heroes.Player = new BaseHero
            {
                Age = 19,
                IsFemale = true,
                Culture = new BaseBasicCultureObject
                {
                    CultureCode = CultureCode.EMPIRE
                },
                Vigor = 5,
                IsPrisoner = true,
                PartyBelongedTo = new BaseMobileParty()
            };

            GameData.Instance.GameContext.Heroes.Captor = new BaseHero
            {
                Age = 23,
                Culture = new BaseBasicCultureObject
                {
                    CultureCode = CultureCode.ASERAI
                },
                PartyBelongedTo = new BaseMobileParty
                {
                    Name = "aParty",
                    IsLordParty = true
                }
            };

            GameData.Instance.GameContext.Tracking.IsCurrentlyOnMap = true;


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

            GameData.Instance.GameContext.Tracking.IsCurrentlyOnMap = false;
            GameData.Instance.GameContext.Tracking.IsCurrentlyInSettlement = true;

            GameData.Instance.StoryContext.Stories[0].Acts[0].Location = Location.MAP;

            var sut = (Story)GameData.Instance.StoryContext.Stories[0];

            // Act
            var actualResult = ((Act)sut.Acts[0]).IsQualifiedRightNow();

            // Assert
            actualResult.Should().BeFalse();
        }

        [Test]
        public void LosingBattle_Choice_FleeFailed_ParentAct_ShouldNotBeNull()
        {
            // Arrange
            new Stories().LoadStoriesFromDisk();

            // Act
            var story = new Story(GameData.Instance.StoryContext.Stories.First(n => n.Header.Name == "Losing Battle"));
            var fleeSeq = story.Sequences.Find(n => n.Name == "Flee Failed");
            var choice = fleeSeq.Choices[0];

            var parentActId = choice.ParentAct.Id;
            var fleeSeqId = fleeSeq.Id;
            var parentStoryId = choice.ParentAct.ParentStory.Id;
            var choiceId = choice.Id;


            // Assert
            parentActId.Should().Be(fleeSeqId);
            choiceId.Should().Be(parentActId + "_" + fleeSeq.Choices[0].Text);
            parentStoryId.Should().Be(fleeSeq.ParentStory.Id);
            choice.ParentAct.ParentStory.Header.Name.Should().NotBeNullOrEmpty();
            choice.ParentAct.Name.Should().NotBeNullOrEmpty();
            choice.Text.Should().NotBeNullOrEmpty();
        }


        [Test]
        public void LosingBattle_Choice_FleeSuccess_ParentAct_ShouldNotBeNull()
        {
            // Arrange
            new Stories().LoadStoriesFromDisk();

            // Act
            var story = new Story(GameData.Instance.StoryContext.Stories.First(n => n.Header.Name == "Losing Battle"));
            var fleeSeq = story.Sequences.Find(n => n.Name == "Flee Success");
            var choice = fleeSeq.Choices[0];

            var parentActId = choice.ParentAct.Id;
            var fleeSeqId = fleeSeq.Id;
            var parentStoryId = choice.ParentAct.ParentStory.Id;
            var choiceId = choice.Id;


            // Assert
            parentActId.Should().Be(fleeSeqId);
            choiceId.Should().Be(parentActId + "_" + fleeSeq.Choices[0].Text);
            parentStoryId.Should().Be(fleeSeq.ParentStory.Id);
            choice.ParentAct.ParentStory.Header.Name.Should().NotBeNullOrEmpty();
            choice.ParentAct.Name.Should().NotBeNullOrEmpty();
            choice.Text.Should().NotBeNullOrEmpty();
        }


        [Test]
        public void LosingBattle_Choice_Id_ShouldNotBeNull()
        {
            // Arrange
            new Stories().LoadStoriesFromDisk();

            // Act
            var story = new Story(GameData.Instance.StoryContext.Stories.First(n => n.Header.Name == "Losing Battle"));
            var fleeAct = story.Acts.Find(n => n.Name == "Laying on the ground");
            var choice = fleeAct.Choices.Find(n => n.Text == "Try to come to your senses and get back on your feet.");

            var parentActId = choice.ParentAct.Id;
            var fleeSeqId = fleeAct.Id;
            var parentStoryId = choice.ParentAct.ParentStory.Id;
            var choiceId = choice.Id;


            // Assert
            parentActId.Should().Be(fleeSeqId);
            choiceId.Should().Be(parentActId + "_" + fleeAct.Choices[0].Text.Replace(" ", ""));
            parentStoryId.Should().Be(fleeAct.ParentStory.Id);
            choice.ParentAct.ParentStory.Header.Name.Should().NotBeNullOrEmpty();
            choice.ParentAct.Name.Should().NotBeNullOrEmpty();
            choice.Text.Should().NotBeNullOrEmpty();
        }


        [Test]
        public void LosingBattle_Choice_ParentAct_ShouldNotBeNull()
        {
            // Arrange
            new Stories().LoadStoriesFromDisk();

            // Act
            var story = new Story(GameData.Instance.StoryContext.Stories.First(n => n.Header.Name == "Losing Battle"));
            var fleeSeq = story.Sequences.Find(n => n.Name == "Flee");
            var choice = fleeSeq.Choices[0];

            var parentActId = choice.ParentAct.Id;
            var fleeSeqId = fleeSeq.Id;
            var parentStoryId = choice.ParentAct.ParentStory.Id;
            var choiceId = choice.Id;


            // Assert
            parentActId.Should().Be(fleeSeqId);
            choiceId.Should().Be(parentActId + "_" + fleeSeq.Choices[0].Text);
            parentStoryId.Should().Be(fleeSeq.ParentStory.Id);
            choice.ParentAct.ParentStory.Header.Name.Should().NotBeNullOrEmpty();
            choice.ParentAct.Name.Should().NotBeNullOrEmpty();
            choice.Text.Should().NotBeNullOrEmpty();
        }


        [Test]
        public void LosingBattle_ChoiceConstruct_ParentAct_ShouldNotBeNull()
        {
            // Arrange
            new Stories().LoadStoriesFromDisk();

            // Act
            var story = new Story(GameData.Instance.StoryContext.Stories.First(n => n.Header.Name == "Losing Battle"));
            var choice = new Choice(story.Acts[0].Choices[0]);


            // Assert
            choice.Id.Should().NotBeNullOrEmpty();
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
        public void TestStory_ActRestriction_ShouldPass()
        {
            // Arrange
            new Stories().LoadStoriesFromDisk();

            // Act
            var story = new Story(GameData.Instance.StoryContext.Stories.First(n => n.Header.Name == "Test Tags"));

            // Assert
            story.Acts.First(n => n.Name == "ConditionalToTag").Intro.Contains("Restriction: ").Should().BeFalse();
            story.Acts.First(n => n.Name == "ConditionalToTag").Restrictions.Count.Should().Be(1);
        }


        [Test]
        public void TestStory_Condition_ShouldPass()
        {
            // Arrange
            new Stories().LoadStoriesFromDisk();

            // Act
            var story = new Story(GameData.Instance.StoryContext.Stories.First(n => n.Header.Name == "Test"));

            // Assert
            story.Acts.First(n => n.Name == "MyAct1").Choices.First(n => n.Text == "Act1choice1.").Conditions.Count.Should().Be(27);
        }


        [Test]
        public void TestStory_Escaping_ShouldPass()
        {
            // Arrange
            new Stories().LoadStoriesFromDisk();

            // Act
            var story = new Story(GameData.Instance.StoryContext.Stories.First(n => n.Header.Name == "Losing Battle"));
            var seq = story.Sequences.Find(n => n.Name == "Flee Success");


            // Assert
            seq.Choices.Find(n => n.Text == "Continue...").Consequences[1].Outcome.Escaping.Should().Be(true);
        }


        [Test]
        public void TestStory_FullTest_ShouldPass()
        {
            // Arrange
            new Stories().LoadStoriesFromDisk();
            var story = GameData.Instance.StoryContext.Stories.First(n => n.Header.Name == "Test");
            var act1 = new Act(story.Acts[0]);
            var act2 = new Act(story.Acts[1]);
            var seq1 = new Sequence(story.Sequences[0]);
            var seq2 = new Sequence(story.Sequences[1]);

            GameData.Instance.GameContext.Time.IsNight = true;
            GameData.Instance.StoryContext.PlayedStories.Add(story);

            GameData.Instance.GameContext.Heroes.Player = new BaseHero
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

            GameData.Instance.GameContext.Heroes.Captor = new BaseHero
            {
                Age = 23,
                Culture = new BaseBasicCultureObject
                {
                    CultureCode = CultureCode.ASERAI
                },
                PartyBelongedTo = new BaseMobileParty
                {
                    IsLordParty = true
                }
            };

            GameData.Instance.GameContext.Tracking.IsCurrentlyOnMap = true;

            // Act
            var s1 = new Story(story).IsQualifiedRightNow();
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
        public void TestStory_LosingBattle1_ShouldPass()
        {
            // Arrange
            new Stories().LoadStoriesFromDisk();

            // Act
            var story = new Story(GameData.Instance.StoryContext.Stories.First(n => n.Header.Name == "Losing Battle"));
            var act = story.Acts.First(n => n.Name == "Laying on the ground");

            // Assert
            act.Image.Should().Be("lg_battle_lost_closeup");
        }

        [Test]
        public void TestStory_LosingBattle2_ShouldPass()
        {
            // Arrange
            new Stories().LoadStoriesFromDisk();

            // Act
            var story = new Story(GameData.Instance.StoryContext.Stories.First(n => n.Header.Name == "Losing Battle"));
            var act = story.Acts.First(n => n.Name == "Laying on the ground");

            // Assert
            act.Id.Should().Be("LosingBattle_Layingontheground");
            act.Choices[1].Id.Should().Be("LosingBattle_Layingontheground_Waitandseewhathappensnext...");
        }


        [Test]
        public void TestStory_StoryRestriction_ShouldPass()
        {
            // Arrange
            new Stories().LoadStoriesFromDisk();

            // Act
            var story = new Story(GameData.Instance.StoryContext.Stories.First(n => n.Header.Name == "Losing Battle"));

            // Assert
            story.Restrictions.Count.Should().Be(4);
        }


        [Test]
        public void TestStory_Surrender1_ShouldPass()
        {
            // Arrange
            new Stories().LoadStoriesFromDisk();

            // Act
            var story = new Story(GameData.Instance.StoryContext.Stories.First(n => n.Header.Name == "Losing Battle"));

            // Assert
            story.Acts.Count.Should().Be(1);
            story.Sequences.Count.Should().Be(21);
            story.Header.TypeOfStory.Should().Be(StoryType.PLAYER_SURRENDER);
        }

        [Test]
        public void TestStory_Surrender2_ShouldPass()
        {
            // Arrange
            new Stories().LoadStoriesFromDisk();

            // Act
            var story = new Story(GameData.Instance.StoryContext.Stories.First(n => n.Header.Name == "Losing Battle"));
            var seq = story.Sequences.First(n => n.Name == "Flee");

            // Assert
            seq.Choices.Count.Should().Be(1);
            seq.Choices[0].Triggers[0].ChanceToTrigger.Should().Be(75);
            seq.Choices[0].Triggers[0].Link.Should().Be("Flee Success");
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
            story.Sequences.First(n => n.Name == "Refuse to kiss the banner").Choices[0].Consequences[0].Numbers.Value.Should().Be("-10");
            story.Sequences.First(n => n.Name == "Refuse to kiss the banner").Choices[0].Consequences[0].Persona.Subject.Should().Be(Actor.PLAYER);
            story.Sequences.First(n => n.Name == "Refuse to kiss the banner").Choices[0].Consequences[0].Persona.Characteristic.Should().Be(Characteristics.HEALTH);
        }
    }
}