using bitirmeProje.Domain.DataBaseContext;
using bitirmeProje.Domain.Entities;
using bitirmeProje.Domain.IRepositories;

namespace bitirmeProje.Domain.Repositories
{
    public class OptionRepository : Repository<Option>,IOptionRepository
    {
        public OptionRepository(Context context) : base(context)
        {
        }
    }
}
