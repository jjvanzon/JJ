using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.Helpers
{
    public class RandomQuestionRepositories
    {
        private IDisposable _underlyingDataStore;

        public IQuestionRepository QuestionRepository { get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }
        public IQuestionFlagRepository QuestionFlagRepository { get; private set; }
        public IFlagStatusRepository FlagStatusRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }

        /// <param name="underlyingDataStore">Optional. Will be disposed when the PersistenceContainer is disposed.</param>
        public RandomQuestionRepositories(
            IQuestionRepository questionRepository,
            ICategoryRepository categoryRepository,
            IQuestionFlagRepository questionFlagRepository,
            IFlagStatusRepository flagStatusRepository,
            IUserRepository userRepository,
            IDisposable underlyingDataStore)
        {
            if (questionRepository == null) throw new ArgumentNullException("questionRepository");
            if (categoryRepository == null) throw new ArgumentNullException("categoryRepository");
            if (questionFlagRepository == null) throw new ArgumentNullException("questionFlagRepository");
            if (userRepository == null) throw new ArgumentNullException("userRepository");

            QuestionRepository = questionRepository;
            CategoryRepository = categoryRepository;
            QuestionFlagRepository = questionFlagRepository;
            FlagStatusRepository = flagStatusRepository;
            UserRepository = userRepository;

            _underlyingDataStore = underlyingDataStore;
        }

        public void Dispose()
        {
            if (_underlyingDataStore != null)
            {
                _underlyingDataStore.Dispose();
            }
        }
    }
}
