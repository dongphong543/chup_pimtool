using PIMBackend.Domain.Entities;

namespace PIMBackend.Repositories
{
    /// <summary>
    ///     Example repository interface
    /// </summary>
    public interface IProjectRepository : IRepository<Project>
    {
        bool ProjectNumberExists(decimal pjNum);
        void Add(string memString, Project project);
        void Update(string memString, Project projectDb);
    }
}