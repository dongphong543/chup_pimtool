using PIMBackend.Database;
using PIMBackend.Domain.Entities;

namespace PIMBackend.Repositories.Imp
{
    /// <summary>
    ///     The implementation of sample repository
    /// </summary>
    public class GroupRepository : BaseRepository<Group>, IGroupRepository
    {
        public GroupRepository(PIMContext context) : base(context)
        {
        }
    }
}