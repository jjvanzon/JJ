using System.Data.Entity;

namespace JJ.Data.QuestionAndAnswer.EntityFramework.Mapping
{
    public class QuestionAndAnswerContext : DbContext
    {
        public QuestionAndAnswerContext(string specialConnectionString)
            : base(specialConnectionString)
        { }
    }
}
