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
        private IQuestionRepository _questionRepository;

        private List<int> _ids = new List<int>();

        public QuestionSelector(IQuestionRepository questionRepository, params Category[] categories)
        {
            if (questionRepository == null) throw new ArgumentNullException("questionRepository");
            if (categories == null) throw new ArgumentNullException("categories");

            _questionRepository = questionRepository;

            foreach (Category category in categories)
            {
                var ids = _questionRepository.GetQuestionIDsByCategory(category);
                _ids.AddRange(ids);
            }

            _ids = _ids.Distinct().ToList();
        }

        public Question TryGetRandomQuestion()
        {
            int id = Randomizer.GetRandomItem(_ids);
            return _questionRepository.Get(id);
        }
    }
}
