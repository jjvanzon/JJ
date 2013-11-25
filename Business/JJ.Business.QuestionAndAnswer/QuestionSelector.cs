using JJ.Framework.Maths;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer
{
    public class QuestionSelector
    {
        private readonly IQuestionRepository _questionRepository;

        private readonly List<int> _ids;

        public QuestionSelector(IQuestionRepository questionRepository, params Category[] categories)
        {
            if (questionRepository == null) throw new ArgumentNullException("questionRepository");
            if (categories == null) throw new ArgumentNullException("categories");

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
