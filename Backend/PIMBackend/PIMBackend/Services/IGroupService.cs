using System.Collections.Generic;
using PIMBackend.Domain.Entities;
using PIMBackend.Domain.Objects;

namespace PIMBackend.Services
{
    /// <summary>
    ///     Example of sample service
    /// </summary>
    public interface IGroupService
    {
        IEnumerable<Group> Get();

        IEnumerable<Group> Get(Filter filter);

        Group Get(decimal id);

        //used to return Group
        void Create(Group sample);

        Group Update(Group sample);

        void Delete(decimal id);
    }
}