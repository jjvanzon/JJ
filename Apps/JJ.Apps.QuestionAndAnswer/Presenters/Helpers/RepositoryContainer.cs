using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.Presenters.Helpers
{
    public class RepositoryContainer
    {
        public IQuestionRepository QuestionRepository { get; private set; }
        public IAnswerRepository AnswerRepository { get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }
        public IQuestionCategoryRepository QuestionCategoryRepository { get; private set; }
        public IQuestionLinkRepository QuestionLinkRepository { get; private set; }
        public IQuestionFlagRepository QuestionFlagRepository { get; private set; }
        public IFlagStatusRepository FlagStatusRepository { get; private set; }
        public ISourceRepository SourceRepository { get; private set; }
        public IQuestionTypeRepository QuestionTypeRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }

        public RepositoryContainer(
            IQuestionRepository questionRepository,
            IAnswerRepository answerRepository,
            ICategoryRepository categoryRepository,
            IQuestionCategoryRepository questionCategoryRepository,
            IQuestionLinkRepository questionLinkRepository,
            IQuestionFlagRepository questionFlagRepository,
            IFlagStatusRepository flagStatusRepository,
            ISourceRepository sourceRepository,
            IQuestionTypeRepository questionTypeRepository,
            IUserRepository userRepository)
        {
            if (questionRepository == null) throw new ArgumentNullException("questionRepository");
            if (answerRepository == null) throw new ArgumentNullException("answerRepository");
            if (categoryRepository == null) throw new ArgumentNullException("categoryRepository");
            if (questionCategoryRepository == null) throw new ArgumentNullException("questionCategoryRepository");
            if (questionLinkRepository == null) throw new ArgumentNullException("questionLinkRepository");
            if (questionFlagRepository == null) throw new ArgumentNullException("questionFlagRepository");
            if (sourceRepository == null) throw new ArgumentNullException("sourceRepository");
            if (questionTypeRepository == null) throw new ArgumentNullException("questionTypeRepository");
            if (userRepository == null) throw new ArgumentNullException("userRepository");

            QuestionRepository = questionRepository;
            AnswerRepository = answerRepository;
            CategoryRepository = categoryRepository;
            QuestionCategoryRepository = questionCategoryRepository;
            QuestionLinkRepository = questionLinkRepository;
            QuestionFlagRepository = questionFlagRepository;
            FlagStatusRepository = flagStatusRepository;
            SourceRepository = sourceRepository;
            QuestionTypeRepository = questionTypeRepository;
            UserRepository = userRepository;
        }
    }
}
