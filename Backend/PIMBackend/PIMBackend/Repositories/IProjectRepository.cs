using PIMBackend.Domain.Entities;

namespace PIMBackend.Repositories
{
    /// <summary>
    ///     Example repository interface
    /// </summary>
    public interface IProjectRepository : IRepository<Project>
    {
        bool ProjectNumberExists(decimal pjNum);
        void AddWithMem(string memString, Project project);
        void UpdateWithMem(string memString, Project projectDb);
    }
}