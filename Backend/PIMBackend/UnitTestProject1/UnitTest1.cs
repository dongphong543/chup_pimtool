using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using PIMBackend.Domain.Entities;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Create()
        {
            // Arrange
            var newSample = new Project { Id = -1, Details = "New sample" };

            // Act
            _sampleService.Create(newSample);
            var result = _sampleService.Get().SingleOrDefault(x => x.Details == newSample.Details);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("New sample", result.Details);
            Assert.AreNotEqual(-1, result.Id);

            Assert.
        }
    }
}
