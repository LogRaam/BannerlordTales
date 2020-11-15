// Code written by Gabriel Mailhot, 12/11/2020.

#region

using System.IO;
using FluentAssertions;
using NUnit.Framework;

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
            var file = new FileInfo("D:\\Program Files (x86)\\Steam\\steamapps\\common\\Mount & Blade II Bannerlord\\Modules\\SandBoxCore\\ModuleData\\spitems\\body_armors.xml");
            var sut = new BodyArmorImporter();

            //Act
            var actualResult = sut.ImportBodyArmorsFrom(file);

            //Assert
            actualResult.Count.Should().BeGreaterThan(0);
        }
    }
}