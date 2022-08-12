using System.Collections.Generic;
using PIMBackend.Domain.Entities;

namespace PIMBackend.Services
{
    /// <summary>
    ///     Example of sample service
    /// </summary>
    public interface IGroupService
    {
        IEnumerable<Group> Get();

        Group Get(decimal id);

        //used to return Group
        void Create(Group sample);

        Group Update(Group sample);

        void Delete(decimal id);
    }
}