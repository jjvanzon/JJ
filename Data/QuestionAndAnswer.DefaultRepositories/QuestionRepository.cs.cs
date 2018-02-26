using JJ.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Exceptions;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories
{
	public class QuestionRepository : RepositoryBase<Question, int>, IQuestionRepository
	{
		public QuestionRepository(IContext context)
			: base(context)
		{ }

		[Obsolete("Consider using more efficient queries instead.")]
		public virtual IList<Question> GetAll() => _context.Query<Question>().ToArray();

		public virtual Question TryGetRandomQuestion()
		{
			throw new NotImplementedException();
		}

		public virtual IList<Question> GetBySourceID(int sourceID)
		{
			return _context.Query<Question>()
						   .Where(x => x.Source.ID == sourceID)
						   .ToArray();
		}

		// TODO: GetQuestionIDsByCategory and GetQuestionIDsByCategoryRecursive belong in the business layer.
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
				IList<int> ids2 = GetQuestionIDsByCategory(subCategory);
				ids.AddRange(ids2);
			}

			return ids;
		}

		public virtual IList<Question> GetByCriteria(bool mustFilterByFlagStatusID, int? flagStatusID)
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

		public virtual int Count()
		{
			throw new NotImplementedException();
		}
	}
}
