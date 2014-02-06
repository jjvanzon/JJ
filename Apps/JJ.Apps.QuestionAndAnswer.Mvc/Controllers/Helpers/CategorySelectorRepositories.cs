﻿using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JJ.Apps.QuestionAndAnswer.Mvc.Controllers.Helpers
{
    public class CategorySelectorRepositories : IDisposable
    {
        private IDisposable _underlyingDataStore;

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
            IUserRepository userRepository,
            IDisposable underlyingDataStore = null)
        {
            if (categoryRepository == null) throw new ArgumentNullException("categoryRepository");
            if (questionRepository == null) throw new ArgumentNullException("questionRepository");
            if (questionFlagRepository == null) throw new ArgumentNullException("questionFlagRepository");
            if (flagStatusRepository == null) throw new ArgumentNullException("flagStatusRepository");
            if (userRepository == null) throw new ArgumentNullException("userRepository");

            CategoryRepository = categoryRepository;
            QuestionRepository = questionRepository;
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