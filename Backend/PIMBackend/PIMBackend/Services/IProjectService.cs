using System.Collections.Generic;
using PIMBackend.Domain.Entities;

namespace PIMBackend.Services
{
    /// <summary>
    ///     Example of sample service
    /// </summary>
    public interface IProjectService
    {
        IEnumerable<Project> Get(string searchText, string searchCriteria);

        Project Get(decimal id);

        //used to return Project
        void Create(Project sample);

        void CreateWithMem(Project project, string memString);

        Project Update(Project project, string memString);

        void Delete(decimal[] pjNums);
        Project GetByPjNum(decimal pjNum);

        bool ProjectNumberExists(decimal pjNum);

    }
}