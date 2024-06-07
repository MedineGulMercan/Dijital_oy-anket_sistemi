using bitirmeProje.Domain.DataBaseContext;
using bitirmeProje.Domain.Entities;
using bitirmeProje.Domain.IRepositories;

namespace bitirmeProje.Domain.Repositories
{
    public class CountryRepository : Repository<Country>,ICountryRepository
    {
        public CountryRepository(Context context) : base(context)
        {
        }
    }
}
