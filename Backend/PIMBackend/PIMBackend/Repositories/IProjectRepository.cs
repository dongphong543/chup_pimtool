using PIMBackend.Domain.Entities;

namespace PIMBackend.Repositories
{
    /// <summary>
    ///     Example repository interface
    /// </summary>
    public interface IProjectRepository : IRepository<Project>
    {
        bool ProjectNumberExists(decimal pjNum);
        void AddMemberChange(string memString, Project project);
        void UpdateMemberChange(string memString, Project projectDb);
    }
}