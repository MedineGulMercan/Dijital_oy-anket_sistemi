using bitirmeProje.Domain.DataBaseContext;
using bitirmeProje.Domain.Entities;
using bitirmeProje.Domain.IRepositories;

namespace bitirmeProje.Domain.Repositories
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(Context context) : base(context)
        {
        }
    }
}
