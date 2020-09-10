// Code written by Gabriel Mailhot, 09/09/2020.

#region

using System.Collections.Generic;
using System.IO;
using TalesContract;
using TalesEntities.Stories;
using TalesEntities.TW;
using TalesEnums;
using TalesPersistence;

#endregion

namespace BannerlordTales.Tests
{
   public class Stories
   {
      public GameData KissTheBannerGameData()
      {
         string p = "F:\\Program Files (x86)\\Steam\\steamapps\\common\\Mount & Blade II Bannerlord\\Modules\\LogRaamBannerlordTales";
         GameData g = new GameData {
            GameContext = new GameContext {
               IsCurrentlyInSettlement = false,
               IsCurrentlyOnMap = true,
               IsNight = false,
               PlayerIsCaptive = false,
               PlayerIsCaptor = true,
               IsDay = true
            },
            StoryContext = new StoryContext {
               PlayedStories = new List<IStory>(),
               StoryImageFiles = new List<FileInfo>(),
               Stories = new List<IStory>(),
               StoryImagesFolder = new DirectoryInfo(p + "\\StoryImages"),
               ModuleFolder = new DirectoryInfo(p),
               CustomStoriesFolder = new DirectoryInfo(p + "\\CustomStories")
            }
         };

         return g;
      }

      public Story KissTheBannerStory()
      {
         Story s = new Story {
            Header = new StoryHeader {
               CanBePlayedOnlyOnce = true,
               DependOn = "None",
               Time = GameTime.DAYTIME,
               TypeOfStory = StoryType.PLAYER_IS_CAPTOR
            },
            Restrictions = new List<IEvaluation> {
               new Evaluation {
                  Subject = Actor.PLAYER,
                  Characteristic = Characteristics.AGE,
                  Operator = Operator.GREATERTHAN,
                  Value = "18"
               },
               new Evaluation {
                  Subject = Actor.PLAYER,
                  Characteristic = Characteristics.GENDER,
                  Operator = Operator.EQUALTO,
                  Value = "female"
               },
               new Evaluation {
                  Subject = Actor.NPC,
                  PartyType = PartyType.LORD,
                  Operator = Operator.EQUALTO,
                  Value = "Noble"
               }
            },
            Acts = new List<IAct> {
               new Act {
                  Name = "Kiss the Banner",
                  Image = "None",
                  Location = Location.MAP,
                  Intro = "Your captors demand, in order to inspire the people, that their captives kiss their banner. It's your turn.",
                  Choices = new List<IChoice> {
                     new Choice {
                        Text = "Kiss the banner.",
                        Consequences = {
                           new Evaluation {
                              Subject = Actor.PLAYER,
                              Characteristic = Characteristics.RENOWN,
                              Operator = Operator.EQUALTO,
                              Value = "-1"
                           }
                        }
                     },
                     new Choice {
                        Text = "Refuse.",
                        Triggers = {
                           new Trigger {
                              ChanceToTrigger = 100,
                              Link = "Refuse to kiss the banner"
                           }
                        }
                     }
                  }
               },
               new Sequence {
                  Name = "Refuse to kiss the banner",
                  Image = "None",
                  Location = Location.MAP,
                  Intro = "Your captors wrap you in their banner and beat your body with clubs.",
                  Choices = new List<IChoice> {
                     new Choice {
                        Text = "Endure the beating.",
                        Consequences = {
                           new Evaluation {
                              Subject = Actor.PLAYER,
                              Characteristic = Characteristics.HEALTH,
                              Operator = Operator.EQUALTO,
                              Value = "-10"
                           }
                        }
                     }
                  }
               }
            }
         };

         return s;
      }

      public void LoadStoriesFromDisk()
      {
         string p = "F:\\Program Files (x86)\\Steam\\steamapps\\common\\Mount & Blade II Bannerlord\\Modules\\LogRaamBannerlordTales";

         GameData.Instance = new GameData {
            StoryContext = new StoryContext {
               PlayedStories = new List<IStory>(),
               StoryImageFiles = new List<FileInfo>(),
               Stories = new List<IStory>(),
               StoryImagesFolder = new DirectoryInfo(p + "\\StoryImages"),
               ModuleFolder = new DirectoryInfo(p),
               CustomStoriesFolder = new DirectoryInfo(p + "\\CustomStories")
            }
         };

         GameData.Instance.StoryContext.Stories = GameData.Instance.StoryContext.ImportStoriesFromDisk();
      }
      //TODO: Should import the story instead of creating it with code.


      public void SetupKissTheBanner()
      {
         Story sut = KissTheBannerStory();
         GameData.Instance = KissTheBannerGameData();
         GameData.Instance.StoryContext.Stories.Add(sut);
      }
   }
}