// unset

#region

using BannerlordTales.Tests;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using TalesPersistence.Context;
using TalesPersistence.Entities;
using TalesPersistence.Stories;

#endregion

namespace TalesPersistenceTests
{
    [TestFixture]
    public class EvaluationTest
    {
        [Test]
        public void ApplyConsequenceInGame_PregnancyRisk_Age10_ShouldNotWork()
        {
            //Arrange
            new Stories().LoadStoriesFromDisk();
            var story = new Story(GameData.Instance.StoryContext.Stories.First(n => n.Header.Name == "Test Pregnancy"));
            var act = story.Acts.First(n => n.Name == "GetPregnant");

            GameData.Instance.GameContext.Heroes.Player = new Hero
            {
                Age = 10,
                IsPregnant = false,
                IsHumanPlayerCharacter = true,
                IsFemale = true,
                IsAlive = true,
                IsFertile = true
            };

            var sut = new Evaluation(act.Choices[0].Consequences[0]);

            //Act
            sut.ApplyConsequenceInGame();

            //Assert
            act.Choices[0].Consequences[0].Outcome.PregnancyRisk.Should().BeTrue();
            GameData.Instance.GameContext.Heroes.Player.IsPregnant.Should().BeFalse();
        }


        [Test]
        public void ApplyConsequenceInGame_PregnancyRisk_IsDead_ShouldNotWork()
        {
            //Arrange
            new Stories().LoadStoriesFromDisk();
            var story = new Story(GameData.Instance.StoryContext.Stories.First(n => n.Header.Name == "Test Pregnancy"));
            var act = story.Acts.First(n => n.Name == "GetPregnant");

            GameData.Instance.GameContext.Heroes.Player = new Hero
            {
                Age = 18,
                IsPregnant = false,
                IsHumanPlayerCharacter = true,
                IsFemale = true,
                IsAlive = false,
                IsFertile = true
            };

            var sut = new Evaluation(act.Choices[0].Consequences[0]);

            //Act
            sut.ApplyConsequenceInGame();

            //Assert
            act.Choices[0].Consequences[0].Outcome.PregnancyRisk.Should().BeTrue();
            GameData.Instance.GameContext.Heroes.Player.IsPregnant.Should().BeFalse();
        }


        [Test]
        public void ApplyConsequenceInGame_PregnancyRisk_IsMale_ShouldNotWork()
        {
            //Arrange
            new Stories().LoadStoriesFromDisk();
            var story = new Story(GameData.Instance.StoryContext.Stories.First(n => n.Header.Name == "Test Pregnancy"));
            var act = story.Acts.First(n => n.Name == "GetPregnant");

            GameData.Instance.GameContext.Heroes.Player = new Hero
            {
                Age = 18,
                IsPregnant = false,
                IsHumanPlayerCharacter = true,
                IsFemale = false,
                IsAlive = true,
                IsFertile = true
            };

            var sut = new Evaluation(act.Choices[0].Consequences[0]);

            //Act
            sut.ApplyConsequenceInGame();

            //Assert
            act.Choices[0].Consequences[0].Outcome.PregnancyRisk.Should().BeTrue();
            GameData.Instance.GameContext.Heroes.Player.IsPregnant.Should().BeFalse();
        }


        [Test]
        public void ApplyConsequenceInGame_PregnancyRisk_IsNotFertile_ShouldNotWork()
        {
            //Arrange
            new Stories().LoadStoriesFromDisk();
            var story = new Story(GameData.Instance.StoryContext.Stories.First(n => n.Header.Name == "Test Pregnancy"));
            var act = story.Acts.First(n => n.Name == "GetPregnant");

            GameData.Instance.GameContext.Heroes.Player = new Hero
            {
                Age = 18,
                IsPregnant = false,
                IsHumanPlayerCharacter = true,
                IsFemale = true,
                IsAlive = true,
                IsFertile = false
            };

            var sut = new Evaluation(act.Choices[0].Consequences[0]);

            //Act
            sut.ApplyConsequenceInGame();

            //Assert
            act.Choices[0].Consequences[0].Outcome.PregnancyRisk.Should().BeTrue();
            GameData.Instance.GameContext.Heroes.Player.IsPregnant.Should().BeFalse();
        }

        [Test]
        public void ApplyConsequenceInGame_PregnancyRisk_ShouldWork()
        {
            //Arrange
            new Stories().LoadStoriesFromDisk();
            var story = new Story(GameData.Instance.StoryContext.Stories.First(n => n.Header.Name == "Test Pregnancy"));
            var act = story.Acts.First(n => n.Name == "GetPregnant");

            GameData.Instance.GameContext.Heroes.Player = new Hero
            {
                Age = 18,
                IsPregnant = false,
                IsHumanPlayerCharacter = true,
                IsFemale = true,
                IsAlive = true,
                IsFertile = true
            };

            var sut = new Evaluation(act.Choices[0].Consequences[0]);

            //Act
            sut.ApplyConsequenceInGame();

            //Assert
            act.Choices[0].Consequences[0].Outcome.PregnancyRisk.Should().BeTrue();
            GameData.Instance.GameContext.Heroes.Player.IsPregnant.Should().BeTrue();
        }
    }
}