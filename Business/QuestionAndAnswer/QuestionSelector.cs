using JJ.Framework.Mathematics;
using JJ.Framework.Exceptions;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace JJ.Business.QuestionAndAnswer
{
    public class QuestionSelector
    {
        private readonly IQuestionRepository _questionRepository;

        private readonly List<int> _ids;

        public QuestionSelector(IQuestionRepository questionRepository, IEnumerable<Category> categories)
            : this(questionRepository, categories.ToArray())
        { }

        public QuestionSelector(IQuestionRepository questionRepository, params Category[] categories)
        {
            if (questionRepository == null) throw new NullException(() => questionRepository);
            if (categories == null) throw new NullException(() => categories);

            _questionRepository = questionRepository;

            _ids = new List<int>();

            foreach (Category category in categories)
            {
                var ids = _questionRepository.GetQuestionIDsByCategory(category);
                _ids.AddRange(ids);
            }

            _ids = _ids.Distinct().ToList();
        }

        public Question TryGetRandomQuestion()
        {
            if (_ids.Count == 0)
            {
                return null;
            }

            int id = Randomizer.GetRandomItem(_ids);
            return _questionRepository.Get(id);
        }
    }
}
