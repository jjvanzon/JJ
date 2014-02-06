using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JJ.Apps.QuestionAndAnswer.Mvc.Controllers.Helpers
{
    public class UserRepositoryWrapper : IDisposable
    {
        private IDisposable _underlyingDataStore;

        public IUserRepository UserRepository { get; private set; }

        public UserRepositoryWrapper(IUserRepository userRepository, IDisposable _underlyingDataStore = null)
        {
            if (userRepository == null) throw new ArgumentNullException("userRepository");
            UserRepository = userRepository;
            _underlyingDataStore = _underlyingDataStore;
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