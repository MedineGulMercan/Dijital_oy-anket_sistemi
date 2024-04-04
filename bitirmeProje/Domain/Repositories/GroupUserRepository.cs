using bitirmeProje.Domain.DataBaseContext;
using bitirmeProje.Domain.Entities;
using bitirmeProje.Domain.IRepositories;

namespace bitirmeProje.Domain.Repositories
{
    public class GroupUserRepository : Repository<GroupUser>, IGroupUserRepository
    {
        public GroupUserRepository(Context context) : base(context)
        {
        }
    }
}
