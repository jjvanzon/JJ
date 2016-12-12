using JJ.Framework.Exceptions;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.Helpers
{
    internal class CategorySelectorRepositories
    {
        public ICategoryRepository CategoryRepository { get; private set; }
        public IQuestionRepository QuestionRepository { get; private set; }
        public IQuestionFlagRepository QuestionFlagRepository { get; private set; }
        public IFlagStatusRepository FlagStatusRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }

        public CategorySelectorRepositories(
            ICategoryRepository categoryRepository,
            IQuestionRepository questionRepository,
            IQuestionFlagRepository questionFlagRepository,
            IFlagStatusRepository flagStatusRepository,
            IUserRepository userRepository)
        {
            if (categoryRepository == null) throw new NullException(() => categoryRepository);
            if (questionRepository == null) throw new NullException(() => questionRepository);
            if (questionFlagRepository == null) throw new NullException(() => questionFlagRepository);
            if (flagStatusRepository == null) throw new NullException(() => flagStatusRepository);
            if (userRepository == null) throw new NullException(() => userRepository);

            CategoryRepository = categoryRepository;
            QuestionRepository = questionRepository;
            QuestionFlagRepository = questionFlagRepository;
            FlagStatusRepository = flagStatusRepository;
            UserRepository = userRepository;
        }
    }
}