using bitirmeProje.Domain.DataBaseContext;
using bitirmeProje.Domain.Entities;
using bitirmeProje.Domain.IRepositories;

namespace bitirmeProje.Domain.Repositories
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        public CityRepository(Context context) : base(context)
        {
        }
    }
}
