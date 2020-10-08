// Code written by Gabriel Mailhot, 05/10/2020.

#region

using FluentAssertions;
using NUnit.Framework;
using TalesPersistence;
using TalesTaleWorlds.Menu;

#endregion

namespace BannerlordTales.Tests
{
    [TestFixture]
    internal class MenuBrokerTests
    {
        [Test]
        public void Test1()
        {
            //Arrange
            new Stories().LoadStoriesFromDisk();
            var sut = new MenuBroker();

            //Act
            var actualResult = sut.GetWaitingMenu();

            //Assert
            GameData.Instance.StoryContext.Stories.Count.Should().BeGreaterThan(0);
            actualResult.Should().NotStartWith("_");
        }
    }
}