// Code written by Gabriel Mailhot, 02/12/2023.

#region

using FluentAssertions;
using NUnit.Framework;
using System.IO;

#endregion

namespace TalesDAL.Tests
{
    [TestFixture]
    public class BodyArmorImporterTests
    {
        [Test]
        public void LoadXml_ShouldWork()
        {
            //Arrange
            var file = new FileInfo("I:\\SteamLibrary\\steamapps\\common\\Mount & Blade II Bannerlord\\Modules\\SandBoxCore\\ModuleData\\items\\body_armors.xml");
            var sut = new BodyArmorImporter();

            //Act
            var actualResult = sut.ImportBodyArmorsFrom(file);

            //Assert
            actualResult.Count.Should().BeGreaterThan(0);
        }
    }
}