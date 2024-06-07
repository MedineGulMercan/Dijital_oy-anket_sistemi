using bitirmeProje.Domain.DataBaseContext;
using bitirmeProje.Domain.Entities;
using bitirmeProje.Domain.IRepositories;

namespace bitirmeProje.Domain.Repositories
{
    public class QuestionTypeRepository : Repository<QuestionType>, IQuestionTypeRepository
    {
        public QuestionTypeRepository(Context context) : base(context)
        {
        }
    }
}
