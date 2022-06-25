using System.Collections.Generic;
using PIMBackend.Domain.Entities;
using PIMBackend.Domain.Objects;

namespace PIMBackend.Services
{
    /// <summary>
    ///     Example of sample service
    /// </summary>
    public interface IProjectService
    {
        IEnumerable<Project> Get(string searchText, string searchCriteria);

        IEnumerable<Project> Get(Filter filter);

        Project Get(decimal id);

        //used to return Project
        void Create(Project sample);

        void CreateWithMem(Project project, string memString);

        Project Update(Project sample);

        void Delete(decimal[] ids);
        Project GetByPjNum(decimal pjNum);
        Project UpdateWithMem(Project project, string memString);
    }
}