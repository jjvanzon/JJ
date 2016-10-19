using JJ.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Common;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories
{
    public class QuestionRepository : RepositoryBase<Question, int>, IQuestionRepository
    {
        public QuestionRepository(IContext context)
            : base(context)
        { }

        public virtual Question TryGetRandomQuestion()
        {
            throw new NotImplementedException();
        }

        public IList<Question> GetBySourceID(int sourceID)
        {
            return _context.Query<Question>().Where(x => x.Source.ID == sourceID).ToArray();
        }

        // TODO: Handle circularities.

        public IList<int> GetQuestionIDsByCategory(Category category)
        {
            if (category == null) throw new NullException(() => category);

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
        public IList<Question> GetByCriteria(bool mustFilterByFlagStatusID, int? flagStatusID)
        {
            if (!mustFilterByFlagStatusID)
            {
                return GetAll().ToArray();
            }
            else
            {
                if (!flagStatusID.HasValue)
                {
                    return _context.Query<Question>().Where(x => x.QuestionFlags.Count == 0).ToArray();
                }
                else
                {
                    return _context.Query<QuestionFlag>().Where(x => x.FlagStatus.ID == flagStatusID).Select(x => x.Question).Distinct().ToArray();
                }
            }
        }

        public virtual IList<Question> GetPage(int firstIndex, int count)
        {
            return _context.Query<Question>().Skip(firstIndex).Take(count).ToArray();
        }

        public virtual int CountAll()
        {
            throw new NotImplementedException();
        }
    }
}
