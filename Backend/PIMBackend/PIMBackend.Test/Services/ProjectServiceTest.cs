using System.Linq;
using Autofac;
using NUnit.Framework;
using PIMBackend.Database;
using PIMBackend.Domain.Entities;
using PIMBackend.Repositories.Imp;
using PIMBackend.Services;
using PIMBackend.Services.Imp;

namespace PIMBackend.Test.Services
{
    [TestFixture]
    public class ProjectServiceTest : BaseTest
    {
        [SetUp]
        public void SetUp()
        {
            Context = new PIMContext();
            _projectService = new ProjectService(new ProjectRepository(Context), new EmployeeRepository(Context));
            // ưtf
        }

        private IProjectService _projectService;

        /// <summary>
        ///     This test used to test the create method of service
        /// </summary>
        [Test]
        public void Create()
        {
            // Arrange
            byte[] ver = { };
            var newProject = new Project { GroupId = 1, ProjectNumber = 5, Name = "Please run", Customer = "null", Status = "NEW", StartDate = System.DateTime.Now, EndDate = null, Version = ver };
            string memStr = "";
            
            // Act
            _projectService.Create(newProject, memStr);
            //var result = _projectService.GetByPjNum(5).SingleOrDefault(x => x.Details == newSample.Details);
            var result = _projectService.GetByPjNum(5);

            // Assert
            Assert.IsNotNull(result);
            //Assert.AreEqual("New sample", result.Details);
            //Assert.Equals(result, newProject);
            Assert.AreNotEqual(-1, result.Id);
        }

        [Test]
        public void Get()
        {
            // Arrange
            
            var result = _projectService.GetByPjNum(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("1ne", result.Name);
        }

        /// <summary>
        ///     This test used to test the update method of service
        /// </summary>
        //[Test]
        //public void Delete()
        //{
        //    // Arrange
        //    Context.Samples.AddRange(new[] {new Sample {Details = "To delete sample"}});
        //    Context.SaveChanges();

        //    var sample = _sampleService.Get().Single(x => x.Details == "To delete sample");

        //    // Act
        //    _sampleService.Delete(sample.Id);
        //    var result = _sampleService.Get(sample.Id);

        //    // Assert
        //    Assert.IsNull(result);
        //}

        ///// <summary>
        /////     This test used to test the get method of service
        ///// </summary>
        //[Test]
        //public void Get()
        //{
        //    // Arrange
        //    Context.Samples.AddRange(new[] {new Sample {Details = "Sample1"}, new Sample {Details = "Sample2"}});
        //    Context.SaveChanges();

        //    // Act
        //    var result = _sampleService.Get().ToArray();

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.IsTrue(result.Length >= 2);
        //    Assert.IsTrue(result.Any(x => x.Details == "Sample1"));
        //    Assert.IsTrue(result.Any(x => x.Details == "Sample2"));
        //    Assert.IsFalse(result.Any(x => x.Details == "Sample"));
        //}

        ///// <summary>
        /////     This test used to test the update method of service
        ///// </summary>
        //[Test]
        //public void Update()
        //{
        //    // Arrange
        //    Context.Samples.AddRange(new[] {new Sample {Details = "Old sample"}});
        //    Context.SaveChanges();

        //    var sample = _sampleService.Get().Single(x => x.Details == "Old sample");

        //    // Act
        //    _sampleService.Update(new Sample {Id = sample.Id, Details = "Empty"});
        //    var result = _sampleService.Get(sample.Id);

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual("Empty", result.Details);
        //    Assert.AreEqual(sample.Id, result.Id);
        //}
    }
}