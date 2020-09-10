// Code written by Gabriel Mailhot, 29/08/2020.

#region

using FluentAssertions;
using NUnit.Framework;
using TalesEntities.Stories;
using TalesEnums;
using TalesPersistence;

#endregion

namespace BannerlordTales.Tests
{
   #region

   #endregion

   [TestFixture]
   public class StoryTests
   {
      [Test]
      public void IsQualifiedEventFor_QualifiedStory1_ReturnTrue()
      {
         // Arrange
         GameData.Instance = new GameData {
            GameContext = new GameContext {
               PlayerIsCaptor = true,
               IsDay = true
            }
         };

         Story sut = new Story {
            Header = new StoryHeader {
               Time = GameTime.DAYTIME,
               TypeOfStory = StoryType.PLAYER_IS_CAPTOR
            }
         };

         // Act
         bool actualResult = sut.IsQualifiedRightNow();

         // Assert
         actualResult.Should().BeTrue();
      }

      [Test]
      public void IsQualifiedEventFor_QualifiedStory2_ReturnTrue()
      {
         // Arrange
         GameData.Instance = new GameData {
            GameContext = new GameContext {
               PlayerIsCaptive = true,
               IsDay = true
            }
         };

         Story sut = new Story {
            Header = new StoryHeader {
               Time = GameTime.DAYTIME,
               TypeOfStory = StoryType.PLAYER_IS_CAPTIVE
            }
         };

         // Act
         bool actualResult = sut.IsQualifiedRightNow();

         // Assert
         actualResult.Should().BeTrue();
      }

      [Test]
      public void IsQualifiedEventFor_QualifiedStory3_ReturnTrue()
      {
         // Arrange
         GameData.Instance = new GameData {
            GameContext = new GameContext {
               PlayerIsCaptive = true,
               IsDay = true
            }
         };


         Story sut = new Story {
            Header = new StoryHeader {
               Time = GameTime.DAYTIME,
               TypeOfStory = StoryType.PLAYER_IS_CAPTIVE
            }
         };

         // Act
         bool actualResult = sut.IsQualifiedRightNow();

         // Assert
         actualResult.Should().BeTrue();
      }


      [Test]
      public void IsQualifiedEventFor_QualifiedStory4_ReturnTrue()
      {
         // Arrange
         new Stories().LoadStoriesFromDisk();
         Story story = new Story(GameData.Instance.StoryContext.Stories[0]);
         Act act = new Act(story.Acts[0]);

         GameData.Instance.GameContext.IsCurrentlyOnMap = true;
         GameData.Instance.GameContext.IsDay = true;

         // Act
         bool actualResult = story.IsQualifiedRightNow() && act.IsQualifiedRightNow();

         // Assert
         actualResult.Should().BeTrue();
      }


      [Test]
      public void IsQualifiedEventFor_QualifiedStory5_ReturnFalse()
      {
         // Arrange
         new Stories().SetupKissTheBanner();

         GameData.Instance.GameContext.IsCurrentlyOnMap = false;
         GameData.Instance.GameContext.IsCurrentlyInSettlement = true;

         GameData.Instance.StoryContext.Stories[0].Acts[0].Location = Location.MAP;

         Story sut = (Story) GameData.Instance.StoryContext.Stories[0];

         // Act
         bool actualResult = ((Act) sut.Acts[0]).IsQualifiedRightNow();

         // Assert
         actualResult.Should().BeFalse();
      }

      [Test]
      public void IsQualifiedEventFor_QualifiedStory6_ReturnTrue()
      {
         // Arrange
         new Stories().LoadStoriesFromDisk();
         Story story = new Story(GameData.Instance.StoryContext.Stories[0]);
         Act act = new Act(story.Acts[0]);

         GameData.Instance.GameContext.IsCurrentlyOnMap = true;
         GameData.Instance.GameContext.IsDay = true;

         // Act
         bool actualResult = story.IsQualifiedRightNow() && act.IsQualifiedRightNow();

         // Assert
         actualResult.Should().BeTrue();
      }


      [Test]
      public void IsQualifiedEventFor_QualifiedStory7_ReturnTrue() //TODO: tester le fichier de test
      {
         // Arrange
         new Stories().LoadStoriesFromDisk();
         Story story = new Story(GameData.Instance.StoryContext.Stories[0]);
         Act act = new Act(story.Acts[0]);

         GameData.Instance.GameContext.IsCurrentlyOnMap = true;
         GameData.Instance.GameContext.IsDay = true;

         // Act
         bool actualResult = story.IsQualifiedRightNow() && act.IsQualifiedRightNow();

         // Assert
         actualResult.Should().BeTrue();
      }
   }
}