using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer.Persistence.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Framework.Common;

namespace JJ.Models.QuestionAndAnswer.Persistence.Repositories
{
    public class QuestionRepository : RepositoryBase<Question, int>, IQuestionRepository
    {
        private SqlExecutor _sqlExecutor;

        public QuestionRepository(IContext context, string sqlConnectionString)
            : base(context)
        {
            _sqlExecutor = new SqlExecutor(sqlConnectionString);
        }

        public Question TryGetRandomQuestion()
        {
            int? randomID = _sqlExecutor.TryGetRandomQuestionID();
            if (randomID.HasValue)
            {
                return Get(randomID.Value);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<Question> GetBySourceID(int sourceID)
        {
            return _context.Query<Question>().Where(x => x.Source.ID == sourceID);
        }

        // TODO: Handle circularities.

        public IList<int> GetQuestionIDsByCategory(Category category)
        {
            if (category == null) throw new ArgumentNullException("category");

            IList<int> ids = GetQuestionIDsByCategoryRecursive(category);
            return ids.Distinct().ToArray();
        }

        private IList<int> GetQuestionIDsByCategoryRecursive(Category category)
        {
            List<int> ids = category.CategoryQuestions.Select(x => x.Question.ID).ToList();

            foreach (Category subCategory in category.SubCategories)
            {
                var ids2 = GetQuestionIDsByCategory(subCategory);
                ids.AddRange(ids2);
            }

            return ids;
        }

        /// <summary>
        /// mustFilterByFlagStatusID = false and flagStatusID = null are two 
        /// different things. mustFilterByFlagStatusID = false means all Questions are returned.
        /// flagStatusID = null means only questions without a flagging are returned.
        /// </summary>
        public IEnumerable<Question> GetByCriteria(bool mustFilterByFlagStatusID, int? flagStatusID)
        {
            if (!mustFilterByFlagStatusID)
            {
                return GetAll();
            }
            else
            {
                if (!flagStatusID.HasValue)
                {
                    return _context.Query<Question>().Where(x => x.QuestionFlags.Count == 0);
                }
                else
                {
                    return _context.Query<QuestionFlag>().Where(x => x.FlagStatus.ID == flagStatusID).Select(x => x.Question).Distinct();
                }
            }
        }
    }
}
