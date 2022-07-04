using System.Linq;
using Autofac;
using NUnit.Framework;
using PIMToolCodeBase.Domain.Entities;
using PIMToolCodeBase.Services;

namespace PIMToolCodeBase.Test.Services
{
    [TestFixture]
    public class SampleServiceTest : BaseTest
    {
        [SetUp]
        public void SetUp()
        {
            _sampleService = Container.Resolve<ISampleService>();
        }

        private ISampleService _sampleService;

        /// <summary>
        ///     This test used to test the create method of service
        /// </summary>
        [Test]
        public void Create()
        {
            // Arrange
            var newSample = new Sample {Id = -1, Details = "New sample"};

            // Act
            _sampleService.Create(newSample);
            var result = _sampleService.Get().SingleOrDefault(x => x.Details == newSample.Details);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("New sample", result.Details);
            Assert.AreNotEqual(-1, result.Id);
        }

        /// <summary>
        ///     This test used to test the update method of service
        /// </summary>
        [Test]
        public void Delete()
        {
            // Arrange
            Context.Samples.AddRange(new[] {new Sample {Details = "To delete sample"}});
            Context.SaveChanges();

            var sample = _sampleService.Get().Single(x => x.Details == "To delete sample");

            // Act
            _sampleService.Delete(sample.Id);
            var result = _sampleService.Get(sample.Id);

            // Assert
            Assert.IsNull(result);
        }

        /// <summary>
        ///     This test used to test the get method of service
        /// </summary>
        [Test]
        public void Get()
        {
            // Arrange
            Context.Samples.AddRange(new[] {new Sample {Details = "Sample1"}, new Sample {Details = "Sample2"}});
            Context.SaveChanges();

            // Act
            var result = _sampleService.Get().ToArray();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length >= 2);
            Assert.IsTrue(result.Any(x => x.Details == "Sample1"));
            Assert.IsTrue(result.Any(x => x.Details == "Sample2"));
            Assert.IsFalse(result.Any(x => x.Details == "Sample"));
        }

        /// <summary>
        ///     This test used to test the update method of service
        /// </summary>
        [Test]
        public void Update()
        {
            // Arrange
            Context.Samples.AddRange(new[] {new Sample {Details = "Old sample"}});
            Context.SaveChanges();

            var sample = _sampleService.Get().Single(x => x.Details == "Old sample");

            // Act
            _sampleService.Update(new Sample {Id = sample.Id, Details = "Empty"});
            var result = _sampleService.Get(sample.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Empty", result.Details);
            Assert.AreEqual(sample.Id, result.Id);
        }
    }
}