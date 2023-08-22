// Code written by Gabriel Mailhot, 02/12/2023.  Updated by  Gabriel Mailhot on 02/19/2023.

#region

using System.Collections.Generic;
using System.IO;
using TalesBase.Stories;
using TalesBase.Stories.Evaluation;
using TalesBase.TW;
using TalesContract;
using TalesDAL;
using TalesEnums;
using TalesPersistence.Context;
using TalesPersistence.Stories;

#endregion

namespace BannerlordTales.Tests
{
    public class Stories
    {
        /// <summary>
        ///     Load stories from disk.  Actually use hard coded path due to issue with NCrunch.  This method will create a new
        ///     instance of GameData.  It reset GameContext and StoryContext.
        /// </summary>
        private const string Path = "P:\\OneDrive\\Programmation\\Bannerlord\\BannerlordTales";

        public void LoadStoriesFromDisk()
        {
            GameData.Instance = new GameData
            {
                StoryContext = new StoryContext
                {
                    PlayedStories = new List<IStory>(),
                    Stories = new List<IStory>(),
                    StoryImagesFolder = new DirectoryInfo(Path + "\\StoryImages"),
                    ModuleFolder = new DirectoryInfo(Path),
                    CustomStoriesFolder = new DirectoryInfo(Path + "\\CustomStories")
                }
            };

            GameData.Instance.StoryContext.Stories = GameData.Instance.StoryContext.ImportStoriesFromDisk();
        }


        public void LoadStoriesFromDisk(string fileName)
        {
            GameData.Instance = new GameData
            {
                StoryContext = new StoryContext
                {
                    PlayedStories = new List<IStory>(),
                    Stories = new List<IStory>(),
                    StoryImagesFolder = new DirectoryInfo(Path + "\\StoryImages"),
                    ModuleFolder = new DirectoryInfo(Path),
                    CustomStoriesFolder = new DirectoryInfo(Path + "\\CustomStories")
                }
            };

            var f = new FileInfo(GameData.Instance.StoryContext.CustomStoriesFolder + "\\" + fileName);

            GameData.Instance.StoryContext.Stories = GameData.Instance.StoryContext.ImportStoriesFromDisk(f);
        }


        public void SetupKissTheBanner()
        {
            var sut = KissTheBannerStory();
            GameData.Instance = KissTheBannerGameData();
            GameData.Instance.StoryContext.Stories.Add(sut);
        }

        #region private

        private GameData KissTheBannerGameData()
        {
            var g = new GameData
            {
                GameContext = new GameContext
                {
                    Tracking =
                    {
                        IsCurrentlyInSettlement = false,
                        IsCurrentlyOnMap = true
                    },
                    Time =
                    {
                        IsNight = false,
                        IsDay = true
                    },
                    Heroes =
                    {
                        Player = new BaseHero
                        {
                            Age = 18,
                            IsFemale = true,
                            IsHumanPlayerCharacter = true
                        },
                        PlayerIsCaptor = true
                    }
                },
                StoryContext = new StoryContext
                {
                    PlayedStories = new List<IStory>(),
                    Stories = new List<IStory>(),
                    StoryImagesFolder = new StoryLoader().GetStoryImagesDirectoryInfo(), //new DirectoryInfo(p + "\\StoryImages"),
                    ModuleFolder = new StoryLoader().GetModuleDirectoryInfo(),
                    CustomStoriesFolder = new StoryLoader().GetCustomStoriesDirectoryInfo()
                }
            };

            return g;
        }

        private Story KissTheBannerStory()
        {
            var s = new Story
            {
                Header = new StoryHeader
                {
                    CanBePlayedOnlyOnce = true,
                    DependOn = "None",
                    Time = GameTime.Daytime,
                    TypeOfStory = StoryType.PlayerIsCaptor
                },
                Restrictions = new List<IEvaluation>
                {
                    new BaseEvaluation
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
                    },
                    new BaseEvaluation
                    {
                        Persona = new Persona
                        {
                            Subject = Actor.Player,
                            Characteristic = Characteristics.Gender
                        },
                        Numbers = new Numbers
                        {
                            Operator = Operator.Equalto,
                            Value = "female"
                        }
                    },
                    new BaseEvaluation
                    {
                        Persona = new Persona
                        {
                            Subject = Actor.Npc
                        },
                        PartyType = PartyType.Lord,
                        Numbers = new Numbers
                        {
                            Operator = Operator.Equalto,
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
                        Location = Location.Map,
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
                                            Subject = Actor.Player,
                                            Characteristic = Characteristics.Renown
                                        },
                                        Numbers = new Numbers
                                        {
                                            Operator = Operator.Equalto,
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
                        Location = Location.Map,
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

            return s;
        }

        #endregion
    }
}