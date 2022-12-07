//using JJ.Models.QuestionAndAnswer;
//using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace JJ.Business.QuestionAndAnswer
//{
//    public class QuestionSelector
//    {
//        private IQuestionRepository _questionRepository;
//        private ICategoryRepository _categoryRepository;
//        private CategoryManager _categoryManager;

//        public QuestionSelector(IQuestionRepository questionRepository, ICategoryRepository categoryRepository)
//        {
//            if (questionRepository == null) throw new ArgumentNullException("questionRepository");
//            if (categoryRepository == null) throw new ArgumentNullException("categoryRepository");
//            _questionRepository = questionRepository;
//            _categoryRepository = categoryRepository;

//            _categoryManager = new CategoryManager(_categoryRepository);
//        }
        
//        public Question TryGetRandomQuestion()
//        {
//            Question question = _questionRepository.TryGetRandomQuestion();

//            if (question == null)
//            {
//            }

//            while (true)
//            {
//                foreach (Category category in Categories)
//                {
//                    question = _questionRepository.TryGetRandomQuestion();

//                    if (_categoryManager.QuestionIsInCategory(question, category))
//                    {
//                        return question;
//                    }
//                }
//            }

//            return question;
//        }

//        private CategoriesClass _categories = new CategoriesClass();

//        public CategoriesClass Categories
//        {
//            get { return _categories; }
//        }

//        public class CategoriesClass : IEnumerable<Category>
//        {
//            private List<Category> _list = new List<Category>();

//            internal CategoriesClass()
//            { }

//            public void Add(Category category)
//            {
//                _list.Add(category);
//            }

//            public void Remove(Category category)
//            {
//                _list.Remove(category);
//            }

//            public IEnumerator<Category> GetEnumerator()
//            {
//                foreach (var x in _list)
//                {
//                    yield return x;
//                }
//            }

//            IEnumerator IEnumerable.GetEnumerator()
//            {
//                foreach (var x in _list)
//                {
//                    yield return x;
//                }
//            }
//        }
//    }
//}
