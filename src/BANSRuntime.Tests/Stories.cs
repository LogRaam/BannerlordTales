// Code written by Gabriel Mailhot, 11/09/2020.

#region

using System.Collections.Generic;
using System.IO;
using TalesBase.Stories;
using TalesBase.Stories.Evaluation;
using TalesBase.TW;
using TalesContract;
using TalesEnums;
using TalesPersistence.Context;
using TalesPersistence.Stories;

#endregion

namespace BannerlordTales.Tests
{
    public class Stories
    {
        public GameData KissTheBannerGameData()
        {
            var p = "D:\\Program Files (x86)\\Steam\\steamapps\\common\\Mount & Blade II Bannerlord\\Modules\\LogRaamBannerlordTales";
            var g = new GameData
            {
                GameContext = new GameContext
                {
                    IsCurrentlyInSettlement = false,
                    IsCurrentlyOnMap = true,
                    IsNight = false,
                    Player = new BaseHero
                    {
                        Age = 18,
                        IsFemale = true,
                        IsHumanPlayerCharacter = true
                    },
                    PlayerIsCaptor = true,
                    IsDay = true
                },
                StoryContext = new StoryContext
                {
                    PlayedStories = new List<IStory>(),
                    Stories = new List<IStory>(),
                    StoryImagesFolder = new DirectoryInfo(p + "\\StoryImages"),
                    ModuleFolder = new DirectoryInfo(p),
                    CustomStoriesFolder = new DirectoryInfo(p + "\\CustomStories")
                }
            };

            return g;
        }

        public void LoadStoriesFromDisk()
        {
            var p = "D:\\Program Files (x86)\\Steam\\steamapps\\common\\Mount & Blade II Bannerlord\\Modules\\LogRaamBannerlordTales";

            GameData.Instance = new GameData
            {
                StoryContext = new StoryContext
                {
                    PlayedStories = new List<IStory>(),
                    Stories = new List<IStory>(),
                    StoryImagesFolder = new DirectoryInfo(p + "\\StoryImages"),
                    ModuleFolder = new DirectoryInfo(p),
                    CustomStoriesFolder = new DirectoryInfo(p + "\\CustomStories")
                }
            };

            GameData.Instance.StoryContext.Stories = GameData.Instance.StoryContext.ImportStoriesFromDisk();
        }

        public void SetupKissTheBanner()
        {
            var sut = KissTheBannerStory();
            GameData.Instance = KissTheBannerGameData();
            GameData.Instance.StoryContext.Stories.Add(sut);
        }

        #region private

        private Story KissTheBannerStory()
        {
            var s = new Story
            {
                Header = new StoryHeader
                {
                    CanBePlayedOnlyOnce = true,
                    DependOn = "None",
                    Time = GameTime.DAYTIME,
                    TypeOfStory = StoryType.PLAYER_IS_CAPTOR
                },
                Restrictions = new List<IEvaluation>
                {
                    new BaseEvaluation
                    {
                        Persona = new Persona
                        {
                            Subject = Actor.PLAYER,
                            Characteristic = Characteristics.AGE
                        },
                        Numbers = new Numbers
                        {
                            Operator = Operator.GREATERTHAN,
                            Value = "18"
                        }
                    },
                    new BaseEvaluation
                    {
                        Persona = new Persona
                        {
                            Subject = Actor.PLAYER,
                            Characteristic = Characteristics.GENDER
                        },
                        Numbers = new Numbers
                        {
                            Operator = Operator.EQUALTO,
                            Value = "female"
                        }
                    },
                    new BaseEvaluation
                    {
                        Persona = new Persona
                        {
                            Subject = Actor.NPC
                        },
                        PartyType = PartyType.LORD,
                        Numbers = new Numbers
                        {
                            Operator = Operator.EQUALTO,
                            Value = "Noble"
                        }
                    }
                },
                Acts = new List<IAct>
                {
                    new Act
                    {
                        Name = "Kiss the Banner",
                        Image = "None",
                        Location = Location.MAP,
                        Intro = "Your captors demand, in order to inspire the people, that their captives kiss their banner. It's your turn.",
                        Choices = new List<IChoice>
                        {
                            new BaseChoice
                            {
                                Text = "Kiss the banner.",
                                Consequences =
                                {
                                    new BaseEvaluation
                                    {
                                        Persona = new Persona
                                        {
                                            Subject = Actor.PLAYER,
                                            Characteristic = Characteristics.RENOWN
                                        },
                                        Numbers = new Numbers
                                        {
                                            Operator = Operator.EQUALTO,
                                            Value = "-1"
                                        }
                                    }
                                }
                            },
                            new BaseChoice
                            {
                                Text = "Refuse.",
                                Triggers =
                                {
                                    new BaseTrigger
                                    {
                                        ChanceToTrigger = 100,
                                        Link = "Refuse to kiss the banner"
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
                        Name = "Refuse to kiss the banner",
                        Image = "None",
                        Location = Location.MAP,
                        Intro = "Your captors wrap you in their banner and beat your body with clubs.",
                        Choices = new List<IChoice>
                        {
                            new BaseChoice
                            {
                                Text = "Endure the beating.",
                                Consequences =
                                {
                                    new BaseEvaluation
                                    {
                                        Persona = new Persona
                                        {
                                            Subject = Actor.PLAYER,
                                            Characteristic = Characteristics.HEALTH
                                        },
                                        Numbers = new Numbers
                                        {
                                            Operator = Operator.EQUALTO,
                                            Value = "-10"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            return s;
        }

        #endregion
    }
}